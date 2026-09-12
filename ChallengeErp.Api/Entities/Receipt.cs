using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChallengeErp.Api.Entities;

public class Receipt
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int OrderLineId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal QuantityReceived { get; set; }
    
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;

    public OrderLine? OrderLine { get; set; }
}