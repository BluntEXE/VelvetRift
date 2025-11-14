# VelvetRift - Dalamud Plugins for FFXIV

A collection of Dalamud plugins for Final Fantasy XIV, focused on roleplay, social activities, and entertainment at venues.

[![Repository](https://img.shields.io/badge/Repository-VelvetRift-blueviolet)](https://github.com/BluntEXE/VelvetRift)
[![Dalamud](https://img.shields.io/badge/Dalamud-API%2013-blue)](https://dalamud.dev)
[![License](https://img.shields.io/badge/License-See%20Individual%20Plugins-lightgrey)](#)

---

## Plugins

### 🃏 Eorzean Degeneracy
**A Cards Against Humanity-style party game for FFXIV degenerates**

[![Version](https://img.shields.io/badge/Version-0.0.0.1-green)](https://github.com/BluntEXE/VelvetRift/releases/tag/v0.0.1-beta)
[![Status](https://img.shields.io/badge/Status-Beta-yellow)](#)

> *"Because even Lalafells deserve bad jokes."*

Play a Cards Against Humanity-style party game with your Free Company, friends, or random strangers at your favorite tavern. Features FFXIV-themed prompts and answers that will make you laugh, cringe, and question your life choices.

**Features:**
- 🎴 **7 Themed Card Packs**: 296 prompt cards and 743 answer cards across Core, Lore, Community, Spicy (18+), Jobs, Raid, and Crafting & Gathering packs
- 🎮 **Turn-Based Gameplay**: Classic Card Czar rotation with anonymous submissions and scoring
- ⚙️ **Customizable**: Choose which card packs to enable, set round timers, and even type custom answers
- 🤖 **Solo-Friendly**: Test the game with AI players before bringing it to your venue
- 🎨 **Responsive UI**: Cards adapt to your window size with proper text wrapping
- ⏱️ **Round Timer**: Configurable time limits with auto-submission to keep games moving

**Commands:**
- `/deg start` - Start a new game
- `/deg join` - Join an ongoing game
- `/deg config` - Open settings
- `/deg test` - Add AI test players

**Perfect for:**
- Club and tavern events
- FC game nights
- Breaking the ice with strangers
- Questionable entertainment at 3am in Limsa

[View Release](https://github.com/BluntEXE/VelvetRift/releases/tag/v0.0.1-beta)

---

### 🔮 Moonshadow's Tarot Reader
**AI-powered tarot readings for FFXIV roleplay**

[![Version](https://img.shields.io/badge/Version-1.0.0.0-green)](https://github.com/BluntEXE/VelvetRift/releases/tag/v1.0.4)
[![Status](https://img.shields.io/badge/Status-Stable-brightgreen)](#)

> *"FFXIV-themed tarot readings with AI-generated interpretations"*

Draw tarot cards and receive AI-powered interpretations tailored to your FFXIV character and roleplay. Perfect for divination-themed RP, fortune-telling venues, or just exploring your Warrior of Light's fate.

**Features:**
- 🎴 **Complete Tarot Deck**: Major and Minor Arcana with FFXIV theming
- 🤖 **AI Interpretations**: Powered by Ollama for local LLM generation
- 🎭 **Three Reading Modes**:
  - **Eorzean**: Grounded, lore-friendly interpretations
  - **Chaos**: Wild, unpredictable readings
  - **Maternal**: Warm, nurturing guidance
- 📖 **Multiple Spreads**: Single card draws and three-card spreads
- 🎨 **Beautiful UI**: Custom tarot card visuals and layouts

**Commands:**
- `/tarot` - Open the tarot reader
- `/tarot draw` - Quick single card draw
- `/tarot spread` - Three-card spread

**Perfect for:**
- Fortune-telling RP venues
- Astrologian character development
- Mystery and intrigue storylines
- Character guidance and reflection

[View Release](https://github.com/BluntEXE/VelvetRift/releases/tag/v1.0.4)

---

## Installation

### Prerequisites
- [XIVLauncher](https://goatcorp.github.io/faq/xl_troubleshooting) with Dalamud enabled
- Final Fantasy XIV game client

### Add Custom Repository

1. Launch FFXIV with XIVLauncher/Dalamud
2. Type `/xlsettings` in chat to open Dalamud Settings
3. Navigate to the **Experimental** tab
4. Scroll to **Custom Plugin Repositories**
5. Add this URL:
   ```
   https://raw.githubusercontent.com/BluntEXE/VelvetRift/master/repo.json
   ```
6. Click the **+** button to save
7. Click **Save and Close**

### Install Plugins

1. Type `/xlplugins` in chat to open the Plugin Installer
2. Search for the plugin you want:
   - "Eorzean Degeneracy" for the card game
   - "Moonshadow's Tarot Reader" for tarot readings
3. Click **Install**
4. The plugin will download with all required assets

---

## Support & Community

- **Issues**: [GitHub Issues](https://github.com/BluntEXE/VelvetRift/issues)
- **Author**: Ehno
- **Repository**: [VelvetRift](https://github.com/BluntEXE/VelvetRift)

---

## Development

### Repository Structure
```
VelvetRift/
├── Project Ideas/
│   ├── Eorzean Degeneracy/    # Card game plugin
│   │   ├── EorzeanDegeneracy/ # Source code
│   │   ├── Data/              # Card packs
│   │   └── PROGRESS_LOG.md    # Development log
│   └── ideas.md               # Plugin concepts
├── .claude/                   # Claude Code agents
├── repo.json                  # Dalamud repository manifest
└── README.md                  # This file
```

### Building from Source

**Eorzean Degeneracy:**
```bash
cd "Project Ideas/Eorzean Degeneracy"
dotnet build EorzeanDegeneracy.sln
```

Output: `EorzeanDegeneracy/bin/Debug/EorzeanDegeneracy.dll`

### Development Docs
- [Eorzean Degeneracy Progress Log](Project%20Ideas/Eorzean%20Degeneracy/PROGRESS_LOG.md)
- [Plugin Ideas](Project%20Ideas/ideas.md)
- [Dalamud Developer Docs](https://dalamud.dev)

---

## Plugin Information

### Eorzean Degeneracy
- **Version**: 0.0.0.1
- **Release**: v0.0.1-beta
- **Status**: Beta (fully functional, testing phase)
- **Content**: 296 prompts, 743 answers across 7 packs
- **Rating**: 18+ content available (optional Spicy Pack)

### Moonshadow's Tarot Reader
- **Version**: 1.0.0.0
- **Release**: v1.0.4
- **Status**: Stable
- **Requirements**: Ollama for AI interpretations
- **Rating**: All ages

---

## License

See individual plugin directories for license information.

---

## Acknowledgments

- **Dalamud Team**: For the incredible plugin framework
- **goatcorp**: For XIVLauncher and Dalamud ecosystem
- **FFXIV RP Community**: For inspiration and feedback

---

**Disclaimer**: These plugins are third-party tools and are not affiliated with or endorsed by Square Enix. Use at your own risk.
