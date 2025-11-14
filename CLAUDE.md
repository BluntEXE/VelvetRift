# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Repository Purpose

This repository contains design documentation and content for **Dalamud plugins** targeting Final Fantasy XIV's roleplaying and social venue communities. The focus is on interactive party games and mini-games for clubs, taverns, and RP venues.

## Project Structure

### Project-Specific Content
- **`Project Ideas/`**: Directory containing all plugin ideas and plugin-specific content
  - **`ideas.md`**: Master document containing 6 complete plugin concepts with implementation details
    - Each concept includes: description, player interaction flow, venue appeal, technical feasibility assessment
    - Concepts range from simple (Emote Charades) to moderate complexity (Eorzean Degeneracy)
  - **`Eorzean Degeneracy/`**: Card packs for the Cards Against Humanity-style game

### Card Game Content (Eorzean Degeneracy)
Located in `Project Ideas/Eorzean Degeneracy/`:

Seven themed JSON card packs:

1. **Core Pack.json** - General FFXIV references (35 prompts, 100 answers)
2. **Lore Pack.json** - Story moments and NPCs (45 prompts, 100 answers)
3. **Community Pack.json** - Player culture and memes (46 prompts, 103 answers)
4. **Spicy Pack.json** - Adult humor, 18+ content (40 prompts, 100 answers)
5. **Jobs Pack.json** - Job-specific mechanics (40 prompts, 120 answers)
6. **Raid Pack.json** - Raiding and combat humor (45 prompts, 120 answers)
7. **Crafting and Gathering Pack.json** - Market Board and crafting (45 prompts, 100 answers)

**Total Content**: 296 prompt cards, 743 answer cards

### Card Pack JSON Schema
```json
{
  "pack_name": "string",
  "pack_description": "string",
  "version": "string",
  "rating": "PG-13|18+",
  "content_warning": "string (optional, for 18+ packs)",
  "prompt_cards": [
    {
      "text": "Prompt with ______ blanks",
      "blanks": 1
    }
  ],
  "answer_cards": [
    {
      "text": "Answer text"
    }
  ]
}
```

### Custom Claude Agents

Located in `.claude/agents/`:

1. **dalamud-plugin-expert.md** - Technical implementation specialist
   - C# programming for Dalamud framework
   - API integration, troubleshooting, optimization
   - Use when: coding plugins, debugging, performance issues

2. **dalamud-rp-idea-generator.md** - Creative ideation specialist
   - Generates RP-friendly plugin concepts
   - Evaluates fun factor and technical feasibility
   - Use when: brainstorming new features, refining concepts

---

## GitHub Repository Integration

### VelvetRift Multi-Plugin Repository

This local development directory (`F:\Claude`) is separate from but syncs with the **VelvetRift GitHub repository** at https://github.com/BluntEXE/VelvetRift

**Repository**: VelvetRift hosts **TWO Dalamud plugins**:

1. **Moonshadow's Tarot Reader** (v1.0.0.0)
   - AI-powered tarot readings for RP
   - Source location: `VelvetRift-repo/SamplePlugin/`
   - Release: v1.0.4
   - Status: ✅ Stable

2. **Eorzean Degeneracy** (v0.0.0.1)
   - Cards Against Humanity-style party game
   - Source location: Local development only (not in GitHub repo yet)
   - Release: v0.0.1-beta
   - Status: ✅ Beta (fully functional)

### Repository Locations

**GitHub Repository (Remote)**:
- URL: https://github.com/BluntEXE/VelvetRift
- Main branch: `master`
- Contains: Moonshadow's Tarot Reader source, repo.json, README.md, releases

**Local Git Clone** (`F:\VelvetRift-repo`):
- Purpose: For updating GitHub repository files
- Use for: Pushing changes to README.md, repo.json, images, etc.
- Synced with: GitHub master branch

**Local Development** (`F:\Claude`):
- Purpose: Plugin development, design docs, card content
- Contains: Eorzean Degeneracy source, plugin ideas, progress logs
- NOT a git repository (separate from VelvetRift-repo)

### Directory Structure Overview

```
F:\
├── Claude/                          # Local development workspace
│   ├── Project Ideas/
│   │   ├── Eorzean Degeneracy/     # ED plugin source & content
│   │   │   ├── EorzeanDegeneracy/  # C# source code
│   │   │   ├── Data/Card Packs/    # 7 JSON card packs
│   │   │   ├── PROGRESS_LOG.md     # Development history
│   │   │   └── repo.json           # Local copy of manifest
│   │   └── ideas.md                # All plugin concept designs
│   ├── .claude/agents/             # Custom Claude Code agents
│   ├── CLAUDE.md                   # This file
│   ├── README.md                   # Template for GitHub README
│   └── EDPluginProject.md          # Project documentation
│
└── VelvetRift-repo/                # GitHub repository clone
    ├── .git/                       # Git metadata
    ├── SamplePlugin/               # Moonshadow's Tarot Reader
    ├── images/                     # Plugin icons
    │   ├── icon.png               # Tarot Reader icon
    │   └── eorzean-degeneracy-icon.png
    ├── repo.json                   # Dalamud manifest (both plugins)
    ├── README.md                   # Main repository page
    └── INSTALLATION.md
```

