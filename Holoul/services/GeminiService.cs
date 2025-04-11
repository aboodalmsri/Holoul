using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

public class GeminiService
{
    private readonly string _apiKey;
    private readonly HttpClient _httpClient;

    public GeminiService(IOptions<GeminiSettings> options)
    {
        _apiKey = options.Value.ApiKey ?? throw new ArgumentNullException("API key is missing in configuration.");
        _httpClient = new HttpClient();
    }

    public async Task<string> GenerateTextAsync(string prompt)
    {
        var requestData = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[] { new { text = prompt } }
                }
            }
        };

        var json = JsonSerializer.Serialize(requestData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var url = $"https://generativelanguage.googleapis.com/v1/models/gemini-2.0-flash:generateContent?key={_apiKey}";

        try
        {
            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();

            using JsonDocument doc = JsonDocument.Parse(responseBody);
            var parts = doc.RootElement
                          .GetProperty("candidates")[0]
                          .GetProperty("content")
                          .GetProperty("parts")[0]
                          .GetProperty("text")
                          .GetString();

            return parts ?? "No content";
        }
        catch (HttpRequestException ex)
        {
            return $"HTTP Error: {ex.Message}";
        }
        catch (JsonException ex)
        {
            return $"Parsing Error: {ex.Message}";
        }
    }


}
