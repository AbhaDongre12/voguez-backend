using backend.DTOs.Profile;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfileController:ControllerBase
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateProfile(UpdateProfileDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var updated = await _profileService.UpdateProfileAsync(userId, dto);
        if (!updated)
            return NotFound();

        return Ok(new
        {
            message = "Profile updated successfully."
        });
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var (success, message) = await _profileService.ChangePasswordAsync(userId, dto);

        if (message == null && !success)
            return NotFound();

        if (!success)
        {
            return BadRequest(new
            {
                message
            });
        }

        return Ok(new
        {
            message = "Password changed successfully."
        });
    }
}
