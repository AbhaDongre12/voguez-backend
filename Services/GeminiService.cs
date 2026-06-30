using Microsoft.Extensions.AI;
using Google.GenAI;

namespace backend.Services;

public class GeminiService
{
    private readonly string _apiKey;

    public GeminiService(IConfiguration configuration)
    {
        _apiKey = configuration["Gemini:ApiKey"];
    }

    public async Task<String> GetIntentAsync(string message)
    {
        var client = new Client(apiKey: _apiKey);
        string prompt = $"""
You are an intent classifier.

Return ONLY one of these values:

GET_LAST_ORDER
GET_CART
GET_PENDING_ORDERS
GET_ALL_ORDERS
UNKNOWN

User message:
{message}
""";
        var response = await client.Models.GenerateContentAsync(
            model: "gemini-3.1-flash-lite",
            contents: prompt
        );
        return response.Text?.Trim() ?? "UNKNOWN";
    }
}
