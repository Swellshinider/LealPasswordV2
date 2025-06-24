using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace LealPasswordV2.Core;

/// <summary>
/// Provides methods for encrypting and decrypting strings and for managing password hashing and verification.
/// </summary>
public static class Security
{
    private static readonly int Argon2Iterations = 4;
    private static readonly int Argon2Parallelism = 2;
    private static readonly int Argon2MemorySize = 65536;

    /// <summary>
    /// Generates a secure password
    /// </summary>
    /// <param name="length">The length of the password. Must be at least 4.</param>
    /// <returns>A secure password string.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the specified password length is less than 4.
    /// </exception>
    public static string GenerateSecurePassword(int length)
    {
        if (length < 4)
            throw new ArgumentException("Password length must be at least 4 to include all required character types.");

        const string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string lowercase = "abcdefghijklmnopqrstuvwxyz";
        const string digits = "0123456789";
        const string special = "!@#$%^&*=+.?";

        // Ensure one character from each required set
        var password = new char[length];

        using var rng = RandomNumberGenerator.Create();
        password[0] = uppercase[GetRandomInt(rng, uppercase.Length)];
        password[1] = lowercase[GetRandomInt(rng, lowercase.Length)];
        password[2] = digits[GetRandomInt(rng, digits.Length)];
        password[3] = special[GetRandomInt(rng, special.Length)];

        // Fill remaining characters from all sets combined
        var allChars = uppercase + lowercase + digits + special;

        for (int i = 4; i < length; i++)
            password[i] = allChars[GetRandomInt(rng, allChars.Length)];

        ShuffleArray(rng, ref password);

        return new string(password);
    }

    /// <summary>
    /// Encrypts the specified target text using the provided password.
    /// The encrypted output includes a randomly generated salt prepended to the ciphertext.
    /// </summary>
    /// <param name="targetText">The text to encrypt.</param>
    /// <param name="password">The password to use for encryption.</param>
    /// <returns>The encrypted text as a base64 string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when targetText or password is null or empty.</exception>
    public static string Encrypt(this string targetText, string password)
    {
        if (string.IsNullOrEmpty(targetText))
            throw new ArgumentNullException(nameof(targetText), "Target text cannot be null or empty.");

        if (string.IsNullOrEmpty(password))
            throw new ArgumentNullException(nameof(password), "Password cannot be null or empty.");

        var salt = GenerateSalt();

        DeriveKeyAndIv(password, salt, out var key, out var iv);

        byte[] encrypted;

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = key;
            aesAlg.IV = iv;
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using var msEncrypt = new MemoryStream();
            using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using var swEncrypt = new StreamWriter(csEncrypt);

            swEncrypt.Write(targetText);

            encrypted = msEncrypt.ToArray();
        }

        // Prepend the salt to the ciphertext so that decryption can use it.
        byte[] saltedCipherText = new byte[salt.Length + encrypted.Length];
        Array.Copy(salt, saltedCipherText, salt.Length);
        Array.Copy(encrypted, 0, saltedCipherText, salt.Length, encrypted.Length);