### Dalamud Repository Manifest (repo.json)

**Location**: `F:\VelvetRift-repo/repo.json` (and `F:\Claude\Project Ideas\Eorzean Degeneracy\repo.json`)

**Purpose**: Tells Dalamud Plugin Installer about available plugins

**Format**: JSON array of plugin metadata
```json
[
  {
    "Name": "Moonshadow's Tarot Reader",
    "AssemblyVersion": "1.0.0.0",
    "DownloadLinkInstall": "https://github.com/.../v1.0.4/MoonshadowTarotReader.zip",
    ...
  },
  {
    "Name": "Eorzean Degeneracy",
    "AssemblyVersion": "0.0.0.1",
    "DownloadLinkInstall": "https://github.com/.../v0.0.1-beta/EorzeanDegeneracy.zip",
    ...
  }
]
```

**Users add this URL to Dalamud**:
```
https://raw.githubusercontent.com/BluntEXE/VelvetRift/master/repo.json
```

### Release Process

#### For Eorzean Degeneracy Updates:

1. **Build Plugin**:
   ```bash
   cd "F:\Claude\Project Ideas\Eorzean Degeneracy"
   dotnet build EorzeanDegeneracy.sln
   ```

2. **Create Release Package**:
   - Include: `EorzeanDegeneracy.dll`, `EorzeanDegeneracy.json`
   - Include: All 7 card pack JSON files (`*Pack.json`)
   - Create ZIP: `EorzeanDegeneracy.zip`

3. **Upload to GitHub**:
   - Create new release tag (e.g., `v0.0.0.2`)
   - Upload `EorzeanDegeneracy.zip`
   - Get download URL

4. **Update repo.json**:
   ```bash
   cd F:\VelvetRift-repo
   # Edit repo.json with new version and download URLs
   git add repo.json
   git commit -m "Update Eorzean Degeneracy to vX.X.X"
   git push
   ```

5. **Verify**: Users can now update via Dalamud Plugin Installer

#### For README Updates:

1. **Edit Template**: Make changes to `F:\Claude\README.md`
2. **Copy to Repo**:
   ```bash
   cp "F:\Claude\README.md" "F:\VelvetRift-repo\README.md"
   ```
3. **Commit and Push**:
   ```bash
   cd F:\VelvetRift-repo
   git add README.md
   git commit -m "Update README: [description]"
   git push
   ```
4. **Verify**: Check https://github.com/BluntEXE/VelvetRift

### Plugin Icons

**Required for each plugin** in repo.json's `IconUrl` field:

- **Moonshadow's Tarot Reader**: `images/icon.png`
- **Eorzean Degeneracy**: `images/eorzean-degeneracy-icon.png`

Icons must be uploaded to `VelvetRift-repo/images/` and pushed to GitHub.

### Current Status (2025-11-14)

**GitHub Repository State**:
- ✅ README showcases both plugins
- ✅ repo.json lists both plugins with correct download links
- ✅ Both plugin icons uploaded to `/images/`
- ✅ Releases available for both plugins
- ✅ Installation instructions provided

**Eorzean Degeneracy**:
- Version: 0.0.0.1
- Release: v0.0.1-beta
- Status: Beta (fully functional, in testing)
- Features: 7 card packs, 296 prompts, 743 answers
- Missing: Multiplayer networking (researched but not implemented)

**Moonshadow's Tarot Reader**:
- Version: 1.0.0.0
- Release: v1.0.4
- Status: Stable

---

## Content Guidelines

### When Adding New Plugin Ideas
Add new concepts to `Project Ideas/ideas.md`. Each plugin concept should include:
- **What It Is**: Clear one-paragraph description
- **How Players Interact**: Step-by-step gameplay flow
- **Why It's Fun at Clubs**: Venue appeal and social dynamics
- **Technical Feasibility**: Assessment (Simple/Moderate/Complex) with implementation notes

### When Creating New Card Packs
- Minimum 30 prompts, 80 answers per pack
- Use consistent JSON schema with proper metadata
- Include `blanks` field (1 or 2) for multi-blank prompts
- Add `content_warning` for mature content packs
- Test that prompt/answer combinations work naturally

