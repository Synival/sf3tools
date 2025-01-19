# SF3 Tools

A fork of Rika's Tools for Shining Force III Hacking with enhancements, new editors, additional modifiable data, and a
completely rewritten architecture.

Almost all of the work done by the editors is performed in a cross-platform backend project called SF3Lib. SF3Lib can
be used to create small applications that tasks like make bulk changes, randomize data, perform analysis, or anything
else you can think of in a console app.

The editors themselves are written for .NET 8.0 using Winforms.

## Editors

### MPD Editor

Can be used to view and modify all .MPD files across all scenarios. Not all data is yet loaded.

| ![BTL02_CatchupMountain](https://github.com/user-attachments/assets/6845cf02-a395-4946-91bb-d57ee474f9a2) | ![TOMTOM00_TerrainAndEvents](https://github.com/user-attachments/assets/f8b4a005-5922-451b-a4f4-e32aec455600) |
|---|---|
| ![BTL47_PrettyLighting](https://github.com/user-attachments/assets/dc410ccf-9f38-4d9f-9033-f69958b0d332) | ![DORA_LookAtThemDragons](https://github.com/user-attachments/assets/774f48df-811c-4dd3-9ea3-d77429c06841) |

Features:

- 3D viewer and editor with mouse and keyboard controls
- Tiles can be selected and their properties can be modified
- Special overlayed views for terrain types, tile event IDs, and camera/battle boundaries
- Normal maps can be recalculated using a better method than used in SF3
- Textures can be viewed, exported, and re-imported
- Animated textures can be viewed, and their individual frames can be exported and re-imported
- Light palette and direction can be modified
- Camera/battle boundaries can be modified
- Color palettes can be modified
- Chunk table is automatically re-compressed and rebuilt upon saving

### X1 Editor

Editor for X1\*.BIN files, which contain battle information (like enemy placements) and out-of-combat information (like
NPC placements).

Features:

(features wip)

### X002 Editor

(info wip)

### X013 Editor

(info wip)

### X019 Editor

(info wip)

### X033/X031 Editor

(info wip)

### IconPointerEditor

(info wip)

## Additional Tools

### DFRTool

A command line tool to create and apply .DFR files used by the SF3 Translation tool.

### DFRToolGUI

A GUI frontend for the DFRTool.