        return Convert.ToBase64String(saltedCipherText);
    }

    /// <summary>
    /// Decrypts the specified target text using the provided password.
    /// Expects the encrypted text to have the salt prepended.
    /// </summary>
    /// <param name="targetText">The text to decrypt, in base64 format.</param>
    /// <param name="password">The password to use for decryption.</param>
    /// <returns>The decrypted text.</returns>
    /// <exception cref="ArgumentNullException">Thrown when targetText or password is null or empty.</exception>
    public static string Decrypt(this string targetText, string password)
    {
        if (string.IsNullOrEmpty(targetText))
            throw new ArgumentNullException(nameof(targetText), "Target text cannot be null or empty.");

        if (string.IsNullOrEmpty(password))
            throw new ArgumentNullException(nameof(password), "Password cannot be null or empty.");

        byte[] saltedCipherText = Convert.FromBase64String(targetText);
        byte[] salt = new byte[16]; // Default salt size
        byte[] encrypted = new byte[saltedCipherText.Length - salt.Length];
        Array.Copy(saltedCipherText, salt, salt.Length);
        Array.Copy(saltedCipherText, salt.Length, encrypted, 0, encrypted.Length);

        DeriveKeyAndIv(password, salt, out var key, out var iv);

        using Aes aesAlg = Aes.Create();
        aesAlg.Key = key;
        aesAlg.IV = iv;
        aesAlg.Mode = CipherMode.CBC;
        aesAlg.Padding = PaddingMode.PKCS7;

        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        using var msDecrypt = new MemoryStream(encrypted);
        using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        using var srDecrypt = new StreamReader(csDecrypt);

        return srDecrypt.ReadToEnd();
    }

    /// <summary>
    /// Generates a hashed value for the specified password using the Argon2id algorithm.
    /// A new random salt is generated and returned via the out parameter.
    /// </summary>
    /// <param name="password">The password to hash.</param>
    /// <param name="salt">The generated salt used in hashing.</param>
    /// <returns>A byte array containing the hashed password.</returns>
    public static byte[] HashPassword(this string password, out byte[] salt)
    {
        salt = GenerateSalt();

        using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));
        argon2.Salt = salt;
        argon2.Iterations = Argon2Iterations;
        argon2.MemorySize = Argon2MemorySize;
        argon2.DegreeOfParallelism = Argon2Parallelism;

        return argon2.GetBytes(32);
    }

    /// <summary>
    /// Verifies that a given password matches the specified hashed password using the provided salt.
    /// </summary>
    /// <param name="password">The password to verify.</param>
    /// <param name="salt">The salt used during the original hashing.</param>
    /// <param name="hashedPassword">The original hashed password to compare against.</param>
    /// <returns>True if the computed hash matches the provided hash; otherwise, false.</returns>
    public static bool Matches(this string password, byte[] salt, byte[] hashedPassword)
    {
        using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));
        argon2.Salt = salt;
        argon2.Iterations = Argon2Iterations;
        argon2.MemorySize = Argon2MemorySize;
        argon2.DegreeOfParallelism = Argon2Parallelism;

        byte[] computedHash = argon2.GetBytes(hashedPassword.Length);
        return CryptographicOperations.FixedTimeEquals(computedHash, hashedPassword);
    }

    /// <summary>
    /// Derives an encryption key and initialization vector (IV) from the specified password and salt.
    /// This method uses PBKDF2 with SHA-256.
    /// </summary>
    /// <param name="password">The password to derive from.</param>
    /// <param name="salt">The salt to use in the derivation.</param>
    /// <param name="key">When the method returns, contains the derived AES key.</param>
    /// <param name="iv">When the method returns, contains the derived initialization vector.</param>
    private static void DeriveKeyAndIv(string password, byte[] salt, out byte[] key, out byte[] iv)
    {
        using var deriveBytes = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
        key = deriveBytes.GetBytes(32); // AES-256 key
        iv = deriveBytes.GetBytes(16);  // AES IV
    }

    /// <summary>
    /// Generates a cryptographically secure random salt.
    /// </summary>
    /// <returns>A byte array containing the salt.</returns>
    private static byte[] GenerateSalt()
    {
        var salt = new byte[16];

        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);

        return salt;
    }

    /// <summary>
    /// Generates a random int between 0 (inclusive) and a specified maximum (exclusive).
    /// </summary>
    /// <param name="rng">An instance of RandomNumberGenerator.</param>
    /// <param name="maxExclusive">The exclusive upper bound for the random number.</param>
    /// <returns>A random int between 0 and maxExclusive.</returns>
    private static int GetRandomInt(RandomNumberGenerator rng, int maxExclusive)
    {
        var uint32Buffer = new byte[4];

        while (true)
        {
            rng.GetBytes(uint32Buffer);

            uint random = BitConverter.ToUInt32(uint32Buffer, 0);
            long max = (1L + uint.MaxValue);
            long remainder = max % maxExclusive;

            if (random < max - remainder)
                return (int)(random % maxExclusive);
        }
    }

    /// <summary>
    /// Shuffles the elements of the specified character array
    /// </summary>
    /// <param name="rng">An instance of RandomNumberGenerator.</param>
    /// <param name="array">The character array to shuffle.</param>
    private static void ShuffleArray(RandomNumberGenerator rng, ref char[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = GetRandomInt(rng, i + 1);
            (array[i], array[j]) = (array[j], array[i]);
        }
    }
}