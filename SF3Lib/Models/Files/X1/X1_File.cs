using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.Shared;
using SF3.Models.Tables.X1;
using SF3.Models.Tables.X1.Battle;
using SF3.Models.Tables.X1.Town;
using SF3.Types;
using static CommonLib.Utils.ResourceUtils;
using SF3.Actors;
using SF3.Models.Structs.Shared;
using SF3.Utils;
using CommonLib.Utils;
using SF3.Models.Structs.X1;
using CommonLib.Discovery;

namespace SF3.Models.Files.X1 {
    public class X1_File : ScenarioTableFile, IX1_File {
        protected X1_File(IByteData data, INameGetterContext nameContext, ScenarioType scenario, bool isBTL99) : base(data, nameContext, scenario) {
            IsBTL99 = isBTL99;
            RamAddress = IsBTL99 ? 0x06060000u : Scenario == ScenarioType.Scenario1 ? 0x0605f000u : 0x0605e000u;
        }

        public static X1_File Create(IByteData data, INameGetterContext nameContext, ScenarioType scenario, bool isBTL99) {
            var newFile = new X1_File(data, nameContext, scenario, isBTL99);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize tables");
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            // TODO: this does soooo much work! Let's try to break it up into subroutines.
            var isScn1OrBTL99 = Scenario == ScenarioType.Scenario1 || IsBTL99;
            bool hasLargeEnemyTable;

            int treasureAddress;
            int warpAddress;
            int battlePointersPointerAddress; // the address to the pointer to the table of battle pointers
            int npcAddress;
            int enterAddress;
            int arrowAddress;

            int battlePointersAddress;
            int tileMovementAddress;
            int characterTargetPriorityTablesAddresses;

            var battlePointersPointerPointerAddress = isScn1OrBTL99 ? 0x0018 : 0x0024;

            battlePointersPointerAddress = Data.GetDouble(battlePointersPointerPointerAddress) - (int) RamAddress;
            battlePointersAddress = Data.GetDouble(battlePointersPointerAddress);

            // A value higher means a pointer is on the address, meaning we are in a battle. If it is not a
            // pointer we are at our destination so we know a town is loaded.
            if (Scenario == ScenarioType.Scenario1 && battlePointersAddress > 0x0605F000 || battlePointersAddress > 0x0605e000) {
                battlePointersAddress -= (int) RamAddress;
                IsBattle = true;
            }
            else {
                battlePointersAddress = -1;
                IsBattle = false;
            }

            // The "Treasure" table is the only table present in all X1 files regardless of scenario or town/battle status.
            treasureAddress = Data.GetDouble(0x000c) - (int) RamAddress;

            if (isScn1OrBTL99) {
                hasLargeEnemyTable = true;

                warpAddress          = -1; // X002 file has Scenario1 WarpTable, and provides the address itself.
                npcAddress           = IsBattle == true ? -1 : battlePointersPointerAddress; // same address
                enterAddress         = Data.GetDouble(0x0024) - (int) RamAddress;
                arrowAddress         = -1; // Not present in Scenario1
            }
            else {
                hasLargeEnemyTable = false;

                warpAddress          = Data.GetDouble(0x0018) - (int) RamAddress;
                npcAddress           = IsBattle == true ? -1 : battlePointersPointerAddress; // same address
                enterAddress         = IsBattle == true ? -1 : Data.GetDouble(0x0030) - (int) RamAddress;
                arrowAddress         = IsBattle == true ? -1 : Data.GetDouble(0x0060) - (int) RamAddress;
            }

            // If this is a battle, we need to get the addresses for a lot of battle-specific stuff.
            if (IsBattle == true) {
                // Load the BattlePointersTable early so we can use it to determine the addresses of other tables.
                BattlePointersTable = BattlePointersTable.Create(Data, "BattlePointers", ResourceFile("BattlePointersList.xml"), battlePointersAddress);

                // Get the address of the selected battle, or, if it's not available, the first available in the BattlePointersTable.
                Battles = new Dictionary<MapLeaderType, Battle>();
                Battle lastBattle = null;
                foreach (var mapLeader in (MapLeaderType[]) Enum.GetValues(typeof(MapLeaderType))) {
                    var mapIndex = (int) mapLeader;
                    var battleTableAddress = BattlePointersTable[mapIndex].Pointer;
                    if (battleTableAddress != 0) {
                        lastBattle = Battle.Create(Data, NameGetterContext, mapLeader, battleTableAddress - (int) RamAddress, hasLargeEnemyTable, Scenario, lastBattle);
                        Battles.Add(mapLeader, lastBattle);
                    }
                }

                // Determine the location of the TileMovementTable, which isn't so straight-forward.
                // This table is not present in Scenario 1.
                if (!isScn1OrBTL99) {
                    // First, look inside a function for its address.
                    // The value we want is 0xac bytes later always (except for X1BTL330-339 and X1BTLP05)
                    var tileMovementAddressPointer = Data.GetDouble(0x000001c4) - (int) RamAddress + 0x00ac;

                    var priorityTablesOffset =
                        (Scenario == ScenarioType.Scenario2) ? 0x78 :
                        (Scenario == ScenarioType.Scenario3) ? 0x5c :
                                  /*ScenarioType.PremiumDisk*/ 0x5c;

                    var funcAddr = Data.GetDouble(0x01DC) - (int) RamAddress;
                    characterTargetPriorityTablesAddresses = Data.GetDouble(funcAddr + priorityTablesOffset) - (int) RamAddress;

                    // No problems with this method in Scenario 2.
                    if (Scenario == ScenarioType.Scenario2)
                        tileMovementAddress = Data.GetDouble(tileMovementAddressPointer) - (int) RamAddress;
                    else {
                        tileMovementAddress = Data.GetDouble(tileMovementAddressPointer);

                        // Is this a valid pointer to memory?
                        if (tileMovementAddress < 0x06070000 && tileMovementAddress > 0)
                            tileMovementAddress -= (int) RamAddress;
                        // If not, emply the workaround for X1BTL330-339 and X1BTLP05 not being consistant with everything else
                        // and locate the table directly.
                        // TODO: does this pointer exist in other X1BTL* files?
                        else
                            tileMovementAddress = Data.GetDouble(0x0024) - (int) RamAddress + 0x14;
                    }
                }
                else {
                    tileMovementAddress = -1;
                    characterTargetPriorityTablesAddresses = -1;
                }
            }
            else {
                // No battle, so none of these tables exist.
                battlePointersAddress = -1;
                Battles = null;
                tileMovementAddress = -1;
                characterTargetPriorityTablesAddresses = -1;
            }

            // Add tables present outside of the battle tables.
            var tables = new List<ITable>();
            if (warpAddress >= 0)
                tables.Add(WarpTable = WarpTable.Create(Data, "Warps", warpAddress, IsBattle, NameGetterContext));
            if (battlePointersAddress >= 0)
                tables.Add(BattlePointersTable);
            if (npcAddress >= 0)
                tables.Add(NpcTable = NpcTable.Create(Data, "NPCs", npcAddress, null));
            if (treasureAddress >= 0)
                tables.Add(InteractableTable = InteractableTable.Create(Data, "Interactables", treasureAddress, NameGetterContext, NpcTable));
            if (enterAddress >= 0)
                tables.Add(EnterTable = EnterTable.Create(Data, "Entrances", enterAddress));
            if (arrowAddress >= 0)
                tables.Add(ArrowTable = ArrowTable.Create(Data, "Arrows", arrowAddress));

            if (characterTargetPriorityTablesAddresses >= 0) {
                CharacterTargetPriorityTables = new CharacterTargetPriorityTable[16];
                int tablePointerAddr = characterTargetPriorityTablesAddresses;
                for (int i = 0; i < 16; i++) {
                    var tableAddr = Data.GetDouble(tablePointerAddr) - (int) RamAddress;
                    var tableName = "CharacterTargetPriorities 0x" + i.ToString("X") + ": " + NameGetterContext.GetName(null, null, i, new object[] { NamedValueType.MovementType });
                    tables.Add(CharacterTargetPriorityTables[i] = CharacterTargetPriorityTable.Create(Data, tableName, tableAddr));
                    tablePointerAddr += 0x04;
                }

                CharacterTargetUnknownTables = new CharacterTargetUnknownTable[16];
                tablePointerAddr = characterTargetPriorityTablesAddresses + 0x140;
                for (int i = 0; i < 16; i++) {
                    var tableAddr = Data.GetDouble(tablePointerAddr) - (int) RamAddress;
                    var tableName = "UnknownTableAfterTargetPriorities 0x" + i.ToString("X") + ": " + NameGetterContext.GetName(null, null, i, new object[] { NamedValueType.MovementType });
                    tables.Add(CharacterTargetUnknownTables[i] = CharacterTargetUnknownTable.Create(Data, tableName, tableAddr));
                    tablePointerAddr += 0x04;
                }
            }

            // Add tables for battle tables.
            if (Battles != null) {
                var battles = Battles.Select(x => x.Value).Where(x => x != null).ToList();
                tables.AddRange(battles.SelectMany(x => x.Tables));
            }

            if (tileMovementAddress >= 0)
                tables.Add(TileMovementTable = TileMovementTable.Create(Data, "TileMovement", tileMovementAddress, true));

            // Locate difficult-to-find common functions/data that are shared between X1 files.
            var searchData = Data.GetDataCopy();
            Discoveries = new DiscoveryContext(searchData, RamAddress);
            Discoveries.DiscoverUnknownPointersToValueRange(RamAddress, (uint) (RamAddress + Data.Length - 3));
            DiscoverFunctions(searchData);
            DiscoverData(searchData);

            // Now that we've discovered some data, let's populate some tables.
            tables.AddRange(PopulateMapUpdateFuncTables());
            tables.AddRange(PopulateModelInstanceTables());
            PopulateScripts();

            // Add references to the scripts for several tables so we can have nice dropdowns.
            AssociateScriptsWithRelevantTables();

            return tables;
        }

