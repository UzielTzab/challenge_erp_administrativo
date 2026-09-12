namespace ChallengeErp.Api.Entities;

public class OrderLine
{
    public int Id { get; set; }
    public string OrderId { get; set; } = string.Empty;
    public string Article { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }

    public Order? Order { get; set; }
    public List<Receipt> Receipts { get; set; } = new();
}