# SF3 Tools

**Editors, frameworks, and utilities for modifying Shining Force III**

## About

SF3Tools is a collection of editors, frameworks, and utilities for modifying many aspects of each disc of Shining Force III.

The featured software suite, SF3Editor, can edit much of the game's data, create new maps, export and modify assets, and manipulate binary data.

This project was originally a fork of Rika's Tools for Shining Force III hacking, which itself was a hack of BoneIdol's tools.

| ![image](https://github.com/user-attachments/assets/bfc97004-066d-4ea2-b3fc-5704cc797efe) | ![X014.BIN](https://github.com/user-attachments/assets/f38620a8-5f80-4a80-a182-e54dbcd46aea) |
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

| ![BTL91](https://github.com/user-attachments/assets/bb61fd38-061f-4918-94e3-7d6f7101680c) | ![BTL02 Modified](https://github.com/user-attachments/assets/1594105a-5908-47f8-8ae3-b7fa868acf59) | ![TOMT00.MPD](https://github.com/user-attachments/assets/3bf0b8a2-3ee2-4e67-a8e6-aa7a9ff5b54a) |
|---|---|---|
| ![BTL47.MPD](https://github.com/user-attachments/assets/6d1eba16-512e-4273-8b7a-67f1e9dded49) | ![DLMANT.MPD](https://github.com/user-attachments/assets/52e0b556-dbee-4c79-b256-996a98e05bab) | ![HNSNOP.MPD](https://github.com/user-attachments/assets/abea6ffd-2387-4e54-bf60-eeb5279ea930) |
| ![DAIDAI.MPD](https://github.com/user-attachments/assets/ece14fc0-33a4-402f-978c-f4d181f61b5d) | ![LECHA.MPD](https://github.com/user-attachments/assets/6e1cb86b-a217-4b62-92f6-b6d26c28bc67) | ![Model Viewer](https://github.com/user-attachments/assets/807a45ce-fe25-4c0d-85a0-9776efc74da0) |

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
