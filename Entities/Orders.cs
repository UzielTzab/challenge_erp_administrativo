using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChallengeErp.Api.Entities;

public enum OrderStatus
{
    Close,
    Open
}

public class Order
{
    [Key]
    [MaxLength(20)]
    public string Id { get; set; } = string.Empty;
    [Required]
    [MaxLength(100)]
    public string Provider { get; set; } = string.Empty;

    public OrderStatus Status { get; set; } = OrderStatus.Open;
    public List<OrderLine> Lines { get; set; } = new();
}