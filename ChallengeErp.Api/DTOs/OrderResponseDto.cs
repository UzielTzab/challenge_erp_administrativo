namespace ChallengeErp.Api.DTOs;

public class OrderResponseDto
{
    public string Id { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public List<OrderLineResponseDto> Lines { get; set; } = new();
}

public class OrderLineResponseDto
{
    public int Id { get; set; }
    public string Article { get; set; } = string.Empty;
    public decimal OrderedQuantity { get; set; }
    public decimal ReceivedQuantity { get; set; }
    public decimal PendingQuantity { get; set; }
    public decimal MaximumAcceptable { get; set; }
}