        private void DiscoverFunctions(byte[] data) {
            // Look for known functions and create corresponding DiscoveredData() entries.
            var funcs = KnownX1Functions.AllKnownFunctions
                .Select(x => new { Info = x.Key, Size = x.Value.Length * 2, Indices = data.IndicesOfSubset(x.Value.ToByteArray()) })
                .ToArray();

            foreach (var func in funcs) {
                for (int i = 0; i < func.Indices.Length; i++)
                    _ = Discoveries.AddFunction((uint) func.Indices[i] + RamAddress, func.Info.TypeName, (i == 0 ? "" : $"DUP{i}_") + func.Info.Name, func.Size);
            }

            // Add functions in the interactable table.
            if (InteractableTable != null) {
                var iFuncs = InteractableTable
                    .Where(x => x.Action >= RamAddress && x.Action < RamAddress + Data.Length - 3)
                    .GroupBy(x => x.Action)
                    .ToDictionary(x => x.Key, x => x.ToArray());

                var ngc = NameGetterContext;
                foreach (var kv in iFuncs) {
                    var funcPtr = kv.Key;
                    if (!Discoveries.HasDiscoveryAt(funcPtr)) {
                        var ids = string.Join("_", kv.Value.Select(x => ngc.GetName(x, null, x.TriggerType, new object[] { NamedValueType.EventTriggerType }) + x.ID.ToString("X2")));
                        Discoveries.AddFunction(funcPtr, "InteractableFunction", $"interactableFuncFor{ids}()", null);
                    }
                }
            }
        }