### Content Themes for Cards
Focus areas proven to work well:
- FFXIV game mechanics and systems
- Community in-jokes and memes
- Story characters and lore moments
- Job-specific gameplay
- Raid/dungeon experiences
- Crafting and economy
- Data center/server culture

## Working with Card Content

### Expanding Existing Packs
Card packs are located in `Project Ideas/Eorzean Degeneracy/`:
```bash
# Card packs are simple JSON - edit directly
# Maintain alphabetical or thematic ordering for answers
# Keep prompt variety: single-blank, double-blank, different subjects
```

### Creating New Themed Packs
Add new packs to `Project Ideas/Eorzean Degeneracy/`. Consider these successful themes:
- PvP Pack (Frontlines, Crystalline Conflict humor)
- Housing Pack (decoration, neighbors, Savage housing)
- Glamour Pack (fashion, dye combinations, Mogstation)
- Beast Tribes Pack (daily quests, tribal lore)
- Gold Saucer Pack (mini-games, MGP grinding)

## Development Context

### Target Platform
- **Dalamud Framework**: FFXIV plugin loader
- **Language**: C# with .NET
- **UI Framework**: ImGui via Dalamud
- **Data Format**: JSON for all content storage

### Design Philosophy
1. **Simple First**: Prefer chat-based commands over complex UIs
2. **Social Focus**: Enhance player interaction, never isolate
3. **Venue-Friendly**: Quick setup, minimal host burden
4. **RP-Positive**: Respect player agency and inclusivity
5. **Performance-Conscious**: Minimal impact on game client

### Technical Feasibility Rankings
- **Simple**: Text processing, RNG, timers, chat monitoring, JSON storage
- **Moderate**: UI panels, proximity detection, state machines, player data access
- **Complex**: Real-time sync, visual effects, game world manipulation, memory hooks

## Using the Custom Agents

### Invoke dalamud-rp-idea-generator when:
- User wants new plugin concepts
- Brainstorming RP features
- Evaluating game mechanic ideas
- Needs creative direction for venues

### Invoke dalamud-plugin-expert when:
- Writing C# plugin code
- Troubleshooting technical issues
- Understanding Dalamud APIs
- Debugging or optimization needed

## File Naming Conventions

- Plugin ideas: `Project Ideas/ideas.md` (master document with all concepts)
- Card packs: `Project Ideas/Eorzean Degeneracy/{Theme} Pack.json` (title case, space-separated)
- Plugin-specific directories: `Project Ideas/{PluginName}/` for assets and content
- Individual plugin designs: `Project Ideas/{PluginName}-design.md` for standalone concept docs
- Progress logs: `Project Ideas/{PluginName}/PROGRESS_LOG.md` (documents development history)
- GitHub README template: `F:\Claude\README.md` (synced to `F:\VelvetRift-repo\README.md`)

## Working with the GitHub Repository

### When to Use VelvetRift-repo

Use the `F:\VelvetRift-repo` clone when:
- Updating the main repository README.md
- Modifying repo.json (adding plugins, updating versions)
- Adding/updating plugin icons in `/images/`
- Pushing any changes that affect the GitHub repository page

### When to Work in F:\Claude

Use the `F:\Claude` directory when:
- Developing plugin source code (Eorzean Degeneracy)
- Creating/editing card packs
- Writing design documentation
- Updating plugin-specific PROGRESS_LOG.md

### Git Workflow for Repository Updates

**Always work from VelvetRift-repo for GitHub changes**:

1. **Sync with GitHub**:
   ```bash
   cd F:\VelvetRift-repo
   git pull origin master
   ```

2. **Make Changes**: Edit files in `F:\VelvetRift-repo\`

3. **Stage and Commit**:
   ```bash
   git add [files]
   git commit -m "Description of changes"
   ```

4. **Push to GitHub**:
   ```bash
   git push origin master
   ```

5. **Verify**: Check https://github.com/BluntEXE/VelvetRift

### Keeping Directories in Sync

**README.md Workflow**:
- Edit template: `F:\Claude\README.md`
- Copy to repo: `cp F:\Claude\README.md F:\VelvetRift-repo\README.md`
- Commit and push from VelvetRift-repo

**repo.json Workflow**:
- Local copy exists at: `F:\Claude\Project Ideas\Eorzean Degeneracy\repo.json`
- Authoritative version: `F:\VelvetRift-repo\repo.json`
- Always update VelvetRift-repo version and push to GitHub

## Quality Standards

### Plugin Ideas
- Must be achievable within Dalamud's capabilities
- Should enhance social gameplay
- Needs clear player value proposition
- Includes realistic technical assessment

### Card Content
- FFXIV-relevant and community-recognizable
- Appropriate for target rating
- Grammatically complete
- Naturally combinable with prompts
- Avoids repetition within pack
