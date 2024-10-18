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
- Fixes to some incorrect spell and items names:
    - Scn1+2 MultipleTargetLaser -> BreathOfWind
    - Misty Rapier and Magic Rapier were sometimes swapped
- Adjusted some columns to fit some text better

### IconPointerEditor
- Added 'File -> Copy Tables From...' to copy table between X011, X021, and X026 files
- Updated 'File -> Open...' and 'File -> Save As...' dialogs to show all files editable without changing the dropdown

### X002 Editor
- Tab for warp tables are now only present when editing Scenario 1 files (and X1BTL99.BIN)

### X013 Editor

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