        private uint? GetSetRenderThinkFuncsAddr() {
            switch (Scenario) {
                // Function at +0x24 in X006.BIN
                case ScenarioType.Scenario1:   return 0x06046024;
                // TODO: not so in v1!!! 
                case ScenarioType.Scenario2:   return 0x06044824;
                case ScenarioType.Scenario3:   return 0x06043D24;
                case ScenarioType.PremiumDisk: return 0x06043D24;
                default: return null;
            }
        }

        private uint? GetSetGroundPlanePositionViaJump() {
            switch (Scenario) {
                case ScenarioType.Scenario1:   return 0x06046114;
                case ScenarioType.Scenario2:   return 0x06044920;
                case ScenarioType.Scenario3:   return 0x06043E44;
                case ScenarioType.PremiumDisk: return 0x06043E44; // assumed
                default: return null;
            }
        }

        private void DiscoverData(byte[] data) {
            // Look for references to that function. There are many variants of this function, so look for all of them.
            var instantiateModelsFuncs = Discoveries.GetFunctions()
                .Where(x => x.TypeName == "InstantiateModelsFunc")
                .ToArray();

            // On the off chance that multiple versions of this function exist (which never seems to be the case),
            // look for usages of all of them.
            int instanceGroupId = 0;
            foreach (var func in instantiateModelsFuncs) {
                // Get all pointers to this function.
                var funcPtrs = Discoveries.GetPointersByValue(func.Address);

                // For each function pointer, look at the pointer before it. This should be the parameter loaded in,
                // which is a pointer to a table we want to load.
                foreach (var funcPtr in funcPtrs) {
                    // Get the address of the previous pointer...
                    var modelsPtrRamAddr = funcPtr.Address - 4;
                    var modelsPtrAddr = modelsPtrRamAddr - RamAddress;

                    // ...and the pointer itself. If it looks like a pointer, we're in business.
                    var modelsRamAddr = data.GetUInt((int) modelsPtrAddr);
                    if (modelsRamAddr >= RamAddress && modelsRamAddr < RamAddress + data.Length)
                        Discoveries.AddArray(modelsRamAddr, nameof(ModelInstanceGroup) + "[]", $"modelInstanceGroup{instanceGroupId++:D2}", null);
                }
            }

            // Look for references to some handy functions.
            var setRenderThinkFuncsAddr = GetSetRenderThinkFuncsAddr();
            if (setRenderThinkFuncsAddr.HasValue)
                Discoveries.AddFunction(setRenderThinkFuncsAddr.Value, "Function", $"assignMapUpdateFuncsViaJump({nameof(MapUpdateFunc)}[]* funcs)", null);

            var setGroundPlanePositionAddr = GetSetGroundPlanePositionViaJump();
            if (setGroundPlanePositionAddr.HasValue)
                Discoveries.AddFunction(setGroundPlanePositionAddr.Value, "Function", "setGroundPlanePositionViaJump(int x, int y, int z)", null);

            if (setRenderThinkFuncsAddr.HasValue) {
                var pointers = Discoveries.GetPointersByValue(setRenderThinkFuncsAddr.Value);
                foreach (var ptr in pointers) {
                    var tableAddr = (uint) Data.GetDouble((int) (ptr.Address - 0x04 - RamAddress));
                    Discoveries.AddArray(tableAddr, nameof(MapUpdateFunc) + "[]", "updateFuncTable", null);

                    // TODO: acatually add the table maybe?
                    while (true) {
                        var value1 = (uint) Data.GetDouble((int) (tableAddr - RamAddress + 0x00));
                        if (value1 == 0xFFFFFFFF)
                            break;

                        var value2 = (uint) Data.GetDouble((int) (tableAddr - RamAddress + 0x04));
                        if (!Discoveries.GetFunctions().Any(x => x.Address == value2)) {
                            var funcType = (value1 == 0x03) ? "GroundPlaneTickFunc" : $"Update{value1}Func";
                            Discoveries.AddFunction(value2, funcType, $"unknown{funcType}", null);
                        }

                        tableAddr += 0x08;
                    }
                }
            }
        }

