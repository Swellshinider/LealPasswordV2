namespace LealPasswordV2.Core.Database.Models;

public class CreditCard
{
    public string CardId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string CardName { get; set; } = string.Empty;
    public string CardholderName { get; set; } = string.Empty;
    public string CardNumberEncrypted { get; set; } = string.Empty;
    public string ExpirationDateEncrypted { get; set; } = string.Empty;
    public string CVVEncrypted { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}