namespace ChallengeErp.Api.DTOs;

public class ReceptionRequestDto
{
    public List<ReceptionLineDto> Lines { get; set; } = new();
}

public class ReceptionLineDto
{
    public int OrderLineId { get; set; }
    public decimal Quantity { get; set; }
}