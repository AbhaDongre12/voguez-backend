using backend.DTOs.Order;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController:ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return Ok(orders);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, UpdateOrderStatusDto dto)
    {
        var order = await _orderService.UpdateStatusAsync(id, dto);
        if (order == null)
            return NotFound();

        return Ok(order);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
    {
        var userId = int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var (success, error, result) = await _orderService.CreateOrderAsync(userId);
        if (!success)
            return BadRequest(error);

        return Ok(result);
    }

    [Authorize]
    [HttpGet("my-orders")]
    public async Task<IActionResult> MyOrders()
    {
        var userId = int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var orders = await _orderService.GetMyOrdersAsync(userId);
        return Ok(orders);
    }

    [Authorize]
    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> CancelOrder(int id)
    {
        var userId = int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var (success, error) = await _orderService.CancelOrderAsync(userId, id);
        if (error == null && !success)
            return NotFound();

        if (!success)
            return BadRequest(error);

        return Ok(new
        {
            Message = "Order Cancelled"
        });
    }
}
