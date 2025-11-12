using System;
using System.Text.Json.Serialization;

namespace MoonshadowTarotReader.Models;

[Serializable]
public class TarotCard
{
    [JsonPropertyName("Name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("Meaning")]
    public string Meaning { get; set; } = string.Empty;

    [JsonPropertyName("EorzeanInterpretation")]
    public string? EorzeanInterpretation { get; set; }

    [JsonPropertyName("ChaosMode")]
    public string? ChaosMode { get; set; }

    [JsonPropertyName("MaternalMode")]
    public string? MaternalMode { get; set; }
}
