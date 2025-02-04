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

| ![BTL02 Original](https://github.com/user-attachments/assets/6d555e9c-c68e-4a29-a7ae-2a808cf05bd3) | ![BTL02_Modified](https://github.com/user-attachments/assets/2c409599-3878-4e5d-9381-43073426b8e3) | ![TOMTOM00_TerrainAndEvents](https://github.com/user-attachments/assets/ef191c3b-3259-426b-9722-3beda3bb2c8e) |
|---|---|---|
| ![BTL47_PrettyLighting](https://github.com/user-attachments/assets/20a7ddcc-2d6c-4022-b708-1c29ea120511) | ![DORA_LookAtThemDragons](https://github.com/user-attachments/assets/cd1b8b01-d398-4486-b8eb-9fdcca3370e5) | ![Animations](https://github.com/user-attachments/assets/f0ceed56-293b-49de-8f91-521dbaff488f) |

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
