using backend.DTOs.Profile;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AddressController : ControllerBase
{
    private readonly IAddressService _addressService;

    public AddressController(IAddressService addressService)
    {
        _addressService = addressService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAddress()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var address = await _addressService.GetAddressAsync(userId);
        if (address == null)
            return NotFound();

        return Ok(address);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAddress(AddressDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await _addressService.UpdateAddressAsync(userId, dto);

        return Ok(new
        {
            message = "Address updated successfully."
        });
    }
}
