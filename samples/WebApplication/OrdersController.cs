using Microsoft.AspNetCore.Mvc;
using Vogen;

[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private static readonly Order[] _orders =
    [
        new() { OrderId = 1, CustomerName = "Fred", DeliveryScheme = DeliveryScheme.PaidNextDay },
        new() { OrderId = 2, CustomerName = "Wilma", DeliveryScheme = DeliveryScheme.PaidNextDay },
        new() { OrderId = 3, CustomerName = "Barney", DeliveryScheme = DeliveryScheme.FreeFiveWorkingDays }
    ];

    [HttpGet]
    public IActionResult CurrentOrders()
    {
        return Ok(_orders);
    }

    [HttpGet, Route("customer/{customerName}")]
    public IActionResult GetByName(string customerName)
    {
        return Ok(_orders.Where(o => o.CustomerName == customerName));
    }

    [HttpGet, Route("orderid/{orderId}")]
    public IActionResult GetByOrderId(int orderId)
    {
        return Ok(_orders.Where(o => o.OrderId == orderId));
    }

    [HttpGet, Route("delivery/{deliveryScheme}")]
    public IActionResult GetByDeliveryScheme(DeliveryScheme deliveryScheme)
    {
        return Ok(_orders.Where(o => o.DeliveryScheme == deliveryScheme));
    }

    [HttpGet, Route("delivery2/{deliveryScheme}")]
    public IActionResult GetByDeliveryScheme(DeliverySchemeEnum deliveryScheme)
    {
        return Ok(_orders.Where(o => o.DeliveryScheme.ToString() == deliveryScheme.ToString()));
    }
}