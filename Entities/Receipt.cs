namespace ChallengeErp.Api.Entities;

public class Receipt
{
    public int Id { get; set; }
    public int OrderLineId { get; set; }
    public decimal QuantityReceived { get; set; }
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;

    public OrderLine? OrderLine { get; set; }
}