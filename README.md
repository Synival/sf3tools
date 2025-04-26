# SF3 Tools

**Frameworks and utilities for modifying Shining Force III**

## About

This is a fork of Rika's Tools for Shining Force III Hacking with enhancements, a single editor for all files, additional modifiable data, and a
completely rewritten architecture with a portable backend. All data can be edited using a program called SF3Editor (C# .NET 8.0 WinForms app)
or accessed by referencing the SF3Lib framework.

It boasts editors for data in several .BIN files along with an advanced editor for .MPD files for creating maps (still limited, but possible).

![image](https://github.com/user-attachments/assets/bfc97004-066d-4ea2-b3fc-5704cc797efe)

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

Can be used to view and modify all .MPD files across all scenarios. Not all data is yet loaded.

| ![BTL91](https://github.com/user-attachments/assets/bb61fd38-061f-4918-94e3-7d6f7101680c) | ![BTL02 Modified](https://github.com/user-attachments/assets/1594105a-5908-47f8-8ae3-b7fa868acf59) | ![TOMT00.MPD](https://github.com/user-attachments/assets/3bf0b8a2-3ee2-4e67-a8e6-aa7a9ff5b54a) |
|---|---|---|
| ![BTL47.MPD](https://github.com/user-attachments/assets/6d1eba16-512e-4273-8b7a-67f1e9dded49) | ![DLMANT.MPD](https://github.com/user-attachments/assets/52e0b556-dbee-4c79-b256-996a98e05bab) | ![HNSNOP.MPD](https://github.com/user-attachments/assets/abea6ffd-2387-4e54-bf60-eeb5279ea930) |
| ![DAIDAI.MPD](https://github.com/user-attachments/assets/ece14fc0-33a4-402f-978c-f4d181f61b5d) | ![LECHA.MPD](https://github.com/user-attachments/assets/6e1cb86b-a217-4b62-92f6-b6d26c28bc67) | ![Model Viewer](https://github.com/user-attachments/assets/807a45ce-fe25-4c0d-85a0-9776efc74da0) |

Features:

- **All visual elements** of each MPD are faithfully reproduced
- **All data contained** in the MPD files has been loaded appropriately and can be modified
- Tiles can be selected and their properties can be modified
- Individual rendered components can be toggles on and off
- Extra technical components can be displayed like collision lines, terrain types, tile event IDs, camera/battle cursor boundaries, and normal maps
- 3D models, textures, animations, and images can be viewed
- Textures can be exported and replaced via import
- Lighting direction and palette can be modified
- Camera/battle boundaries, gradients, color palettes, and much more can be modified
- Normal maps can be recalculated using a better method than used in SF3, fixing some bugs and quirks
- **Experimental:** The map FIELD.MPD on Scenario 3 and the Premium Disk can be drawn upon using a special set of brush tools enabled in the "Edit" menu. This is designed to work with a modified version of this file with extra textures, but it's mostly functional with the vanilla MPD.

### .BIN Files

Various tables and other bits of data can be modified across a large number of .BIN files:

- `X1*.BIN` files: Individual programs for scenes. Can modify event triggers, battle information (like enemy placements and AI), and town information (like NPC placements)
- `X002.BIN`: Items, spells, spells granted by weapons, the master scene loading table, and other misc. data 
- `X005.BIN`: Gameplay camera settings
- `X011.BIN`, `X021.BIN`, `X026.BIN`: Pointers for icons
- `X012.BIN`: Scenario 1 terrain movement
- `X013.BIN`: Special attacks, partnership bonuses/chances, and several misc. battle stats
- `X014.BIN`: 3D scenes played during battles (Scenario 2+)
- `X019.BIN`: Monster/enemy stats
- `X031.BIN, X033.BIN`: Player stats and growth charts
-  X011.BIN, X021.BIN, X026.BIN: Icon pointers

## Additional Tools

### DFRTool

A command line tool to create and apply .DFR files used by the SF3 Translation tool.

### DFRToolGUI

A GUI frontend for the DFRTool.
