# Installation Guide

## Prerequisites

1. **FINAL FANTASY XIV** with **XIVLauncher** and **Dalamud** installed
2. **Ollama** installed and running with a model downloaded

### Installing Ollama

1. Download Ollama from: https://ollama.com/download
2. Install and open Ollama
3. Download a model (choose one):
   ```bash
   ollama pull mistral
   # or
   ollama pull llama2
   # or
   ollama pull gemma
   ```
4. Start Ollama server:
   ```bash
   ollama serve
   ```

## Plugin Installation

### Method 1: Custom Plugin Repository (Recommended)

1. Launch FFXIV with XIVLauncher
2. Type `/xlsettings` in chat to open Dalamud Settings
3. Go to the **Experimental** tab
4. Scroll to **Custom Plugin Repositories**
5. Paste this URL and click the **+** button:
   ```
   https://raw.githubusercontent.com/BluntEXE/VelvetRift/master/repo.json
   ```
6. Click **Save and Close**
7. Type `/xlplugins` in chat
8. Search for **"Moonshadow's Tarot Reader"**
9. Click **Install**

### Method 2: Manual Installation

1. Download the latest release from: https://github.com/BluntEXE/VelvetRift/releases
2. Extract `MoonshadowTarotReader.zip`
3. Copy the contents to:
   ```
   %APPDATA%\XIVLauncher\devPlugins\MoonshadowTarotReader\
   ```
4. In FFXIV, type `/xlsettings`
5. Go to **Experimental** tab
6. Enable **"Dev Plugin Locations"** (if not already enabled)
7. Type `/xlplugins` and click the reload button

## Usage

1. **Make sure Ollama is running** before playing FFXIV:
   ```bash
   ollama serve
   ```

2. **In-game commands:**
   - `/tarot` - Opens the tarot reader window

3. **Using the plugin:**
   - Enter your character name
   - Select interpretation mode (Eorzean, Chaos, or Maternal)
   - Choose a job flavor (optional)
   - Select your Ollama model (mistral, llama2, or gemma)
   - Click "Draw Single Card" or "Draw 3-Card Spread"
   - Wait for AI interpretations to generate (5-30 seconds)
   - Use copy buttons to paste readings in chat with `/p`

## Configuration

### Plugin Settings

Access settings by clicking **"Show Settings"** in the tarot reader window.

- **Ollama API URL**: Default is `http://localhost:11434/api/generate`
- Change this if Ollama is running on a different port

### Saved Settings

Your settings are automatically saved:
- Character name
- Last used interpretation mode
- Job flavor selection
- Ollama model preference

## Troubleshooting

### "Cannot connect to Ollama"

**Solution:**
1. Make sure Ollama is running: `ollama serve`
2. Check the selected model is downloaded: `ollama list`
3. Verify API URL in plugin settings

### Plugin doesn't appear in /xlplugins

**Solution:**
1. Make sure you added the custom repository URL correctly
2. Try restarting FFXIV
3. Check Dalamud logs with `/xllog`

### Interpretations take too long or fail

**Solution:**
1. Try a different model (mistral is usually fastest)
2. Make sure Ollama has enough system resources
3. Check `/xllog` for error messages

### Plugin won't load

**Solution:**
1. Make sure you're using the latest version of Dalamud
2. Check if `cards.json` is in the plugin folder
3. View error details in `/xllog`

## Updating

### If using Custom Repository:
1. Type `/xlplugins` in-game
2. Click the update button next to the plugin

### If using Manual Installation:
1. Download the latest release
2. Replace all files in the plugin folder
3. Reload plugins with `/xlplugins`

## Uninstalling

1. Type `/xlplugins` in chat
2. Find "Moonshadow's Tarot Reader"
3. Click the trash icon to uninstall

**Optional:** Remove the custom repository:
1. Type `/xlsettings`
2. Go to **Experimental** tab
3. Remove the repository URL from **Custom Plugin Repositories**

## Support

For issues, bugs, or feature requests:
- Open an issue: https://github.com/BluntEXE/VelvetRift/issues
- Include `/xllog` output if reporting bugs

---

🔮 *May the cards guide your journey* ✨
