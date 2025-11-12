using System;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Windowing;
using MoonshadowTarotReader.Services;

namespace MoonshadowTarotReader.Windows;

public class TarotReaderWindow : Window, IDisposable
{
    private readonly Plugin plugin;
    private readonly TarotService tarotService;
    private readonly OllamaService ollamaService;

    private string characterName = "Your Warrior of Light";
    private int selectedModeIndex = 0;
    private int selectedJobIndex = 0;
    private int selectedModelIndex = 0;

    private readonly string[] interpretationModes = { "Eorzean", "Chaos", "Maternal" };
    private readonly string[] jobFlavors = {
        "None", "Paladin", "Warrior", "Dark Knight", "Gunbreaker",
        "White Mage", "Scholar", "Astrologian", "Sage",
        "Monk", "Dragoon", "Ninja", "Samurai", "Reaper",
        "Bard", "Machinist", "Dancer",
        "Black Mage", "Summoner", "Red Mage", "Blue Mage",
        "Viper", "Pictomancer"
    };
    private readonly string[] ollamaModels = { "mistral", "llama2", "gemma" };

    private string outputText = "";
    private bool isGenerating = false;
    private string statusMessage = "Ready";

    public TarotReaderWindow(Plugin plugin, TarotService tarotService, OllamaService ollamaService)
        : base("Moonshadow's Tarot Reader###TarotReader")
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(800, 600),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.plugin = plugin;
        this.tarotService = tarotService;
        this.ollamaService = ollamaService;

