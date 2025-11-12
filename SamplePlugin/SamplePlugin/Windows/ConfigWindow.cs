using System;
using System.Numerics;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Windowing;

namespace MoonshadowTarotReader.Windows;

public class ConfigWindow : Window, IDisposable
{
    private readonly Configuration configuration;
    private string ollamaApiUrl;

    public ConfigWindow(Plugin plugin) : base("Tarot Reader Settings###TarotConfig")
    {
        Flags = ImGuiWindowFlags.NoCollapse;

        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(400, 150),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        configuration = plugin.Configuration;
        ollamaApiUrl = configuration.OllamaApiUrl;
    }

    public void Dispose() { }

    public override void PreDraw()
    {
        // Flags must be added or removed before Draw() is being called, or they won't apply
        if (configuration.IsConfigWindowMovable)
        {
            Flags &= ~ImGuiWindowFlags.NoMove;
        }
        else
        {
            Flags |= ImGuiWindowFlags.NoMove;
        }
    }

    public override void Draw()
    {
        ImGui.Text("Ollama API Configuration:");
        ImGui.Spacing();

        if (ImGui.InputText("Ollama API URL", ref ollamaApiUrl, 200))
        {
            configuration.OllamaApiUrl = ollamaApiUrl;
            configuration.Save();
        }

        ImGui.Spacing();
        ImGui.TextWrapped("Make sure Ollama is running with your selected model.");
        ImGui.TextWrapped("Use /tarot to open the tarot reader.");

        ImGui.Spacing();
        ImGui.Separator();

        var movable = configuration.IsConfigWindowMovable;
        if (ImGui.Checkbox("Movable Config Window", ref movable))
        {
            configuration.IsConfigWindowMovable = movable;
            configuration.Save();
        }
    }
}
