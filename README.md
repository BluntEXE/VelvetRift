# VelvetRift - Moonshadow's Tarot Reader

[![Latest Release](https://img.shields.io/github/v/release/BluntEXE/VelvetRift)](https://github.com/BluntEXE/VelvetRift/releases/latest)
[![Downloads](https://img.shields.io/github/downloads/BluntEXE/VelvetRift/total)](https://github.com/BluntEXE/VelvetRift/releases)

A Dalamud plugin for Final Fantasy XIV that provides FFXIV-themed tarot card readings with AI-generated interpretations.

**Custom Repository URL:**
```
https://raw.githubusercontent.com/BluntEXE/VelvetRift/master/repo.json
```

## Features

- 🔮 **Single Card Draws** - Quick one-card readings
- 🎴 **Three-Card Spreads** - Past, Present, Future readings
- 🤖 **AI Interpretations** - Powered by Ollama (local LLM)
- 🎭 **Multiple Modes** - Eorzean, Chaos, and Maternal interpretation styles
- ⚔️ **Job Flavoring** - Customize readings for your FFXIV job
- 📋 **Easy Copying** - Individual card copy buttons for roleplay
- ✨ **30+ Custom Cards** - All themed around FFXIV lore

## Installation

### Quick Install (Recommended)

Add the custom plugin repository to Dalamud:

1. In FFXIV, type `/xlsettings`
2. Go to **Experimental** tab
3. Add this URL to **Custom Plugin Repositories**:
   ```
   https://raw.githubusercontent.com/BluntEXE/VelvetRift/master/repo.json
   ```
4. Click **Save**, then type `/xlplugins`
5. Search for **"Moonshadow's Tarot Reader"** and install

**📖 Full installation guide:** [INSTALLATION.md](INSTALLATION.md)

### Prerequisites

**Ollama** - Required for AI interpretations:
```bash
ollama pull mistral
ollama serve
```

## Usage

1. **Start Ollama** before launching FFXIV:
   ```bash
   ollama serve
   ```

2. **In-game commands:**
   - `/tarot` - Open the tarot reader window

3. **Drawing cards:**
   - Enter your character name
   - Select interpretation mode, job flavor, and Ollama model
   - Click "Draw Single Card" or "Draw 3-Card Spread"
   - Wait for AI interpretations to generate
   - Use copy buttons to paste in chat with `/p`

## Configuration

Settings are stored in: `%APPDATA%\XIVLauncher\pluginConfigs\MoonshadowTarotReader.json`

Configurable options:
- Character name
- Interpretation mode (Eorzean/Chaos/Maternal)
- Job flavor (any FFXIV job)
- Ollama model selection
- Ollama API URL (default: http://localhost:11434/api/generate)

## Building from Source

### Requirements
- .NET 8.0 SDK or later
- Dalamud installed via XIVLauncher

### Build Instructions

```bash
cd SamplePlugin
dotnet build
```

Output will be in: `SamplePlugin/SamplePlugin/bin/x64/Debug/`

## Development

This plugin is built with:
- **Dalamud.NET.Sdk 13.1.0** - FFXIV plugin framework
- **ImGui** - User interface
- **Ollama** - Local LLM for AI interpretations

### Project Structure

```
SamplePlugin/
├── Data/
│   └── cards.json          # Tarot card definitions
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
│   └── MoonshadowTarotReader.json
└── MoonshadowTarotReader.sln
```

## Customization

### Adding New Cards

Edit `SamplePlugin/Data/cards.json`:

```json
{
  "Name": "Your Card Name",
  "Meaning": "Brief meaning description",
  "EorzeanInterpretation": "Optional",
  "ChaosMode": "Optional",
  "MaternalMode": "Optional"
}
```

### Modifying AI Prompts

Edit the prompt in `Services/OllamaService.cs` (lines 31-52)

## Troubleshooting

### "Cannot connect to Ollama"
- Ensure Ollama is running: `ollama serve`
- Check API URL in plugin settings
- Verify selected model is downloaded: `ollama list`

### Plugin won't load
- Check Dalamud logs: `/xllog` in-game
- Verify `cards.json` is in the plugin folder
- Ensure .NET 8.0 SDK is installed

### Interpretations not generating
- Check selected model is pulled: `ollama pull mistral`
- Try switching to a different model
- Check Dalamud logs for error messages

## License

AGPL-3.0-or-later

## Credits

- **Original Python Version**: Moonshadow's Tarot Reader (standalone app)
- **Dalamud Framework**: goatcorp
- **AI Integration**: Ollama

## Support

For issues, questions, or contributions, please open an issue on GitHub.

---

🔮 *May the cards guide your journey through Eorzea* ✨