        private ITable[] PopulateMapUpdateFuncTables() {
            var tables = new List<ITable>();

            // TODO: actually fetch it!
            var updateFuncTableAddresses = Discoveries.GetArrays()
                .Where(x => x.TypeName == nameof(MapUpdateFunc) + "[]")
                .ToArray();

            // TODO: what to do in this case?
            if (updateFuncTableAddresses.Length > 1)
                ;

            if (updateFuncTableAddresses.Length > 0) {
                var address = updateFuncTableAddresses[0].Address - RamAddress;
                tables.Add(MapUpdateFuncTable = MapUpdateFuncTable.Create(Data, nameof(MapUpdateFuncTable), (int) address, addEndModel: false));
            }

            return tables.ToArray();
        }

        private ITable[] PopulateModelInstanceTables() {
            var tables = new List<ITable>();

            // Look for all arrays discovered as 'ModelInstanceGroup[]'.
            ModelInstanceGroupTablesByAddress = new Dictionary<uint, ModelInstanceGroupTable>();
            var modelMatrixGroupTables = Discoveries.GetArrays()
                .Where(x => x.TypeName == nameof(ModelInstanceGroup) + "[]")
                .ToArray();

            // Create corresponding tables for all the discovered data.
            var modelMatrixGroupTableRamAddrs = modelMatrixGroupTables.Select(x => x.Address).OrderBy(x => x).Distinct().ToList();
            int groupIndex = 0;
            foreach (var ramAddr in modelMatrixGroupTableRamAddrs)
                tables.Add(ModelInstanceGroupTablesByAddress[ramAddr] = ModelInstanceGroupTable.Create(Data, $"{nameof(ModelInstanceGroup)}s_{groupIndex++:D2}", (int) (ramAddr - RamAddress), addEndModel: false));

            // Re-fetch all those new tables, ordered by address.
            var modelInstanceGroups = ModelInstanceGroupTablesByAddress.Values
                .SelectMany(x => x.Select(y => y))
                .OrderBy(x => x.Address)
                .Distinct()
                .ToArray();

            // Create sub-tables and mark them as 'Discovered'.
            int groupLinkIndex = 0;
            ModelInstanceTablesByAddress = new Dictionary<uint, ModelInstanceTable>();
            foreach (var group in modelInstanceGroups) {
                var modelsRamAddr = group.ModelInstanceTablePtr;
                var modelsAddr    = modelsRamAddr - RamAddress;

                // Create the ModelInstance sub-table.
                var newTable = ModelInstanceTablesByAddress[modelsRamAddr] = ModelInstanceTable.Create(Data, $"{nameof(ModelInstance)}s_{groupLinkIndex:D2}", (int) modelsAddr, null, addEndModel: false);
                tables.Add(newTable);

                // Mark the sub-table and its pointer as 'Discovered'.
                // TODO: Because this starts the 'ModelInstanceGroup[]' table, don't just replace the name with 'ModelInstance*', but append it.
                // TODO: ...but this is contrary to reality!! We need to mark something at an address as *multiple* types of data.
                Discoveries.AddArray(modelsRamAddr, $"{nameof(ModelInstance)}[]", $"modelInstances{groupLinkIndex:D2}", newTable.SizeInBytes);

                // The second parameter is a pointer to a 'ModelMatrix*'. It should be outside the bounds of the file, but try to mark it in case its not.
                var matricesRamPtr = group.MatrixTablePtr;
                Discoveries.AddArray(matricesRamPtr, "ModelMatrix[]", $"modelMatrices{groupLinkIndex:D2}", 0x38 * newTable.Length);
                // TODO: it's not adding pointers because they're outside the file.

                groupLinkIndex++;
            }

            return tables.ToArray();
        }

