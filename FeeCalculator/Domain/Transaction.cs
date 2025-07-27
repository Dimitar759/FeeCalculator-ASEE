using Domain;

public class Transaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Type { get; set; } = default!; // POS, e-commerce, etc.
    public decimal Amount { get; set; }
    public string Currency { get; set; } = default!;
    public bool IsInternational { get; set; }
    public Client Client { get; set; } = default!;
    public Guid ClientId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
