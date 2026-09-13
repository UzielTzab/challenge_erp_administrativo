using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChallengeErp.Api.Entities;

public class Receipt
{
    [Key]
    public int Id { get; init; }

    [Required]
    public int LineId { get; init; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal QuantityReceived { get; set; }
    
    public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;

    public Line? Line { get; set; }
}