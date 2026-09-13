using ChallengeErp.Api.Controllers;
using ChallengeErp.Api.Data;
using ChallengeErp.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChallengeErp.Tests;

public class OrdersControllerTests
{
    private AppDbContext GetDatabaseContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        var context = new AppDbContext(options);
        context.Database.EnsureCreated(); 
        context.ChangeTracker.Clear(); 
        
        return context;
    }

    [Fact]
    public async Task RegisterReception_RecepcionParcialValida_ReturnsOkAndUpdatesPending()
    {
        var context = GetDatabaseContext("TestDb_Partial");
        var controller = new OrdersController(context);
        var request = new ReceptionRequestDto
        {
            Lines = new List<ReceptionLineDto>
            {
                // Pedimos 100 de vidrio (Id 1), recibiremos 50
                new ReceptionLineDto { LineId = 1, Quantity = 50 } 
            }
        };

        var result = await controller.RegisterReception("OC-1001", request);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<OrderResponseDto>(okResult.Value);
        var line1 = response.Lines.First(l => l.Id == 1);
        
        Assert.Equal(50, line1.ReceivedQuantity);
        Assert.Equal(50, line1.PendingQuantity);
        Assert.Equal("Open", response.Status);
    }

    [Fact]
    public async Task RegisterReception_ExcedeTolerancia_Returns422()
    {
        var context = GetDatabaseContext("TestDb_Tolerance");
        var controller = new OrdersController(context);
        var request = new ReceptionRequestDto
        {
            Lines = new List<ReceptionLineDto>
            {
                // Pedimos 40 de silicón (Id 2). Su máximo al 2% es 40.80. Intentamos recibir 45.
                new ReceptionLineDto { LineId = 2, Quantity = 45 } 
            }
        };

        var result = await controller.RegisterReception("OC-1001", request);

        var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(422, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task RegisterReception_CierreAutomatico_CompletaTodasLasLineas()
    {
        var context = GetDatabaseContext("TestDb_Close");
        var controller = new OrdersController(context);
        var request = new ReceptionRequestDto
        {
            Lines = new List<ReceptionLineDto>
            {
                // Completamos las tres líneas para verificar el cierre automático de la orden.
                new ReceptionLineDto { LineId = 1, Quantity = 100 },
                new ReceptionLineDto { LineId = 2, Quantity = 40 },
                new ReceptionLineDto { LineId = 3, Quantity = 25 }
            }
        };

        var result = await controller.RegisterReception("OC-1001", request);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<OrderResponseDto>(okResult.Value);
        
        Assert.Equal("Close", response.Status);
    }
}