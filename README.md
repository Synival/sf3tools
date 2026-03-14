# SF3 Tools

**Editors, frameworks, and utilities for modifying Shining Force III**

## About

SF3Tools is a collection of editors, frameworks, and utilities for modifying many aspects of each disc of Shining Force III.

The featured software suite, SF3Editor, can edit much of the game's data, create new maps, export and modify assets, and manipulate binary data.

This project was originally a fork of Rika's Tools for Shining Force III hacking, which itself was a hack of BoneIdol's tools.

| ![MPD Editor](Screenshots/v0.3.0%20Map%20Editor.png) | ![X014.BIN](Screenshots/v0.3.0%20X014.png) |
|---|---|

All of the work done by the editors is performed in a cross-platform backend project called SF3Lib. SF3Lib can
be used to create small applications that tasks like make bulk changes, randomize data, perform analysis, or anything
else you can think of in a console app.

## How to Use

To edit files, simply:
- Run `SF3Editor.exe`
- Open any supported file using the "File -> Open" menu from either:
    - the game disc (scenario auto-detected), or
    - any other folder (must select the scenario via the "File -> Open Scenario" menu)

## Files Currently Supported

### .MPD Files

Can be used to view and modify all .MPD files across all scenarios.

| ![BTL91](Screenshots/v0.3.0%20BTL91.png) | ![BTL02](Screenshots/v0.3.0%20BTL02.png) | ![TOMT00.MPD](Screenshots/v0.3.0%20TOMT00.png) |
|---|---|---|
| ![BTL47.MPD](Screenshots/v0.3.0%20BTL47.png) | ![DLMANT.MPD](Screenshots/v0.3.0%20DLMANT.png) | ![TANK00](Screenshots/v0.3.0%20TANK00.gif) |
| ![DAIDAI.MPD](Screenshots/v0.3.0%20DAIDAI.png) | ![LECHA.MPD](Screenshots/v0.3.0%20LECHA.png) | ![Model Viewer](Screenshots/v0.3.0%20ModelViewer.png) |

Features:

- **All visual elements** of each MPD are faithfully reproduced (although not 100% hardware accurate)
- **All data contained** in the MPD files is loaded and be modified
- Tiles and models can be selected and their properties can be modified
- Extra technical components can be displayed like collision lines, terrain types, tile event IDs, camera/battle cursor boundaries, and normal maps
- Textures, animation frames, plane images, and tilesets can be exported or replaced
- 3D model viewer
- Lighting can be modified, exported, and imported
- Camera/battle boundaries, gradient effects, and other color effects present in MPD files can be modified
- Normal maps can be recalculated using a better method than used in SF3, fixing some bugs and quirks
- **Experimental:** The map FIELD.MPD on Scenario 3 and the Premium Disk can be drawn upon using a special set of brush tools enabled in the "View -> MPD" menu. This is designed to work with a modified version of this file with extra textures, but it's mostly functional with the vanilla MPD.

### Faces / Portraits

`KAO*.DAT` and `FACE*.DAT` files can be opened and edited, including the complete replacement of animated portraits.

(More info TBD!)

### Sprites

Sprites are fully modifiable, but there's a catch: although we can view the contents of `.CHR` and `.CHP` files, editing and creating them must be done using the `chrtool.exe` command-line tool, which requires a development environment setup under Windows' Linux Subsystem.

Please check out the "SF3Sprites" project:
https://github.com/Synival/sf3sprites

### .BIN Files

Various tables and other bits of data can be modified across a large number of .BIN files:

- `X1*.BIN` files: Individual programs for scenes. Can modify event triggers, battle data, town/scene data, blacksmith tables, and much more!
- `X002.BIN`: Items, spells, spells granted by weapons, the master scene loading table, and other misc. data 
- `X005.BIN`: Gameplay camera settings
- `X011.BIN`, `X021.BIN`, `X026.BIN`, `X032.BIN`: Pointers for icons
- `X012.BIN`: Scenario 1 terrain movement and miscellaneous AI-related tables
- `X013.BIN`: Special attacks, partnership bonuses/chances, special attack/spell animations, and several misc. battle stats
- `X014.BIN`: 3D scenes played during battles, models used for players/enemies during battle, and more special attack/spell animation data
- `X019.BIN`, `X044.BIN` (PD): Monster/enemy stats
- `X023.BIN`: Shop data
- `X023.BIN`, `X024.BIN`, `X027.BIN`: Blacksmith data (Scenario 3+)
- `X031.BIN, X033.BIN`: Player stats and growth charts
- `X011.BIN, X021.BIN, X026.BIN`: Icon pointers

## Additional Tools

### CHRTool

A command line tool for ompiling and decompiling sprite data to and from `.CHR` and `.CHP` files.

### DFRTool

A command line tool to create and apply .DFR files used by the SF3 Translation tool.

### DFRToolGUI

A GUI frontend for the DFRTool.

### SF3Compress

A command line tool for compressing and decompressing data in Shining Force 3's LZSS format.
