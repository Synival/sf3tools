## 2025-01-13

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

