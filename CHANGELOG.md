## SF3Editor and SF3Lib v0.1.1 (2025-04-26)

Another release, another huge batch of changes!

This release adds a lot of features and accomplishes a major milestone: all of the file editors are now bundled together
into a single master editor! The code reorganization to make this possible has been in the works for quite a while and I'm
very happy that this goal has finally be reached. The edito supports opening multiple files into tabs, scenario /
file type autodetection, and all the featues found in the standalone editos (and more!).

The MPD editor is also more complete: all data is now loaded and supported for editing, it's more configurable, it
renders more accurately, and it's a bit easier to use.

With some extra hacking outside the editor, the MPD editor can now be used to make new overworld maps for Shining
Force III. This is still in the experimental stages and only works with a very specific file (which, unfortunately,
cannot be made public), but it's approaching! To get a glimpse of how it can work, enable the experimental setting
at the bottom of the "Settings | MPD" menu and play around with "FIELD.MPD" on the Scenario 3 or Premium Disk.

X1 files have been demystified a bit: it's now much easier to modify game events and triggers, warp locations, and some
aspects of the enemy AI.

Enjoy!
-- Synival

Full changelog:

### SF3Lib (all editors/files):
- Added more dropdowns:
    - Game flags (Not all game flags referenced are documented yet; Scenario 2 and 3 still need some work)
    - Sprites (Scenario 1 and 2 have been identified, but Scenario 3 + PD are still WIP and marked with a preceeding '!' mark)
    - 'Load' table name
    - Music track names
    - Magic bonus table reference
    - Special attack type (crit or counter)
- Renamed 'EntranceID' and 'SceneNumber' to 'SceneID' everywhere for consistency
- Updated names of items and monsters to match their SF3 Translation Team names and corrected mistakes.
    - (Thanks to Clear Crossroads for the names and info!)

### SF3LibWin:
- Bugfix: Decimal entry control no longer truncates values to only their integer component

### SF3Editor:
- All standalone editors have been merged into one single editor called 'SF3Editor.exe'. It supports all files the standalone editors and some new ones:
    - *.MPD: Map assets
    - X1*.BIN: Town/battle tables and programming
    - X002.BIN: Items, spells, weapon spells, "load" scenes, several misc. game stats, and Scenario 1 warp table
    - X005.BIN (new): Camera settings
    - X011.BIN: Icon pointers
    - X012.BIN (new): Scenario 1 terrain type info and some tables for AI targeting priority
    - X013.BIN: Several battle-specific stat tables
    - X014.BIN (new): Scenario 2+ battle scene info
    - X019.BIN: Monsters
    - X021.BIN: Icon pointers
    - X026.BIN: Icon pointers
    - X031.BIN: Character stats and initial info (copy 1/2)
    - X033.BIN: Character stats and initial info (copy 2/2)
    - X044.BIN: Monsters (PD only)
