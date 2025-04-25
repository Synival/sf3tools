# SF3 Tools

**Frameworks and utilities for modifying Shining Force III**

## About

This is a fork of Rika's Tools for Shining Force III Hacking with enhancements, a single editor for all files, additional modifiable data, and a
completely rewritten architecture with a portable backend. All data can be edited using a program called SF3Editor (C# .NET 8.0 WinForms app)
or accessed by referencing the SF3Lib framework.

It boasts editors for data in several .BIN files along with an advanced editor for .MPD files for creating maps (still limited, but possible).

![image](https://github.com/user-attachments/assets/29d511ed-db26-4cfa-a841-431ef534cfbe)

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

| ![BTL91](https://github.com/user-attachments/assets/7bbaebce-a688-4a15-a06e-838f1345d4ae) | ![BTL02_Modified](https://github.com/user-attachments/assets/2c409599-3878-4e5d-9381-43073426b8e3) | ![TOMTOM00_TerrainAndEvents](https://github.com/user-attachments/assets/ef191c3b-3259-426b-9722-3beda3bb2c8e) |
|---|---|---|
| ![BTL47_PrettyLighting](https://github.com/user-attachments/assets/20a7ddcc-2d6c-4022-b708-1c29ea120511) | ![image](https://github.com/user-attachments/assets/6ac9cee6-ff0f-4186-a7e2-aedd99e81071) | ![Animations](https://github.com/user-attachments/assets/f0ceed56-293b-49de-8f91-521dbaff488f) |
| ![image](https://github.com/user-attachments/assets/26de635b-010a-4918-87eb-e0697ff6e4b6) | ![image](https://github.com/user-attachments/assets/0aa93801-efb3-4ae0-a80c-fdd8e82844aa) | ![image](https://github.com/user-attachments/assets/d7516e4a-6fec-41f1-b211-4eb5a425edf6) |


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