        // Load settings from configuration
        LoadSettings();
    }

    private void LoadSettings()
    {
        characterName = plugin.Configuration.CharacterName;
        selectedModeIndex = Array.IndexOf(interpretationModes, plugin.Configuration.InterpretationMode);
        if (selectedModeIndex < 0) selectedModeIndex = 0;

        selectedJobIndex = Array.IndexOf(jobFlavors, plugin.Configuration.JobFlavor);
        if (selectedJobIndex < 0) selectedJobIndex = 0;

        selectedModelIndex = Array.IndexOf(ollamaModels, plugin.Configuration.OllamaModel);
        if (selectedModelIndex < 0) selectedModelIndex = 0;
    }

    private void SaveSettings()
    {
        plugin.Configuration.CharacterName = characterName;
        plugin.Configuration.InterpretationMode = interpretationModes[selectedModeIndex];
        plugin.Configuration.JobFlavor = jobFlavors[selectedJobIndex];
        plugin.Configuration.OllamaModel = ollamaModels[selectedModelIndex];
        plugin.Configuration.Save();
    }

    public void Dispose() { }

    public override void Draw()
    {
        // Left Column - Settings
        ImGui.BeginChild("LeftPanel", new Vector2(250, -1), true);
        {
            ImGui.Text("Character Name:");
            if (ImGui.InputText("##charname", ref characterName, 100))
            {
                SaveSettings();
            }

            ImGui.Spacing();
            ImGui.Text("Interpretation Mode:");
            if (ImGui.Combo("##mode", ref selectedModeIndex, interpretationModes, interpretationModes.Length))
            {
                SaveSettings();
            }

            ImGui.Spacing();
            ImGui.Text("Job Flavor:");
            if (ImGui.Combo("##job", ref selectedJobIndex, jobFlavors, jobFlavors.Length))
            {
                SaveSettings();
            }

            ImGui.Spacing();
            ImGui.Text("Ollama Model:");
            if (ImGui.Combo("##model", ref selectedModelIndex, ollamaModels, ollamaModels.Length))
            {
                SaveSettings();
            }

            ImGui.Spacing();
            ImGui.Separator();
            ImGui.Spacing();

            if (ImGui.Button("Show Settings", new Vector2(-1, 0)))
            {
                plugin.ToggleConfigUi();
            }
        }
        ImGui.EndChild();

        ImGui.SameLine();

        // Middle Column - Actions
        ImGui.BeginChild("MiddlePanel", new Vector2(200, -1), true);
        {
            ImGui.Text("Draw Options:");
            ImGui.Spacing();

            if (ImGui.Button("Draw Single Card", new Vector2(-1, 0)))
            {
                DrawSingleCard();
            }

            if (ImGui.Button("Draw 3-Card Spread", new Vector2(-1, 0)))
            {
                DrawThreeCardSpread();
            }

            if (ImGui.Button("Regenerate", new Vector2(-1, 0)))
            {
                RegenerateInterpretations();
            }

            if (ImGui.Button("Clear All", new Vector2(-1, 0)))
            {
                ClearAll();
            }

            ImGui.Spacing();
            ImGui.Separator();
            ImGui.Spacing();

            ImGui.Text("Copy to Clipboard:");
            ImGui.Spacing();

            if (ImGui.Button("Copy All", new Vector2(-1, 0)))
            {
                CopyToClipboard(outputText);
            }

            if (tarotService.LastDrawnCards.Count > 0)
            {
                ImGui.Spacing();
                ImGui.Text("Individual Cards:");

                // For single card draw
                if (tarotService.LastDrawnCards.Count == 1)
                {
                    if (ImGui.Button("Copy Card", new Vector2(-1, 0)))
                    {
                        var cardText = GetIndividualCardText(0);
                        CopyToClipboard(cardText);
                    }
                }
                // For three-card spread
                else if (tarotService.LastDrawnCards.Count == 3)
                {
                    if (ImGui.Button("Copy Past", new Vector2(-1, 0)))
                    {
                        var pastText = GetIndividualCardText(0);
                        CopyToClipboard(pastText);
                    }

                    if (ImGui.Button("Copy Present", new Vector2(-1, 0)))
                    {
                        var presentText = GetIndividualCardText(1);
                        CopyToClipboard(presentText);
                    }

                    if (ImGui.Button("Copy Future", new Vector2(-1, 0)))
                    {
                        var futureText = GetIndividualCardText(2);
                        CopyToClipboard(futureText);
                    }
                }
            }
        }
        ImGui.EndChild();

        ImGui.SameLine();

        // Right Column - Output
        ImGui.BeginChild("RightPanel", new Vector2(-1, -1), true);
        {
            ImGui.Text("Reading Output:");
            ImGui.Separator();

            if (isGenerating)
            {
                ImGui.TextColored(new Vector4(1, 1, 0, 1), "Generating interpretations...");
            }
            else
            {
                ImGui.TextColored(new Vector4(0, 1, 0, 1), statusMessage);
            }

            ImGui.Spacing();

            // Selectable text output
            ImGui.InputTextMultiline("##output", ref outputText, 50000, new Vector2(-1, -1),
                ImGuiInputTextFlags.ReadOnly);
        }
        ImGui.EndChild();
    }

    private async void DrawSingleCard()
    {
        if (isGenerating)
        {
            statusMessage = "Already generating, please wait...";
            return;
        }

        try
        {
            isGenerating = true;
            statusMessage = "Drawing card...";

            var drawnCard = tarotService.DrawSingleCard();
            var mode = interpretationModes[selectedModeIndex];

            outputText = "Generating interpretation...";

            statusMessage = "Generating interpretation...";

            // Generate AI interpretation
            var jobFlavor = jobFlavors[selectedJobIndex];
            var interpretation = await ollamaService.GenerateInterpretationAsync(
                drawnCard.Card.Name,
                mode,
                "General",
                characterName,
                jobFlavor != "None" ? jobFlavor : null
            );

            drawnCard.AiInterpretation = interpretation;
            outputText = drawnCard.GetFormattedReadingWithInterpretation(characterName, "General", mode);
            statusMessage = "Ready";
        }
        catch (Exception ex)
        {
            statusMessage = $"Error: {ex.Message}";
            Plugin.Log.Error($"Error drawing single card: {ex}");
        }
        finally
        {
            isGenerating = false;
        }
    }

    private async void DrawThreeCardSpread()
    {
        if (isGenerating)
        {
            statusMessage = "Already generating, please wait...";
            return;
        }

        try
        {
            isGenerating = true;
            statusMessage = "Drawing cards...";

            var drawnCards = tarotService.DrawThreeCardSpread();
            var mode = interpretationModes[selectedModeIndex];

            var sb = new StringBuilder();
            sb.AppendLine($"/p ✦ Reading for {characterName} ✦");
            sb.AppendLine("Generating interpretations...");

            outputText = sb.ToString();
            statusMessage = "Generating interpretations...";

            // Generate AI interpretations
            var jobFlavor = jobFlavors[selectedJobIndex];
            for (int i = 0; i < drawnCards.Count; i++)
            {
                var card = drawnCards[i];
                var label = tarotService.LastDrawnLabels[i];

                var interpretation = await ollamaService.GenerateInterpretationAsync(
                    card.Card.Name,
                    mode,
                    label,
                    characterName,
                    jobFlavor != "None" ? jobFlavor : null
                );

                card.AiInterpretation = interpretation;
            }

            // Build final output - each card with interpretation on same line
            sb.Clear();
            sb.AppendLine($"/p ✦ Reading for {characterName} ✦");

            for (int i = 0; i < drawnCards.Count; i++)
            {
                var label = tarotService.LastDrawnLabels[i];
                sb.AppendLine(drawnCards[i].GetFormattedReadingWithInterpretation(characterName, label, mode));
            }

            outputText = sb.ToString();
            statusMessage = "Ready";
        }
        catch (Exception ex)
        {
            statusMessage = $"Error: {ex.Message}";
            Plugin.Log.Error($"Error drawing three card spread: {ex}");
        }
        finally
        {
            isGenerating = false;
        }
    }

    private async void RegenerateInterpretations()
    {
        if (isGenerating)
        {
            statusMessage = "Already generating, please wait...";
            return;
        }

        if (tarotService.LastDrawnCards.Count == 0)
        {
            statusMessage = "No previous draw to regenerate";
            return;
        }

        try
        {
            isGenerating = true;
            statusMessage = "Regenerating interpretations...";

            var mode = interpretationModes[selectedModeIndex];
            var jobFlavor = jobFlavors[selectedJobIndex];

            for (int i = 0; i < tarotService.LastDrawnCards.Count; i++)
            {
                var card = tarotService.LastDrawnCards[i];
                var label = tarotService.LastDrawnLabels[i];

                var interpretation = await ollamaService.GenerateInterpretationAsync(
                    card.Card.Name,
                    mode,
                    label,
                    characterName,
                    jobFlavor != "None" ? jobFlavor : null
                );

                card.AiInterpretation = interpretation;
            }

            // Rebuild output
            var sb = new StringBuilder();
            if (tarotService.LastDrawnCards.Count > 1)
            {
                sb.AppendLine($"/p ✦ Reading for {characterName} ✦");
            }

            for (int i = 0; i < tarotService.LastDrawnCards.Count; i++)
            {
                var label = tarotService.LastDrawnLabels[i];
                sb.AppendLine(tarotService.LastDrawnCards[i].GetFormattedReadingWithInterpretation(characterName, label, mode));
            }

            outputText = sb.ToString();
            statusMessage = "Ready";
        }
        catch (Exception ex)
        {
            statusMessage = $"Error: {ex.Message}";
            Plugin.Log.Error($"Error regenerating interpretations: {ex}");
        }
        finally
        {
            isGenerating = false;
        }
    }

    private void ClearAll()
    {
        outputText = "";
        statusMessage = "Cleared";
    }

    private string GetReadingOnlyText()
    {
        var sb = new StringBuilder();
        if (tarotService.LastDrawnCards.Count > 1)
        {
            sb.AppendLine($"/p ✦ Reading for {characterName} ✦");
        }

        for (int i = 0; i < tarotService.LastDrawnCards.Count; i++)
        {
            var label = tarotService.LastDrawnLabels[i];
            sb.AppendLine(tarotService.LastDrawnCards[i].GetFormattedReading(characterName, label));
        }

        return sb.ToString();
    }

    private string GetInterpretationOnlyText()
    {
        var sb = new StringBuilder();
        var mode = interpretationModes[selectedModeIndex];

        for (int i = 0; i < tarotService.LastDrawnCards.Count; i++)
        {
            var label = tarotService.LastDrawnLabels[i];
            sb.AppendLine(tarotService.LastDrawnCards[i].GetFormattedInterpretation(label, mode));
        }

        return sb.ToString();
    }

    private string GetIndividualCardText(int cardIndex)
    {
        if (cardIndex < 0 || cardIndex >= tarotService.LastDrawnCards.Count)
        {
            return "";
        }

        var card = tarotService.LastDrawnCards[cardIndex];
        var label = tarotService.LastDrawnLabels[cardIndex];
        var mode = interpretationModes[selectedModeIndex];

        return card.GetFormattedReadingWithInterpretation(characterName, label, mode);
    }

    private void CopyToClipboard(string text)
    {
        try
        {
            ImGui.SetClipboardText(text);
            statusMessage = "Copied to clipboard!";
        }
        catch (Exception ex)
        {
            statusMessage = $"Failed to copy: {ex.Message}";
            Plugin.Log.Error($"Failed to copy to clipboard: {ex}");
        }
    }
}
