using ChallengeErp.Api.Data;
using ChallengeErp.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChallengeErp.Api.Entities;

namespace ChallengeErp.Api.Controllers; 

[ApiController]
[Route("ordenes")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _context;

    public OrdersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResponseDto>> GetOrder(string id)
    {
        // 1. Consultamos la orden por su ID, incluyendo las líneas y sus recepciones
        var order = await _context.Orders
            .Include(order => order.Lines)
            .ThenInclude(line => line.Receipts)
            .FirstOrDefaultAsync(order => order.Id == id);

        // 2. Manejar el caso de que la orden no exista
        if (order == null)
        {
            return NotFound(new { message = $"No se encontró la orden con el ID: {id}" });
        }

        // 3. Construimos la respuesta utilizando nuestro DTO de la orden. Realizamos los cálculos de negocio
        var response = new OrderResponseDto
        {
            Id = order.Id,
            Provider = order.Provider,
            Status = order.Status.ToString(),
            Lines = order.Lines.Select(line => 
            {
        
                decimal receivedQuantity = line.Receipts.Sum(receipt => receipt.QuantityReceived);
                decimal maximumAcceptable = Math.Round(line.Quantity * 1.02m, 2);
                decimal pendingQuantity = Math.Max(0, line.Quantity - receivedQuantity);

                return new OrderLineResponseDto
                {
                    Id = line.Id,
                    Article = line.Article,
                    UnitOfMeasure = line.UnitOfMeasure,
                    OrderedQuantity = line.Quantity,
                    ReceivedQuantity = receivedQuantity,
                    PendingQuantity = pendingQuantity,
                    MaximumAcceptable = maximumAcceptable
                };
            }).ToList()
        };

        return Ok(response);
    }

    [HttpPost("{id}/recepciones")]
    public async Task<ActionResult<OrderResponseDto>> RegisterReception(string id, [FromBody] ReceptionRequestDto request)
    {

        var order = await _context.Orders
            .Include(order => order.Lines)
            .ThenInclude(line => line.Receipts)
            .FirstOrDefaultAsync(order => order.Id == id);

        // Manejar el caso de que la orden no exista
        if (order == null)
            return NotFound(new { message = $"No se encontró la orden {id}" });

        // Manejar el caso de una orden cerrada
        if (order.Status == OrderStatus.Close)
            return Conflict(new { message = "La orden ya está cerrada y no acepta más recepciones." });

        // Manejar las cantidades de la línea válidas y pertenencia
        foreach (var reqLine in request.Lines)
        {
            if (reqLine.Quantity <= 0)
                return BadRequest(new { message = "La cantidad a recibir debe ser mayor que cero.", lineId = reqLine.OrderLineId });

            if (!order.Lines.Any(line => line.Id == reqLine.OrderLineId))
                return BadRequest(new { message = "La línea no pertenece a esta orden de compra.", lineId = reqLine.OrderLineId });
        }

        // Manejar la Tolerancia del 2%
        foreach (var reqLine in request.Lines)
        {
            var line = order.Lines.First(line => line.Id == reqLine.OrderLineId);

            decimal receivedQuantity = line.Receipts.Sum(receipt => receipt.QuantityReceived);
            
            decimal maximumAcceptable = Math.Round(line.Quantity * 1.02m, 2, MidpointRounding.AwayFromZero);
            
            if (receivedQuantity + reqLine.Quantity > maximumAcceptable)
            {
                return StatusCode(422, new 
                { 
                    message = "La recepción completa fue rechazada porque excede la tolerancia del 2%. No se aplicó ninguna línea.",
                    lineId = line.Id,
                    pendingQuantity = Math.Max(0, line.Quantity - receivedQuantity),
                    maximumAcceptable = maximumAcceptable
                });
            }
        }

        // APLICAR RECEPCIONES (Si superó todas las validaciones)
        foreach (var reqLine in request.Lines)
        {
            var line = order.Lines.First(line => line.Id == reqLine.OrderLineId);
            var receipt = new Receipt 
            { 
                OrderLineId = reqLine.OrderLineId, 
                QuantityReceived = reqLine.Quantity 
            };
            
            line.Receipts.Add(receipt);
        }

        // Cierre automático
        bool allLinesCompleted = order.Lines.All(line => line.Receipts.Sum(receipt => receipt.QuantityReceived) >= line.Quantity);
        if (allLinesCompleted)
        {
            order.Status = OrderStatus.Close;
        }

        await _context.SaveChangesAsync();

        var response = new OrderResponseDto
        {
            Id = order.Id,
            Provider = order.Provider,
            Status = order.Status.ToString(),
            Lines = order.Lines.Select(line => 
            {
                decimal receivedQuantity = line.Receipts.Sum(receipt => receipt.QuantityReceived);
                return new OrderLineResponseDto
                {
                    Id = line.Id,
                    Article = line.Article,
                    UnitOfMeasure = line.UnitOfMeasure,
                    OrderedQuantity = line.Quantity,
                    ReceivedQuantity = receivedQuantity,
                    PendingQuantity = Math.Max(0, line.Quantity - receivedQuantity),
                    MaximumAcceptable = Math.Round(line.Quantity * 1.02m, 2, MidpointRounding.AwayFromZero)
                };
            }).ToList()
        };

        return Ok(response);
    }
}