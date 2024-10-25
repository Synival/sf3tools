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

