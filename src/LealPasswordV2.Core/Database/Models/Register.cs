namespace LealPasswordV2.Core.Database.Models;

public class Register
{
    public string RegisterId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string EncryptedName { get; set; } = string.Empty;
    public string EncryptedUsername { get; set; } = string.Empty;
    public string EncryptedPassword { get; set; } = string.Empty;
    public string? EncryptedTag { get; set; }
    public string? EncryptedDescription { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}