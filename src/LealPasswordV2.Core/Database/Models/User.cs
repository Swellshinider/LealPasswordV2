namespace LealPasswordV2.Core.Database.Models;

public class User
{
    public string UserId { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public byte[] MasterPasswordHash { get; set; } = [];
    public byte[] Salt { get; set; } = [];
    public DateTime CreatedAt { get; set; }
}