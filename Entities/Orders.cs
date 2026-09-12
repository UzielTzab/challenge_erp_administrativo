namespace ChallengeErp.Api.Entities;

public class Order
{
    public string Id { get; set; } = string.Empty;
    public string Supplier { get; set; } = string.Empty;
    public OrderStatus Status { get; set; } = OrderStatus.Open;
    public List<OrderLine> Lines { get; set; } = new List<OrderLine>();
}

public enum OrderStatus
{
    Close, 
    Open
}