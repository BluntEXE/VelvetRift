# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Moonshadow's Tarot Reader is a Dalamud plugin for Final Fantasy XIV that performs tarot card readings with AI-generated interpretations. The plugin integrates with Ollama for local LLM inference to generate thematic interpretations based on FFXIV lore and aesthetics.

The repository contains both:
- **Original Python version** (reader.py, cards.json) - Standalone tkinter application
- **C# Dalamud Plugin** (SamplePlugin/) - In-game FFXIV plugin

## Architecture

### Python Version (Legacy)

- **reader.py**: Main application file containing all UI, logic, and AI integration
- **cards.json**: Tarot card data with 30+ custom cards themed around FFXIV
- **moonshadow.ico**: Application icon
- **background.png**: UI background image (not tracked, referenced in code at line 274)

### C# Dalamud Plugin (Current)

**Plugin Structure** (SamplePlugin/):
- **Plugin.cs**: Main plugin entry point, registers `/tarot` command, initializes services
- **Configuration.cs**: Persistent settings (character name, mode, job, Ollama config)
- **Models/TarotCard.cs**: Data model for cards from JSON
- **Services/TarotService.cs**: Card deck management, drawing logic, state tracking
- **Services/OllamaService.cs**: Async HTTP client for Ollama API integration
- **Windows/TarotReaderWindow.cs**: Main ImGui window (three-panel layout)
- **Windows/ConfigWindow.cs**: Settings window for Ollama API configuration
- **Data/cards.json**: Tarot card data (30+ custom FFXIV-themed cards)

### Key Technical Details

**AI Integration (lines 73-100)**:
- Uses Ollama API at `http://localhost:11434/api/generate`
- Supports multiple models: mistral, llama2, gemma (configurable via dropdown)
- Streaming response processing for real-time interpretation generation
- Model auto-launch via subprocess on selection change (lines 38-54)

**UI Architecture (lines 184-428)**:
- Custom borderless window with custom title bar (overrideredirect + drag-to-move)
- Hidden root window + Toplevel for proper Windows taskbar integration
- Three-column layout: Left (settings), Center (actions), Right (clipboard operations)
- Neon/dark theme with hover effects and custom styling

**Card Reading Modes (lines 109-172)**:
- Single card draw: Random card + direction (upright/reversed) + AI interpretation
- Three-card spread: Past/Present/Future positions with individual interpretations
- Regenerate: Re-generate AI interpretations for last draw without changing cards
- All outputs formatted for FFXIV roleplay (`/p` emote prefix)

**Global State Management**:
- Lines 22-35: Global variables store last draw state (cards, labels, output text)
- Enables regeneration and selective copying without redrawing cards

**PyInstaller Compatibility**:
- `resource_path()` function (lines 13-16) handles bundled resource loading
- Used for cards.json, moonshadow.ico, and background.png

## Building and Running

### Dalamud Plugin (C#)

**Prerequisites**:
- .NET 8.0 SDK or later
- Dalamud installed via XIVLauncher
- Ollama running locally with a supported model

**Build**:
```bash
cd SamplePlugin
dotnet build
```

**Install for Testing**:
```bash
# Copy build output to Dalamud dev plugins folder
# Output: SamplePlugin/bin/Debug/net8.0-windows/
# Install to: %APPDATA%\XIVLauncher\devPlugins\MoonshadowTarotReader\
```

**Run Ollama**:
```bash
ollama pull mistral  # or llama2, gemma
ollama serve
```

**In-game Usage**:
- `/tarot` - Open tarot reader window
- `/xlplugins` - Manage plugins
- Settings are auto-saved

### Python Version (Legacy)

**Dependencies**:
```bash
pip install tkinter requests pillow
```

**Run**:
```bash
python reader.py
```

## Development Notes

### Dalamud Plugin Development

**Service Pattern**:
- TarotService manages card state and drawing logic
- OllamaService handles async HTTP communication
- Services initialized in Plugin.cs constructor
- Services should be disposed in Plugin.Dispose()

**ImGui Window System**:
- Windows extend `Window` class from Dalamud.Interface.Windowing
- Draw() method called every frame when window is open
- Use ImGui.BeginChild() for scrollable regions
- ImGui state management requires local variables for ref parameters

**Async Operations**:
- OllamaService.GenerateInterpretationAsync() uses async/await
- Window methods calling async code should be `async void`
- Use isGenerating flag to prevent concurrent requests
- Display status messages during long operations

**Configuration Persistence**:
- Configuration.cs implements IPluginConfiguration
- Auto-saved via Plugin.PluginInterface.SavePluginConfig()
- Settings loaded in window constructor
- Changes saved immediately on user interaction

**Key Files to Modify**:
- TarotReaderWindow.cs:209-283 - Card drawing and UI logic
- OllamaService.cs:31-52 - AI prompt template
- TarotService.cs:31-60 - Drawing mechanics
- Configuration.cs:14-19 - Add new settings

### Customizing Interpretations

Edit `cards.json` to modify card meanings. Each card requires:
- `Name`: Card display name
- `Meaning`: Brief card meaning description
- Optional: `EorzeanInterpretation`, `ChaosMode`, `MaternalMode` (not currently used by AI, may be legacy)

AI interpretations are generated dynamically based on:
- Card name
- Interpretation mode (Eorzean/Chaos/Maternal)
- Character name
- Optional job flavor (FFXIV jobs: Paladin, White Mage, etc.)

### Modifying AI Behavior

The prompt template is at lines 76-80 in `generate_ai_interpretation()`. The prompt requests:
- Poetic, immersive tone
- Mode-specific flavor
- Character personalization
- Job-specific theming (optional)
- Under 50 words

### UI Customization

Color scheme and styling:
- Lines 233-266: Button/entry styles and neon theme
- Primary colors: `#2A1B3D` (dark purple), `#B829FE` (neon purple), `#121218` (dark bg)
- Hover effects: lines 269-271

Window behavior:
- Lines 190-216: Taskbar integration and borderless window setup
- Lines 218-226: Drag-to-move implementation

## Important Patterns

1. **Thread Safety**: Model launch runs in daemon thread (line 355) to avoid blocking UI
2. **Ollama Process Management**: Process stored globally (line 34) and cleaned up on exit (lines 56-60)
3. **Clipboard Operations**: All outputs have individual copy buttons for FFXIV RP paste convenience
4. **Error Handling**: Minimal error handling - streaming errors printed to console (line 98)
5. **Model Status**: Auto-refreshing status label updates every 1000ms (lines 416-418)
