namespace LealPasswordV2.Core.Database.Models;

public class SecretNote
{
    public string NoteId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string EncryptedTitle { get; set; } = string.Empty;
    public string EncryptedNote { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}