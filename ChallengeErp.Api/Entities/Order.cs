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
    public string Id { get; init; } = string.Empty;

    [Required]
    public string Supplier { get; set; } = string.Empty;

    public OrderStatus Status { get; set; } = OrderStatus.Open;
    public List<Line> Lines { get; set; } = new();
}