        private void PopulateScripts() {
            const int c_maxScriptLength = 0x1000;

            // ==================================================================
            // TODO: This is atrocious!!! Refactor this, like, 100 times, please.
            // ==================================================================

            ScriptsByAddress = new Dictionary<uint, ActorScript>();
            var scriptInfoByRamAddr    = new Dictionary<uint, List<string>>();
            var scriptRamAddrs         = new HashSet<uint>();
            var knownScriptRamAddrs    = new HashSet<uint>();
            var maybeScriptRamAddrs    = new HashSet<uint>();
            var probablyScriptRamAddrs = new HashSet<uint>();
            var pointerValues          = new HashSet<uint>();

            // Adds a line of text to a list of info for a confirmed or potential script by RAM address.
            void AddScriptInfo(uint ramAddr, string info, bool prepend) {
                if (!scriptInfoByRamAddr.ContainsKey(ramAddr))
                    scriptInfoByRamAddr[ramAddr] = new List<string>() { info };
                else if (prepend)
                    scriptInfoByRamAddr[ramAddr].Insert(0, info);
                else
                    scriptInfoByRamAddr[ramAddr].Add(info);
            }

            // On the off chance that we've discovered script pointers already, mark them as "known".
            var discoveredPointersByRamAddr = Discoveries.GetPointers()
                .ToDictionary(x => x.Address, x => x);

            foreach (var disc in discoveredPointersByRamAddr) {
                var addr = disc.Value.Address;
                var ramAddr = disc.Key;
                var potentialScriptRamAddr = (uint) Data.GetDouble((int) (addr - RamAddress));
                var potentialScriptAddr = potentialScriptRamAddr - RamAddress;

                // On the off chance that this was already discovered, add it.
                if (disc.Value.Name.Contains($"{nameof(ActorScript)}Command"))
                    AddScriptInfo(potentialScriptRamAddr, "Previously Discovered", prepend: true);
                // If this isn't an identified pointer, it's something else and not a potential script.
                else if (disc.Value.IsUnidentifiedPointer)
                    continue;

                // This could be a script: add some info.
                AddScriptInfo(potentialScriptRamAddr, $"Referenced at 0x{addr:X4} / 0x{ramAddr:X8}", prepend: false);
                _ = pointerValues.Add(potentialScriptRamAddr);
            }

            // Add known references to scripts from the NpcTable
            if (NpcTable != null) {
                var ramAddrs = NpcTable
                    .Select(x => (uint) (x.ScriptOffset))
                    .Where(x => x >= 0)
                    .OrderBy(x => x)
                    .Distinct()
                    .ToArray();

                foreach (var ramAddr in ramAddrs) {
                    _ = scriptRamAddrs.Add(ramAddr);
                    _ = knownScriptRamAddrs.Add(ramAddr);
                    AddScriptInfo(ramAddr, $"Referenced in {nameof(Models.Tables.X1.Town.NpcTable)}", prepend: true);
                }
            }

            // Add known references to scripts from any ModelInstanceTables
            if (ModelInstanceTablesByAddress != null) {
                var ramAddrs = ModelInstanceTablesByAddress
                    .SelectMany(x => x.Value.Select(y => y.ScriptAddr))
                    .Where(x => x >= 0)
                    .OrderBy(x => x)
                    .Distinct()
                    .ToArray();

                foreach (var ramAddr in ramAddrs) {
                    _ = scriptRamAddrs.Add(ramAddr);
                    _ = knownScriptRamAddrs.Add(ramAddr);
                    AddScriptInfo(ramAddr, $"Referenced in {nameof(ModelInstanceGroupTable)}", prepend: true);
                }
            }

            // Look for anything that potentially could be a script (filter out obvious negatives)
            var addrMax = Data.Length - 3;
            for (uint addr = 0; addr < addrMax; addr += 4) {
                // If this is an actual pointer here, it can't possibly be a script.
                var ramAddr = addr + RamAddress;
                if (discoveredPointersByRamAddr.ContainsKey(ramAddr) || knownScriptRamAddrs.Contains(ramAddr))
                    continue;
                if (ActorScriptUtils.DataLooksLikeBeginningOfScript(Data, addr)) {
                    _ = scriptRamAddrs.Add(ramAddr);
                    _ = maybeScriptRamAddrs.Add(ramAddr);
                }
            }

            // Start gathering script data, and measure their accuracy along the way.
            var accuracyByAddr = new Dictionary<uint, float>();

            // Read all scripts in their entirety.
            int nextScriptId = 0;
            foreach (var scriptAddr in scriptRamAddrs) {
                var scriptReader = new ScriptReader(Data, (int) (scriptAddr - RamAddress));
                _ = scriptReader.ReadUntilDoneDetected(c_maxScriptLength);

                // Don't add scripts that overflowed. Also filter out for some very likely false-positives.
                bool isJustTen = (scriptReader.CommandsRead == 1 && scriptReader.ScriptData[0] == 0x10);
                var accuracy = scriptReader.PercentValidCommands;
                if (scriptReader.Position >= c_maxScriptLength || scriptReader.Aborted == true || (maybeScriptRamAddrs.Contains(scriptAddr) && isJustTen) || accuracy < 0.75f) {
                    _ = knownScriptRamAddrs.Remove(scriptAddr);
                    _ = maybeScriptRamAddrs.Remove(scriptAddr);
                    _ = scriptRamAddrs.Remove(scriptAddr);
                    continue;
                }

                // This script looks valid enough -- add it.
                ScriptsByAddress[scriptAddr] = new ActorScript(Data, nextScriptId, $"Script_{nextScriptId:D2}", (int) (scriptAddr - RamAddress), scriptReader.ScriptData.Count * 4);
                nextScriptId++;
                accuracyByAddr[scriptAddr] = accuracy;

                // If this is definitely a script, was originally thought to *maybe* be a script, and has a pointer to it somewhere,
                // then let's just consider this a script. Move it to the correct set.
                if (accuracy == 1.00f && pointerValues.Contains(scriptAddr) && maybeScriptRamAddrs.Contains(scriptAddr) && !knownScriptRamAddrs.Contains(scriptAddr)) {
                    _ = maybeScriptRamAddrs.Remove(scriptAddr);
                    _ = probablyScriptRamAddrs.Add(scriptAddr);
                    AddScriptInfo(scriptAddr, "Looks like a referenced script", prepend: true);
                }
            }

            // We're going to start removing overlapping scripts, prioritizing known scripts and large scripts.
            var dataScriptBytes = new bool[Data.Length / 4];
            void MarkScriptBytes(uint addr) {
                var pos = (uint) (addr - RamAddress) / 4;
                for (int i = 0; i < ScriptsByAddress[addr].ScriptLength; i++)
                    dataScriptBytes[pos++] = true;
            }

            // Keep track of all the bytes known as scripts.
            foreach (var addr in knownScriptRamAddrs)
                MarkScriptBytes(addr);
            foreach (var addr in probablyScriptRamAddrs)
                MarkScriptBytes(addr);

            // Ugly brute-force method to keep adding the most accurate scripts to the bool and elimate overlapping ones
            var overlappingScriptsByAddr = new HashSet<uint>();

            while (maybeScriptRamAddrs.Count > 0) {
                // Remove 'maybe' scripts that intersect with 'knowns'.
                var maybeIterAddrs = new HashSet<uint>(maybeScriptRamAddrs);
                foreach (var addr in maybeIterAddrs) {
                    var pos = (addr - RamAddress) / 4;
                    for (int i = 0; i < ScriptsByAddress[addr].ScriptLength; i++) {
                        if (dataScriptBytes[pos++]) {
                            _ = overlappingScriptsByAddr.Add(addr);
                            _ = maybeScriptRamAddrs.Remove(addr);
                            break;
                        }
                    }
                }

                // Put the most accurate and longest maybe into the 'known' camp.
                if (maybeScriptRamAddrs.Count > 0) {
                    var mostAccurateMaybe = maybeScriptRamAddrs
                        .OrderByDescending(x => accuracyByAddr[x])
                        .ThenByDescending(x => ScriptsByAddress[x].Size)
                        .First();

                    _ = maybeScriptRamAddrs.Remove(mostAccurateMaybe);
                    _ = probablyScriptRamAddrs.Add(mostAccurateMaybe);

                    MarkScriptBytes(mostAccurateMaybe);
                    AddScriptInfo(mostAccurateMaybe, "Looks like an unreferenced script", prepend: true);
                }
            }

            // Filter out all the ones we don't think are real.
            ScriptsByAddress = ScriptsByAddress
                .Where(x => !overlappingScriptsByAddr.Contains(x.Key))
                .OrderByDescending(x => knownScriptRamAddrs.Contains(x.Key))
                .ThenByDescending(x => pointerValues.Contains(x.Key))
                .ThenBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.Value);

