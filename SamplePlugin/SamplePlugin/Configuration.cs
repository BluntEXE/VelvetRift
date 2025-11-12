using Dalamud.Configuration;
using System;

namespace MoonshadowTarotReader;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 0;

    // Window settings
    public bool IsConfigWindowMovable { get; set; } = true;

    // Tarot Reader settings
    public string CharacterName { get; set; } = "Your Warrior of Light";
    public string InterpretationMode { get; set; } = "Eorzean";
    public string JobFlavor { get; set; } = "None";
    public string OllamaModel { get; set; } = "mistral";
    public string OllamaApiUrl { get; set; } = "http://localhost:11434/api/generate";

    // The below exist just to make saving less cumbersome
    public void Save()
    {
        Plugin.PluginInterface.SavePluginConfig(this);
    }
}
