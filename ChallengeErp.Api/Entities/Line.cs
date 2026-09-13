using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChallengeErp.Api.Entities;

public enum OrderUnitOfMeasure
{
    M2,
    Pza
}

public class Line
{
    [Key]
    public int Id { get; init; }

    [Required]
    [MaxLength(20)]
    public string OrderId { get; init; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Article { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    public OrderUnitOfMeasure UnitOfMeasure { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public Order? Order { get; set; }
    public List<Receipt> Receipts { get; set; } = new();
}