            // Add names to common scripts that have been identified.
            foreach (var addr in ScriptsByAddress.Keys) {
                ScriptsByAddress[addr].ScriptName = ActorScriptUtils.DetermineScriptName(ScriptsByAddress[addr]);
                if (scriptInfoByRamAddr.ContainsKey(addr))
                    ScriptsByAddress[addr].ScriptNote = string.Join("\r\n", scriptInfoByRamAddr[addr]);

                // Dump unknown commands to the debug console
                var scriptLines = ScriptsByAddress[addr].Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                var scriptCommands = scriptLines.Where(x => x.Contains("// ")).Select(x => x.Substring(x.IndexOf("// ") + 3)).ToList();
                if (scriptCommands.Any(x => x.StartsWith("Unknown"))) {
                    System.Diagnostics.Debug.WriteLine($"Unknown command used at 0x{addr:X8}" + (pointerValues.Contains(addr) ? " (has pointer):" : ":"));
                    System.Diagnostics.Debug.WriteLine(ScriptsByAddress[addr]);
                }
            }

            // Mark scripts as discovered.
            MarkScriptDiscoveries();
        }

        private void MarkScriptDiscoveries() {
            // Mark discovered scripts, unidentified pointers to them, and any functions they may contain.
            foreach (var kv in ScriptsByAddress) {
                var scriptRamAddr = kv.Key;
                var scriptAddr = scriptRamAddr - RamAddress;
                var script = kv.Value;

                var scriptName = (script.ScriptName == "") ? $"Unnamed_0x{script.Address + RamAddress:X8}" : script.ScriptName;
                // TODO: separate function for this, with a beautiful regex
                var scriptCodeNameBase = string.Join("", scriptName.Replace("(", "").Replace(")", "").Replace("-", "").Split(' ').Where(x => x.Length >= 1).Select(x => Char.ToUpper(x[0]) + x.Substring(1)));

                Discoveries.AddArray(scriptRamAddr, $"{nameof(ActorScript)}Command[]", $"script_{scriptCodeNameBase}", script.Size);

                // Looks for 'RunFunction' commands.
                var scriptReader = new ScriptReader(Data, script.Address);
                // TODO: truncate commands that exceed ScriptLength
                while (scriptReader.Position < script.ScriptLength) {
                    var command = scriptReader.ReadCommand();
                    var commandData = command.Data;

                    if (commandData[0] == (uint) ActorCommandType.RunFunction)
                        Discoveries.AddFunction(commandData[1], "ScriptFunc", $"scriptRunFunc_cmdId{command.Id}_{scriptCodeNameBase}", null);
                    else if (commandData[0] == (uint) ActorCommandType.SetProperty && commandData[1] == (uint) ActorPropertyCommandType.ThinkFunction)
                        Discoveries.AddFunction(commandData[2], "ScriptFunc", $"scriptThinkFunc_cmdId{command.Id}_{scriptCodeNameBase}", null);
                }
            }
        }