- New editor can open multiple files as tabs
- Scenario is now auto-detected based on folder or drive volume name.
    - (If the editor can't figure it out, you can set it explicitly in the 'File | Open Scenario' menu)
- Improved menus from standalone editors:
    - Added convenient/QoL items to the file menu:
        - Save All
        - Close All
        - Recent Files
    - New 'View' menu (currently only has options for the MPD viewer)
    - New 'Tools' menu (DFR and Table import/export tools have been migrated here from the 'File' menu)
    - New 'MPD' menu with various tools when editing an MPD file
    - New 'Settings' menu with more settings than before

### MPDs:
- Editor:
    - New Features:
        - Wireframes are now rendered without face culling so they're visible from behind
        - Collision lines can now be viewed in 3D (is has some quirks still, but it's very functional):
            - Collision lines that are disabled based on game flags have different coloration
        - Shadow models marked with the 2000 tag are now properly made semi-transparent
        - "Technical"(?) models marked with the 3000 tag are now properly hidden
        - Added keyboard shortcuts 'E' and 'N' for S(e)lect mode and (N)avigate mode
        - Trees can now be discarded and replaced onto tiles using the 'Has Tree' checkbox.
            - (Unchecking the checkbox moves a tree out-of-bounds and checking it attempts to pull one in. We can't *add* or *remove* tree models yet, all we can do it move them around.)
        - Textue importing/exporting now supports textures from all collections and uses hex IDs in filenames instead of decimal
        - Added new settings to control how surface map normals are calculated (includes some non-vanilla options and vanilla bugfixes)
        - Added rotating 3D model viewer to PDATAs[] and Models tables
        - Implemented 'MPD -> Model Switch Groups' menu for toggling models based on game flags
            - (This is used for cupboards, ruins state, the Elbesem statue, etc)
        - Added viewer option to reproduce in-game model hiding based on camera direction
        - Added viewer option to render on a black background instead of cyan
        - Removed the 'Recalculate Surface Map Normals' toolbar buttons; they're now in the 'MPD' menu, not taking up valuable real estate
    - Bugfixes:
        - Wireframes are now placed on top of models with much less clipping/artifacts
        - Tile normals weren't calculated in a consistent matter when updating tile heights
        - Current cursor is now reset to 'Select' when starting the app
        - Fixed some render order issues related to transparent models
        - Renamed some misleading property names with Model Switch Groups
        - More tweaks to lighting (still not perfect!)
    - EXPERIMENTAL: proof-of-concept tile editor tools. Worked with one specific (private) map, but should generally work with the PD file FIELD.MPD
- Files:
    - Walking collision lines have been figured out; added appropriate tables
    - "Movable" models (chests and barrels) are now supported (they're not placed in the map in the MPD file, but their assets are in there)
    - Added support for "OnlyVisibleFromDirection" property for models
    - Bugfix: Model angles and scales were incorrectly read in the "weird" CompressedFIXED format

### X1's:
- Added tables:
    - CharacterTargetPriority(s)
    - Unknown 16 tables after character target priority
- Regrouped and re-ordered 'Slots' submenus for better clarity and navigatibility
- More is known about the warp table:
    - Added a dropdown for the 'Load' index
    - Added game flag checked dropdown
- More is known about interactables:
    - Most of the parameters have been identified and associated with dropdowns and better descriptions
    - Implemented context-dependent dropdowns depending on parameters (it gets messy!)
    - Added human-readable description for interactable trigger and action (still WIP, mostly finished)
    - Added associated game flag with dropdown for expected on/off value
    - Updated MPD EventID referencing
- More is known about the 'Slots' table:
    - Added a 'Battle ID' field to show the enemy index referenced in game code
    - Added more enemy flag dropdowns. Not all flags are yet known, but it's getting close
    - The field for the 'game flag' is sometimes a 'Battle ID' based on context. The dropdown now reflects this
- More about the 'Scripted Movements' table is known; made corrections and improvements
- Added game flag dropdowns for 'Enter' and 'Arrow' tables (not actually used for arrows, but it's supported!)

### X005 (new editor):
- Added camera settings

### X012 (new editor):
- Added some tables specific to Scenario 1:
    - Class/terrain type movement and land effect table        - ClassTargetPriorityTable(s)
    - UnknownPriorityTable(s)

### X013:
- Single-element tables like "Heal Exp", "Soulmate Change Fail", etc. have been merged into one "Significant Values" table

### X014 (new editor):
- Added table for battle scenes based on MPD file (Scenario 2+ only)
    - Can set the 3D scene used, skybox image (how this is stored is not yet known), lighting, and fog type
- Added tables for terrain type-based battle scenes

### X019:
- Regrouped and re-ordered monster submenus for better clarity and navigatibility

### X031/X033:
- Regrouped and re-ordered character stat submenus for better clarity and navigatibility

### DFRToolGUI:
- Bugfix: When used in the SF3Editor, the window margin was missing

### X1\_Analyzer (new project):
- Similar to the MPD\_Analyzer, this can be used to search through X1 files for info

## 2025-02-23

I'm happy to announce a HUGE update: nearly all the work on rendering MPD files is complete! Models, ground planes, skyboxes, and gradients are now rendered properly,
along with extras like the Titan, the Dark Kraken, and tables that tweak lighting. Speaking of lighting, the lighting model is now much more accurate to what appears in-game.

Here's the small list of big features before the gigantic list of all changes and bugfixes:

- Rendering: Models, Ground/water planes, Skyboxes (only in-map in Scenario2+), Gradients (Scenario2+)
- 3D Viewer: Added buttons for select/navigation mode
- 3D Viewer: Added buttons to enable/disable more elements in the MPD
- 3D Viewer: Added buttons to set the camera angle
- Beta MPDs now load, as does SHIP2.MPD (sort of)
- Lots of bugfixes
- "MPD Analyzer" program now reports a lot more MPD errors, including a lot of quirks in the original MPDs

Enjoy!
-- Synival

Full changelog:

### MPD_Editor

- New Features / Improvements
    - Rendering
        - Models
            - Models are now loaded and rendered
            - Polygons now respect one- and two-sided modes
            - Sprite models (like trees) automatically rotate to face the camera
            - Semi-transparent polygons are rendered when they're indicated in the MPD (many things, like shadows in Balsamo, are not)
            - Polygons with either colors or textures are rendered
            - Added support for Chunk19 as a model, which is used by the Titan in Z_AS.MPD
            - Transparency for Palette3 textures is applied (indicated by the "Light Adjustment Table")
        - Ground / Water
            - Both types of repeating ground/water chunks are rendered: single image (512x256) and tiled (2048x2048)
            - Lighting is adjusted based on the "Light Adjustment Table" if available (this data is always present, but only actually used in-game in a few places)
            - Tiled ground planes position themselves as close to the camera center as possible. This fixes MPDs like BEER.MPD (Baersol) whose offset is a full screen away for some reason
            - Tiled ground planes, which repeat in-game, render 50% extra to show wraps that are normally visible in-game
        - Skyboxes
            - Skyboxes are now loaded and rendered in Scenario 2 or later (they only render in battle cutscenes in Scenario 1)
        - Backgrounds
            - Background images, like Ishahakat's background/foreground images, are now loaded
        - Gradients
            - Gradient effects in Scenario 2 and later are now loaded and rendered
        - Lighting
            - Applied much more accurate mixing formula for lighting
            - Slightly more accurate but still not quite there Scn2 lighting (eventually should just use a lookup table)
        - Other
            - Increased render distance
            - Non-planar quads rendering is improved by a lazy and not-accurate-at-all fix that adds a center vertex, splitting them into 4 triangles
            - Removed a lot of seams when rendering surface models
            - Polygons with alpha channels no longer have ugly visual artifacts on their edges when rendered from a distance
    - Editors / Viewers
        - 3D Viewer / Editor
            - Added toolbar buttons for "select" vs "navigate" mode
            - Added toolbar buttons for toggling display of various elements and in-game features (lighting, animations)
            - Added toolbar buttons for camera control
            - Added toolbar button to auto-rotate sprites up to face the camera. This doesn't happen in-game, but is very convenient for top-down views
        - Tile Editor
            - Changing tile terrain type by typing or pasting text now works a lot better
            - Tiles are now associated with nearby tree models (can't yet toggle to add/remove trees for forest terrain type)
            - Renamed recently-identified terrain type "UnknownA" to "CantStay"
            - Renamed recently-identified terrain type "UnknownD" to "PlayerOnly"
        - Table Editor
            - Added new editor for single-row "tables" like the main header table. New editor has properties in rows rather one giant row with way too many columns
            - Added bare-bones hex viewer for unknown/unhandled chunks
            - Added columns "ChunkFileAddress", "ChunkType", and "CompressionType" to chunk table
            - Added a lot of columns for individual flags in main header table
    - Data
        - Textures
            - Added texture chunks for movable objects (chests and barrels)
            - Indexed textures are now supported and detected whenever possible
            - Added Scenario 3 "Indexed Textures" table that indicates which textures use Palette3
            - Texture animations support indexed textures (indicated with a 0x100 flag in the texture ID)
            - Added support for texture chunk Chunk[21] (used for the Dark Kraken and one other random thing)
        - Tables
            - Added several tables to model chunks (needs some corrections, but completely editable)
            - Added "Gradients" table (Scenario 2+)
            - Added "Light Adjustment" table (Scenario 2+)
            - Added "TextureAnimationsAlt" table
            - Added "ModelSwitchGroups" table (very incomplete)
            - Added Scenario1 "Unknown2" table (research suggests this is completely unused, even in the one MPD where this exists)
        - Chunks
            - Chunk loading now respects flags indicated in the header
        - Other
            - Reverse-engineered a lot (but not yet all) of the MPD header flags and added columns for them
    - Other Improvements
        - Added support and detection for SHIP2.MPD
        - Added support and detection for beta MPDs
        - Added some integrity checks for chunk recompression and chunk table rebuilding

- Fixes
    - Rendering
        - Surface Models
            - Fixed some ugliness with wireframe rendering
            - Fixed a lot of Z-fighting issues
        - Lighting
            - When correcting surface normal in-game bugs, the vectors are now normalized
            - Corrected a texture palette UV wrapping bug when lighting was directly overhead
        - Texture Atlases
            - Texture atlases no longer fail to generate because the minimum size was too small
            - Texture atlases no longer have distorted rectangular textures
            - Texture atlases no longer cut-off textures that are transparent on the edges, causing distortion for the entire chunk
        - Other
            - Fragment shaders now discard transparent fragments, fixing some visual errors and improving performance
    - Editors / Viewers
        - 3D Viewer / Editor
            - More accurate texture animation speeds and consistent camera movement (internal fixes to frame delta)
        - Tile Editor
            - Changing current tile using Ctrl+Direction now takes the current camera angle into account
        - Table Editor
            - Removed Palette3 from ChunkViews in Scenario 1
            - Fixed ObjectListView combobox crashing when auto-expand for boolean values
            - Added some minimum widths missing in the main header table
    - Data
        - Textures
            - Fixes for MPD files with missing texture chunks
        - Tables
            - Palettes with invalid data are now detected and ignored
            - Renamed "Light Direction" to more accurate "Light Position"
            - Fixes to size of Scenario1 "Unknown1" table (research suggests this is completely unused)
            - Renamed "OffsetScrollScreenTable" to "Ground Animation Table"
        - Chunks
            - Fixes so some MPDs with corrupt or missing data will load
            - Fixed an issue where resizing a final empty chunk would try to resize non-existent data; fixed an issue when looking for ScenarioType.Demo resources
            - Rebuilding an out-of-order chunk table *should* work now
            - Fixes to rebuilding the chunk table when the first few empty chunks aren't in the correct position
        - Technical / Internal
            - Fixed Length values of 0 in event arguments when using ExpandOrContract and contracting

- Code Stuff
    - Rendering
        - Overhauled OpenGL and texture coordinates to be consistently counter-clockwise, matching order in the model chunk; added face-culling; removed now-redundant twoSided shader attribute
        - More pixel conversion functions (but not more tests...)
        - Texture ARGB1555/8888 and Hash data is now lazily cached
        - Replaced AssumedPixelFormat with actual PixelFormat; known PixelFormat's can now be supplied to the TextureCollection
        - Expanded Texture.CreateBitmap into ARGB1555 and ARGB8888 variants
    - Interface
        - Some simplifications and enhancements to TextureTableView, TextureView
    - Tables
        - TableView table can now be changed after Create()
        - Lots of refactoring for ITable's: introduced IBaseTable and different types of tables (TerminatedTable, FixedSizeTable, ResourceTable)
        - ITable changes: renamed Reset() to Unload(), introduced indexer, is now an IEnumerable<T>, has Length
        - Added SizeInBytes to ITable
    - Data
        - WIP patch for ByteArraySegment to not fail when adding an uncompressed chunk of size 0 that occurs at the same location of another
        - NameGetterContext now works with values with types other than int
        - Changed some offsets from 'int' to 'uint' for easier debugging
        - Added more unit tests for ByteArray and ByteArraySegment
        - Minor clean-ups of ByteArraySegment.OnParentRangeModifiedReal()
        - Big updates to MPDHeaderModel based on Marf's notes
        - Refactored TextureAnimationModel to have a sub-table for its frames
        - Removed some tables that only have a single element (e.g. MPDHeaderTable)
        - Migrated some column calculations to TableViewModelColumn
    - Utility functions:
        - Added column-major order version of To2DArray()
        - Added MathHelpers.Clamp()
        - Expanded CornerType extensions, added a ton of constants
    - Other:
        - Fixed incorrect namespace for several MPD structs
        - FIXED and CompressedFIXED values can now be modified
        - Renamed TextureView to ImageView

### MPD_Analyzer

- New Features / Improvements
    - Added more random checks to MPD_Analyzer
    - Added more info in MPD_Analyzer file line
    - Updated MPD_Analyzer to check for some model and surface model chunk errors
    - Rewrote analysis for chunks 14 through 19
    - Added list of unused MPDs for Scenarios 1, 2

- Fixes
    - Fixes and upgrades to the MPD_Analyzer

- Internal / Code Stuff
    - Lots of refactoring and additional reports
    - Some (very messy) code to match MPDs based on specific conditions, which reports header flags and chunks

### X019 Editor

- Fixes
    - X019 Editor
        - Some fixes for Monster names in S2, S3, PD
        - More S2 monster name fixes
        - More monster name corrections
        - Updated Monsters.xml using the latest translations

## 2025-01-19

This bug update introduces an MPD Editor! MPD files contain 3D map geometry, tiles, textures, terrain types, and much more. In this version, you can view and modify all the tiles of a map, modify their textures, tweak lighting (not yet shown visually), and change camera and battle boundaries.

### MPD Editor

- Compression/decompression code for MPD chunks by Agrathejagged (https://github.com/Agrathejagged)
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

### Sample Projects

- Grayscaler: Opens all MPD files, converts their textures to grayscale, and outputs them to a destination folder.
- LightPaletteSetter: Opens all MPD files, applies a specified light palette and direction, and outputs them to a destination folder.
- MPD\_Analyzer: Opens all MPD files across all discs and checks for abnormalities.
- TextureExtractor: Opens all MPD files and dumps all unique textures to a folder. Textures are identified by MD5 hash.

### All Editors

- Read-only values are now greyed-out
- Clicking "named values" with combo boxes now automatically expand the combo box (saved you a click!)
- Fixed several bugs with editing integer values of different sizes and signedness

### Big Code Changes

- Updated all frontend projects to .NET 8.0. Backend projects are still .NET Standard 2.0
- New editor code, currently only used in MPD Editor and IconPointerEditor:
    - Tables for data (ObjectListViews) are now automatically created using property annotations
    - Editors are now components (Views) rather than 'Form's, so multiple editors of different types can be loaded simultaneously
- Refactored a lot of core stuff for opening and modifying binary files
- Lots of restructuring of structs, tables, and "files" in SF3Lib
- Renamed SF3Editor to SF3LibWin -- it's now a library for anything specific to the Windows frontend
- Migrated a lot of code from individual editor projects to SF3LibWin
- Introduced 'ByteArray'. This is a wrapper for byte[] that can be renamed and listened-to for "modified" events
- Introduced 'ByteArraySegment'. This represents a small window of byte[]'s inside a 'ByteArray' that can be resized dynamically, affecting the parent and other neighboring 'ByteArraySegment's.
- Updated 'ByteData's to use new 'IByteArray' instead of byte[].

## 2024-11-10

A few nice extra features of quality-of-life changes that deserve to be released
sooner rather than later. Enjoy :)

### DFRTool

- 'apply' command now requires an output file by default
- The input file can still be patched with the '-i' command line option

### DFRToolGUI

- The GUI DFRTool has been upgraded to have an 'Apply DFR ' mode as well. This haves
  identically to the command line version.

### All Editors

- Changed all hex values to decimal if the value was player-facing or otherwise made
  sense to be a decimal (e.g, weapon level or friendship experience)
- Hex values are now displayed with a fixed-width font and at least 2 digits to
  better differentiate themselves from decimal values
- Larger hex values like addresses should now have at least 4 digits shown
- Fixed signedness: hopefully all values that can be negative are shown as such now
- Applied consistent capitalization to all lists of data (e.g, monsters, spells, items)
- Fixed many misspellings, typos and inconsistent names in lists of data
- Expanded columns that wouldn't fit their names and displayed "..." (this may still
  happen depending on your OS or settings)
- Renamed "Generate DFR File" to "Create DFR File" for consistency
- Applying a DFR file now pops open the "DFRTool" in "Apply" mode, similar to the
  "Create DFR File" option
- Changed DFR hotkeys for "Apply" and "Create" to Ctrl+A and Ctrl+C respectively

### X013 Editor

- Added "SpecialEffects" table for Scenario 3 + Premium Disk to modify the table of
    status effects for specials
- Added 10 "Unknown" byte values to the debuff%-by-luck table

## 2024-11-08

This version marks a major milestone in development. All the table code has been
rewritten so it's much more flexible and easier to work with. This opens the door
to a lot of really cool stuff, like auto-detection of files, a much improved
X1 editor, and some other really big things down the road. Stay tuned!! :)

Each editor now shares the same underlying menu and core functions, like the "Copy
Tables" commands. There's also support for creating DFR patches and applying them
to the current open file. And TONS of quality of life stuff.

Enjoy!

### DFRTool

- Added 'apply' command to apply DFR patch to an existing .BIN file. Run 'dfrtool --help' to see usage
- Existing behavior is now 'create' command . Run 'dfrtool --help' to see usage
- Added '--version' option

### DFRToolGUI

- Intentionally downgraded from .NET 8.0 to the much older .NET Framework 4.8 so it's compatible with the other editors

### All Editors

- Added "File -> Apply DFR File..." to all editors
- Added "File -> Generate DFR File..." to all editors
- Added "File -> Copy Tables To..." to all editors
- For all editors that didn't have it, added "File -> Copy Tables From..."
- Added option "Edit -> Use Dropdowns for Named Values" (on by default)
    - Disabling this will allow you to edit values directly for fields that normally have dropdowns
- Added basic "File -> Save" command to save the current open file
- Improved some error handling
- Tweaked some hotkeys: editing premium disk is now "Ctrl-4" instead of "Ctrl-P"
- Fixed the value range for int editors: it didn't work with pointers, like in the BattlePointerTable

### IconPointerEditor

- X044 files are now auto-detected; the flag for selecting them explicitly has been removed

### X002 Editor

- StatUp values for items now have the appropriate dropdowns if their type is special or spell
- Renamed "Preset" table to "WeaponSpell"

### X013 Editor

- Added dropdowns for SupportA and SupportB values in "SupportType" table
- Corrected incorrect address of Scn2 SoulFailTable (it was pointing to code, whoops!)
- Scenario 1 magic bonus table now uses 32-bit ints instead of 8-bit ints
- Magic bonus table now shows values properly as signed (can be negative) integers

### X019 Editor

- Added dropdown for item drop rate values
- Added true/false dropdown for "Can't See Status" flag

### X033/X031 Editor

- Added named values with dropdowns for:
    - Characters' Weapons equippable
    - Characters' Accessories equippable
- Hovering the mouse over lines in the "Stat Growth Chart" now shows the probabilities of each stat value for each level

### X1 Editor

- The X1 editor now opens all maps at once instead of only the selected map:
    - The "Map" dropdown has now been removed. All maps are now loaded
    - Tables for each battle have been migrated to sub-tabs under these tabs:
        - Battle (Synbios)
        - Battle (Medion)
        - Battle (Julian)
        - Battle (Extra)
    - Battle tabs for battles that don't exist (i.e, their pointers in the BattlePointersTable are zero) are automatically hidden
    - The auto-generated "Map" column in the "Header" table is now gone (it's no longer relevant)
- Events in the "Interactables" table now have a dropdown for items when the EventType is 0x100 or 0x101

### Code clean-ups

- Each editor now has a core menu that's shared between them
- Created CommonLib and CommonLibWin projects for shared code that isn't domain-specific (e.g. SF3, DFR)
- Massive auto-linting / refactoring from VS analysis tools
- Renamed "ModelArray" and "\*List" classes to "Table" classes
- Reorganized Models/Tables into individual "Models" and "Tables" folders, rather than sharing a namespace for each model
- Added unit tests to ensure that all files for all supported discs are working as expected
- Merged S1 and S2+3+PD WarpTables into a single model/table
- Rewrote all table loading code to about 5% their original size
- HUGE architecture changes for Models, Tables, and FileEditors:
    - All code to determine table/model address have been migrated out from their Models and merged into the MakeTables() routines of their corresponding editors.
    - It's now the responsibility of the FileEditor to determine table address,
    - and the responsibility of the table to determine model addresses.
- Extracted smaller interface IByteEditor from IFileEditor to allow working with in-memory data
- Completely rewrote how "NamedValues" work. Instead of being their own object, they're now property tabs that get their name from a shared INameGetterContext.
- Upgrades to "Copy Tables" bulk copy features:
    - Now supports copying dictionaries
    - Logging is improved:
        - Property names are now shown when available
        - Fixed some bugs regarding really badly-formatted logs for collections
        - (debug only) Collection keys and underlying types are now shown

## 2024-10-26

This is the first version of the DFRTool command-line tool and GUI tool (DFRToolGUI)!
These can be used to generate .DFR files using an original file and an altered file.
They require the .NET 8.0 runtime to use.

### DFRTool
- Run `dfrtool [-c] original-file altered-file > output.DFR` to get your DFR file.
- (use `dfrtool --help` for more info)

### DFRToolGUI
- Can do everything the DFRTool can do, but with a GUI frontend.

### All Editors
- Added keyboard shortcuts to menus and did some re-organizing to follow common practices:
    - "Help" menu moved to the right side
    - Open/Save/Close are Ctrl+O, Ctrl+Alt+S, Ctrl+W
    - Scenarios can be quickly selected with Ctrl+1, Ctrl+2, Ctrl+3, Ctrl+P
- All .XML resource files have been migrated into one place so they can be shared between editors, with the following file structure:
    - Resources/ (contains .XML files that aren't scenario-specific)
    - Resources/S1 (Scenario 1-specific .XML files)
    - Resources/S2 (Scenario 2-specific .XML files)
    - Resources/S3 (Scenario 3-specific .XML files)
    - Resources/PD (Premium Disk-specific .XML files)
- Editing dropdowns with unhandled values (like 0xFFFD for 'EnemyID' in Sc3 X1BTL328.BIN, Julian Map) no longer crashes
- Fixed a bug where closing an edited file wouldn't clean up its data completely, which could cause strange behavior in the next file edited

### IconPointerEditor
- Added "Spell Name" column with text to reflect the actual spell referenced in addition to the icon name

### X002 Editor
- Added more dropdowns with value names for columns:
    - Items:
        - Effective Type 1, Effective Type 2
    - Spells:
        - Element
        - Spell Target
    - Loaded:
        - Added dropdowns for file indexes (X1, X5, MPD, CHR)
    - LoadOverride:
        - Added dropdowns for file indexes (synMPD, medMPD, julMPD, extraMPD, synCHR, medCHR, julCHR, extraCHR)

### X019 Editor
- Added more dropdowns with value names for columns:
    - Item dropped
    - Movement type

### X033/X031 Editor
- Added more dropdowns with value names for columns:
    - Movement type

### X1 Editor
- Fix to crash when loading bad X1 files.
- Added dropdown 'IsBoss' before to 'FacingIsBoss' to convenient view/modify its boss bit

### Code clean-ups
- Moved all code not related to the editor frontends into SF3Lib project. Now, everything can be shared amonst all tools.
- Mass-applied code formatting script with .editorconfig for terse-style code
- Began refactoring all the 'ModelLists' to use simplified, shared code
- Added automated testing for IconPointerEditor to ensure it works across all scenarios and X026.BIN files
- Made it easy to manage the "NamedValues" for new resources
- More refactoring (it's almost done!)

## 2024-10-19

### All Editors
- Editor titles have been updated to show the file you're currently editing
- Every editor now tracks if the file is modified
- If you have unchanged changes, all editors will now ask if you want to save
- Added 'File -> Close' menu item for all editors
- Added 'File -> Exit' menu item for all editors
- Added a names for values in cells for:
    - Spells
    - Items
    - Monsters
    - Specials
    - Item Stat Types
- Fixes to some incorrect spell and items names:
    - Scn1+2 MultipleTargetLaser -> BreathOfWind
    - Misty Rapier and Magic Rapier were sometimes swapped
- Adjusted some columns to fit some text better

### IconPointerEditor
- Added 'File -> Copy Tables From...' to copy table between X011, X021, and X026 files
- Updated 'File -> Open...' and 'File -> Save As...' dialogs to show all files editable without changing the dropdown
- Added a column for "Spell Name" to differentiate between the spell represented by the icon (which has a lot of STHA holdovers) and the actual spell referenced.

### X002 Editor
- Fixes for compatability with Scenario 1 original JP version
- Tab for warp tables are now only present when editing Scenario 1 files (and X1BTL99.BIN)

### X013 Editor
- Fixes for compatability with Scenario 1 original JP version

### X019 Editor
- Premium Disk X044.BIN should now load properly again
- Updated 'File -> Open...' and 'File -> Save As...' dialogs to show all files editable without changing the dropdown

### X033/X031 Editor
- Added 'File -> Copy Tables From...' to copy table between X033 and X031 files
- Updated 'File -> Open...' and 'File -> Save As...' dialogs to show all files editable without changing the dropdown

### X1 Editor
- Tabs for data irrelevant to the file being edited are now hidden

### Code clean-ups
- Updated some leftover unnamed/misnamed stuff
- Fixed mismatches between variable names for S\*LearnedLevel and S\*LearnedID
- Started sharing .XML resources between editors
- Created base 'EditorForm' from which all editors have derived code (less copying!)
- Migrated ModelLists out of forms and into the \*\_FileEditor classes
- Removed lots of dead code

---

## 2024-10-11

### All Editors
- All togglable/selectable menu items now have checkboxes
- Several editors no longer crash when loading an incomptable / corrupt file
- More helpful error messages when opening a file fails
- Fixed inconsistent positioning / sizes of tables across all editors and tabs

### IconPointerEditor

### X002 Editor
- Item flags now have individual dropdowns:
    - isCursed
    - canCrack
    - healingItem
    - Can't Unequip
    - Rare
    - FakeRare
    - healingItem2
    - requiresPromo
    - requiredPromo2
    - hero only
    - male
    - female
- Weapon types can now be edited with a dropdown list with names

### X013 Editor

### X019 Editor

### X033/X031 Editor
- Character classes can now be edited with a dropdown list with names
- Weapon types can now be edited with a dropdown list with names
- Sexes can now be edited with a dropdown list with names
- Added "Curve Graph" tab to view stat growth curves with ranges of likely values
- Minor tweaks to stat growth calculations

### X1 Editor

### Code clean-ups
- Upgraded to .NET Framework 4.8
- Merged code for all editors into one single solution with shared resources
- Made FileEditor non-static to allow multiple files open at once in the future
- Replaced old names from the STHA Editor with proper names for the SF3 Editor
- Corrected unset or incorrect names, such as tabPageN, olvColumnN, etc.
- Lots of refactoring to simplify codebase
- Removed a lot of dead code
- Lots and lots of prettifying :)

