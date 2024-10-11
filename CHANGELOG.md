## 2024-10-11

### All Editors
- All togglable/selectable menu items now have checkboxes
- Several editors no longer crash when loading an incomptable / corrupt file
- More helpful error messages when opening a file fails
- Fixed inconsistent positioning / sizes of tables across all editors and tabs

### Code clean-ups
- Upgraded to .NET Framework 4.8
- Merged code for all editors into one single solution with shared resources
- Made FileEditor non-static to allow multiple files open at once in the future
- Replaced old names from the STHA Editor with proper names for the SF3 Editor
- Corrected unset or incorrect names, such as tabPageN, olvColumnN, etc.
- Lots of refactoring to simplify codebase
- Removed a lot of dead code
- Lots and lots of prettifying :)

### IconPointerEditor
-

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
-

### X019 Editor
-

### X033/X031 Editor
- Character classes can now be edited with a dropdown list with names
- Weapon types can now be edited with a dropdown list with names
- Sexes can now be edited with a dropdown list with names
- Added "Curve Graph" tab to view stat growth curves with ranges of likely values
- Minor tweaks to stat growth calculations

### X1 Editor
-

