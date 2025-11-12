using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MoonshadowTarotReader.Services;

public class OllamaService
{
    private readonly HttpClient httpClient;
    private readonly Configuration configuration;

    public OllamaService(Configuration configuration)
    {
        this.configuration = configuration;
        this.httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromMinutes(2)
        };
    }

    public async Task<string> GenerateInterpretationAsync(
        string cardName,
        string mode,
        string label,
        string character,
        string? jobFlavor = null)
    {
        try
        {
            var jobFlavorText = jobFlavor != null && jobFlavor != "None"
                ? $" flavored for a {jobFlavor}"
                : "";

            var prompt = $"Give a poetic, immersive tarot interpretation for the card '{cardName}', " +
                        $"in {mode} mode, for a character named '{character}'{jobFlavorText}. " +
                        $"Label this as the {label} interpretation. Keep it under 50 words.";

            var requestBody = new OllamaRequest
            {
                Model = configuration.OllamaModel,
                Prompt = prompt,
                Stream = false
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            Plugin.Log.Debug($"Sending request to Ollama: {prompt}");
            var response = await httpClient.PostAsync(configuration.OllamaApiUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Plugin.Log.Error($"Ollama API error: {response.StatusCode} - {error}");
                return $"[Error: Unable to generate interpretation - {response.StatusCode}]";
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            var ollamaResponse = JsonSerializer.Deserialize<OllamaResponse>(responseJson);

            return ollamaResponse?.Response?.Trim() ?? "[Error: Empty response from Ollama]";
        }
        catch (TaskCanceledException)
        {
            Plugin.Log.Warning("Ollama request timed out");
            return "[Error: Request timed out. Is Ollama running?]";
        }
        catch (HttpRequestException ex)
        {
            Plugin.Log.Error($"Ollama connection error: {ex.Message}");
            return "[Error: Cannot connect to Ollama. Make sure it's running on localhost:11434]";
        }
        catch (Exception ex)
        {
            Plugin.Log.Error($"Ollama error: {ex.Message}");
            return $"[Error: {ex.Message}]";
        }
    }

    public void Dispose()
    {
        httpClient?.Dispose();
    }
}

internal class OllamaRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    [JsonPropertyName("prompt")]
    public string Prompt { get; set; } = string.Empty;

    [JsonPropertyName("stream")]
    public bool Stream { get; set; }
}

internal class OllamaResponse
{
    [JsonPropertyName("response")]
    public string? Response { get; set; }

    [JsonPropertyName("done")]
    public bool Done { get; set; }
}
