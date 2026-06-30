using backend.DTOs.Cart;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController:ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        var userId = int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var cart = await _cartService.GetCartItemsAsync(userId);
        return Ok(cart);
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddToCart(AddToCartDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var (success, error) = await _cartService.AddToCartAsync(userId, dto);
        if (!success)
        {
            if (error == "Product Not Found")
                return NotFound(error);
            return BadRequest(error);
        }

        return Ok(new
        {
            Message = "Added to Cart"
        });
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateCart(UpdateCartDto dto)
    {
        var item = await _cartService.UpdateCartItemAsync(dto);
        if (item == null)
            return NotFound();

        return Ok(item);
    }

    [HttpDelete("remove/{id}")]
    public async Task<IActionResult> RemoveFromCart(int id)
    {
        var removed = await _cartService.RemoveFromCartAsync(id);
        if (!removed)
            return NotFound();

        return Ok(new
        {
            Message = "Removed"
        });
    }
}
