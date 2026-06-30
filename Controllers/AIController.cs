using backend.DTOs.AI;
using backend.Services;
using Google.GenAI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AIController:ControllerBase
{
    private readonly IAIService _aiService;

    public AIController(IAIService aiService)
    {
        _aiService = aiService;
    }

    [HttpPost("query")]
    public async Task<IActionResult> Query(AIQueryDto dto)
    {
        try
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var result = await _aiService.ProcessQueryAsync(userId, dto.Message);

            return Ok(result);
        }
        catch (ClientError ex)
        {
            return StatusCode(StatusCodes.Status429TooManyRequests, new
            {
                message = "The AI assistant has reached its usage limit. Please try again later.",
                details = ex.Message
            });
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                message = "An unexpected error occurred while processing your request."
            });
        }
    }
}
