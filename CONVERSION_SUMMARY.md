# Tarot Reader - Python to C# Dalamud Plugin Conversion

## Summary

Your Python tarot reader application has been successfully converted into a C# Dalamud plugin for Final Fantasy XIV! The plugin maintains all the core functionality while integrating seamlessly into the FFXIV game client.

## What Was Created

### New Files

1. **Models/TarotCard.cs** - Data model for tarot cards matching the JSON structure
2. **Services/TarotService.cs** - Manages card deck, drawing logic, and state
3. **Services/OllamaService.cs** - Handles AI interpretation generation via Ollama API
4. **Windows/TarotReaderWindow.cs** - Main UI window with ImGui interface

### Modified Files

1. **Plugin.cs** - Updated to initialize tarot services and register `/tarot` command
2. **Configuration.cs** - Added tarot-specific settings (character name, mode, job, model)
3. **Windows/ConfigWindow.cs** - Simplified to show Ollama API configuration
4. **SamplePlugin.json** - Updated plugin metadata
5. **SamplePlugin.csproj** - Added cards.json as embedded resource
6. **Data/cards.json** - Copied from root directory

## Key Features Converted

✅ Single card drawing with upright/reversed orientation
✅ Three-card spread (Past/Present/Future)
✅ AI interpretation generation using Ollama
✅ Multiple interpretation modes (Eorzean, Chaos, Maternal)
✅ Job flavor customization (all FFXIV jobs supported)
✅ Clipboard copy functionality for readings
✅ Regenerate interpretations without redrawing cards
✅ Configuration persistence across sessions

## How to Build

```bash
cd SamplePlugin
dotnet build
```

This will create the plugin DLL in `SamplePlugin/bin/Debug/net8.0-windows/`

## How to Install

1. Build the plugin (see above)
2. Copy the entire output folder contents to:
   ```
   %APPDATA%\XIVLauncher\devPlugins\MoonshadowTarotReader\
   ```
3. In FFXIV, open Dalamud Settings → Experimental tab
4. Enable "Dev Plugin Locations" if not already enabled
5. Restart the game or use `/xlplugins` to reload

## How to Use

1. **Start Ollama** before launching FFXIV:
   ```bash
   ollama pull mistral  # or llama2, gemma
   ollama serve
   ```

2. **In-game commands**:
   - `/tarot` - Open the tarot reader window
   - `/xlplugins` - Manage plugins

3. **Using the plugin**:
   - Enter your character name (saved automatically)
   - Select interpretation mode, job flavor, and Ollama model
   - Click "Draw Single Card" or "Draw 3-Card Spread"
   - Wait for AI interpretations to generate
   - Copy readings to clipboard and paste in /p chat

## Architecture Changes

### From Python to C#

| Python | C# Equivalent |
|--------|---------------|
| tkinter GUI | ImGui (Dalamud windowing) |
| requests.post() | HttpClient async/await |
| json.load() | JsonSerializer.Deserialize |
| Global variables | TarotService state management |
| Threading | async/await Tasks |
| clipboard_append() | ImGui.SetClipboardText() |

### Key Differences

1. **Async/Await**: AI generation is now async, preventing UI freezing
2. **Service Architecture**: Logic separated into services (TarotService, OllamaService)
3. **Configuration**: Settings persist via Dalamud's configuration system
4. **No Subprocess**: Ollama must be running separately (no auto-launch)
5. **ImGui UI**: Three-panel layout instead of tkinter columns

## Configuration Files

Settings are stored in:
```
%APPDATA%\XIVLauncher\pluginConfigs\SamplePlugin.json
```

Contains:
- Character name
- Interpretation mode
- Job flavor
- Ollama model selection
- Ollama API URL

## Troubleshooting

### "Cannot connect to Ollama"
- Ensure Ollama is running: `ollama serve`
- Check API URL in settings (default: http://localhost:11434/api/generate)

### Plugin won't load
- Check Dalamud logs: `/xllog`
- Verify cards.json is in the plugin folder
- Ensure .NET 8.0 SDK is installed

### Interpretations not generating
- Verify selected model is pulled: `ollama list`
- Check Dalamud logs for specific error messages
- Try switching to a different model

## Future Enhancements (Optional)

- Add custom card images for visual appeal
- Implement streaming interpretations (word-by-word display)
- Add more spread types (Celtic Cross, etc.)
- Theme customization with ImGui styling
- Auto-detect player's current job

## File Structure

```
SamplePlugin/
├── Data/
│   ├── cards.json          # Tarot card data
│   └── goat.png           # (unused, can remove)
├── SamplePlugin/
│   ├── Models/
│   │   └── TarotCard.cs
│   ├── Services/
│   │   ├── TarotService.cs
│   │   └── OllamaService.cs
│   ├── Windows/
│   │   ├── ConfigWindow.cs
│   │   └── TarotReaderWindow.cs
│   ├── Configuration.cs
│   ├── Plugin.cs
│   └── SamplePlugin.json
└── SamplePlugin.sln
```

## Notes

- The original Python files (reader.py, cards.json) are preserved in the root directory
- MainWindow.cs is no longer used (replaced by TarotReaderWindow.cs)
- You can customize the UI colors in TarotReaderWindow.cs using ImGui color functions
- The plugin uses non-streaming Ollama API for simplicity (can be enhanced for streaming)

Enjoy your new Dalamud plugin! 🔮✨
