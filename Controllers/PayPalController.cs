using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using PayPalHttp;




[Route("api/[controller]")]
[ApiController]
public class PayPalController : ControllerBase
{
    private readonly PayPalEnvironment _environment;
    private readonly PayPalHttpClient _client;

    public PayPalController(IConfiguration config)
    {
        // Usa sandbox o production según tu entorno
        _environment = new SandboxEnvironment(
            config["PayPal:ClientId"],
            config["PayPal:Secret"]
        );
        _client = new PayPalHttpClient(_environment);
    }

    // Modelo para recibir el importe desde el front
    public class CreateOrderRequestModel
    {
        public decimal Value { get; set; }
    }

    [HttpPost("CreateOrder")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequestModel model)
    {
        try
        {
            var request = new OrdersCreateRequest();
            request.Prefer("return=representation");
            request.RequestBody(new OrderRequest
            {
                CheckoutPaymentIntent = "CAPTURE",
                PurchaseUnits = new List<PurchaseUnitRequest> {
                new PurchaseUnitRequest {
                    AmountWithBreakdown = new AmountWithBreakdown {
                        CurrencyCode = "USD",
                        Value = model.Value.ToString("F2")
                    }
                }
            }
            });

            var response = await _client.Execute(request);
            var order = response.Result<Order>();
            return Ok(new { orderID = order.Id });
        }
        catch (Exception ex)
        {
            // Devuelve siempre JSON, nunca texto plano
            return StatusCode(500, new
            {
                error = ex.Message,
                details = ex.StackTrace
            });
        }
    }


    [HttpPost("CaptureOrder/{orderId}")]
    public async Task<IActionResult> CaptureOrder(string orderId)
    {
        // Captura el pago
        var request = new OrdersCaptureRequest(orderId);
        request.RequestBody(new OrderActionRequest());
        var response = await _client.Execute(request);
        var result = response.Result<Order>();
        return Ok(result);
    }
}