        private void AssociateScriptsWithRelevantTables() {
            if (NpcTable != null)
                foreach (var npc in NpcTable)
                    npc.ActorScripts = ScriptsByAddress;

            if (ModelInstanceTablesByAddress != null)
                foreach (var table in ModelInstanceTablesByAddress.Values)
                    table.ActorScripts = ScriptsByAddress;
        }

        public override void Dispose() {
            if (Battles != null) {
                foreach (var b in Battles.Where(x => x.Value != null))
                    b.Value.Dispose();
                Battles.Clear();
            }
        }

        public override string Title => base.Title + " Type: " + (IsBTL99 ? "BTL99" : IsBattle == true ? "Battle" : "Town");

        public uint RamAddress { get; }
        public bool IsBTL99 { get; }
        public bool IsBattle { get; private set; }

        [BulkCopyRecurse]
        public InteractableTable InteractableTable { get; private set; }
        [BulkCopyRecurse]
        public WarpTable WarpTable { get; private set; }
        [BulkCopyRecurse]
        public BattlePointersTable BattlePointersTable { get; private set; }
        [BulkCopyRecurse]
        public NpcTable NpcTable { get; private set; }
        [BulkCopyRecurse]
        public EnterTable EnterTable { get; private set; }
        [BulkCopyRecurse]
        public ArrowTable ArrowTable { get; private set; }

        [BulkCopyRecurse]
        public Dictionary<MapLeaderType, Battle> Battles { get; private set; }

        [BulkCopyRecurse]
        public TileMovementTable TileMovementTable { get; private set; }
        [BulkCopyRecurse]
        public CharacterTargetPriorityTable[] CharacterTargetPriorityTables { get; private set; }
        [BulkCopyRecurse]
        public CharacterTargetUnknownTable[] CharacterTargetUnknownTables { get; private set; }

        [BulkCopyRecurse]
        public Dictionary<uint, ModelInstanceGroupTable> ModelInstanceGroupTablesByAddress { get; private set; }
        [BulkCopyRecurse]
        public Dictionary<uint, ModelInstanceTable> ModelInstanceTablesByAddress { get; private set; }

        [BulkCopyRecurse]
        public Dictionary<uint, ActorScript> ScriptsByAddress { get; private set; }
        [BulkCopyRecurse]
        public MapUpdateFuncTable MapUpdateFuncTable { get; private set; }

        public DiscoveryContext Discoveries { get; private set; }
    }
}
