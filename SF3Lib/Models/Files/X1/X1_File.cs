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
            DiscoveredDataByAddress = new Dictionary<uint, DiscoveredData>();
            var searchData = Data.GetDataCopy();
            DiscoverFunctions(searchData);
            DiscoverData(searchData);

            // Now that we've discovered some data, let's populate some tables.
            tables.AddRange(PopulateModelMatrixGroupTables());
            PopulateScripts();

            // Add references to the scripts for several tables so we can have nice dropdowns.
            AssociateScriptsWithRelevantTables();

            return tables;
        }

        private void DiscoverFunctions(byte[] data) {
            // Look for known functions and create corresponding DiscoveredData() entries.
            var funcDictionary = KnownX1Functions.AllKnownFunctions
                .Select(x => new { Name = x.Key, Size = x.Value.Length * 2, Index = data.IndexOfSubset(x.Value.ToByteArray()) })
                .Where(x => x.Index >= 0)
                .ToDictionary(x => (uint) x.Index, x => new DiscoveredData(x.Index, x.Size, DiscoveredDataType.Function, x.Name));

            foreach (var kv in funcDictionary)
                DiscoveredDataByAddress[kv.Key] = kv.Value;
        }

        private void DiscoverData(byte[] data) {
            // Look for references to that function. There are many variants of this function, so look for all of them.
            var instantiateModelsFuncs = DiscoveredDataByAddress
                .Where(x => x.Value.Type == DiscoveredDataType.Function && x.Value.Name.StartsWith("InstantiateModels"))
                .Select(x => x.Value)
                .ToArray();

            // On the off chance that multiple versions of this function exist (which never seems to be the case),
            // look for usages of all of them.
            foreach (var func in instantiateModelsFuncs) {
                // Get all pointers to this function.
                var addrAsBytes = ((uint) (func.Address + RamAddress)).ToByteArray();
                var funcPtrAddrs = data.IndicesOfSubset(addrAsBytes, alignment: 4);

                // For each function pointer, look at the pointer before it. This should be the parameter loaded in,
                // which is a pointer to a table we want to load.
                foreach (var funcPtrAddr in funcPtrAddrs) {
                    var funcPtrRamAddr = (uint) (funcPtrAddr + RamAddress);
                    DiscoveredDataByAddress[funcPtrRamAddr] = new DiscoveredData(funcPtrAddr, 4, DiscoveredDataType.Pointer, $"{func.Name}()*");

                    // Get the address of the previous pointer...
                    var modelsPtrAddr = (uint) (funcPtrAddr - 4);
                    var modelsPtrRamAddr = modelsPtrAddr + RamAddress;

                    // ...and the pointer itself. If it looks like a pointer, we're in business.
                    var modelsRamAddr = data.GetUInt((int) modelsPtrAddr);
                    if (modelsRamAddr >= RamAddress && modelsRamAddr < RamAddress + data.Length) {
                        var modelsAddr = modelsRamAddr - RamAddress;
                        DiscoveredDataByAddress[modelsPtrRamAddr] = new DiscoveredData((int) modelsPtrAddr, 4, DiscoveredDataType.Pointer, nameof(ModelMatrixGroup) + "*");
                        DiscoveredDataByAddress[modelsRamAddr]    = new DiscoveredData((int) modelsAddr, null, DiscoveredDataType.Table, nameof(ModelMatrixGroup) + "[]");
                    }
                }
            }
        }

        private ITable[] PopulateModelMatrixGroupTables() {
            var tables = new List<ITable>();

            ModelMatrixGroupTablesByAddress = new Dictionary<uint, ModelMatrixGroupTable>();
            var modelMatrixGroupTables = DiscoveredDataByAddress.Values.Where(x => x.Type == DiscoveredDataType.Table && x.Name == nameof(ModelMatrixGroup) + "[]").ToArray();

            var modelMatrixGroupTableAddrs = modelMatrixGroupTables.Select(x => x.Address).OrderBy(x => x).Distinct().ToList();
            int groupIndex = 0;
            foreach (var addr in modelMatrixGroupTableAddrs)
                tables.Add(ModelMatrixGroupTablesByAddress[(uint) (addr + RamAddress)] = ModelMatrixGroupTable.Create(Data, $"ModelMatrixGroups_{groupIndex++:X2}", addr, addEndModel: false));

            ModelMatrixGroupLinkTablesByAddress = new Dictionary<uint, ModelMatrixGroupLinkTable>();
            var modelMatrixGroupLinkTableAddrs = ModelMatrixGroupTablesByAddress.Values
                .SelectMany(x => x.Select(y => y.ModelMatrixGroupLinkTablePtr))
                .OrderBy(x => x)
                .Distinct()
                .ToArray();

            int groupLinkIndex = 0;
            foreach (var addr in modelMatrixGroupLinkTableAddrs)
                tables.Add(ModelMatrixGroupLinkTablesByAddress[addr] = ModelMatrixGroupLinkTable.Create(Data, $"ModelMatrixGroupLinks_{groupLinkIndex++:X2}", (int) (addr - RamAddress), null, addEndModel: false));

            return tables.ToArray();
        }

        private void PopulateScripts() {
            const int c_maxScriptLength = 0x1000;

            // ==================================================================
            // TODO: This is atrocious!!! Refactor this, like, 100 times, please.
            // ==================================================================

            ScriptsByAddress = new Dictionary<uint, ActorScript>();
            var scriptInfoByAddress = new Dictionary<uint, List<string>>();

            var scriptAddrs = new HashSet<uint>();
            var knownScriptAddrs = new HashSet<uint>();
            var maybeScriptAddrs = new HashSet<uint>();
            var probablyScriptAddrs = new HashSet<uint>();
            var pointers = new HashSet<uint>();

            void AddScriptInfo(uint addr, string info, bool prepend) {
                if (!scriptInfoByAddress.ContainsKey(addr))
                    scriptInfoByAddress[addr] = new List<string>() { info };
                else if (prepend)
                    scriptInfoByAddress[addr].Insert(0, info);
                else
                    scriptInfoByAddress[addr].Add(info);
            }

            // Add known references to scripts
            if (NpcTable != null) {
                var addrs = NpcTable
                    .Select(x => (uint) (x.ScriptOffset))
                    .Where(x => x >= 0)
                    .OrderBy(x => x)
                    .Distinct()
                    .ToArray();

                foreach (var addr in addrs) {
                    _ = scriptAddrs.Add(addr);
                    _ = knownScriptAddrs.Add(addr);
                    AddScriptInfo(addr, "Referenced in NPC Table", prepend: true);
                }
            }

            // Add known references to scripts
            if (ModelMatrixGroupLinkTablesByAddress != null) {
                var addrs = ModelMatrixGroupLinkTablesByAddress.SelectMany(x => x.Value.Select(y => y.ScriptAddr))
                    .Where(x => x >= 0)
                    .OrderBy(x => x)
                    .Distinct()
                    .ToArray();

                foreach (var addr in addrs) {
                    _ = scriptAddrs.Add(addr);
                    _ = knownScriptAddrs.Add(addr);
                    AddScriptInfo(addr, "Referenced in ModelMatrixGroupLink Table", prepend: true);
                }
            }

            // Look for anything that potentially could be a script (filter out obvious negatives)
            var posMax = Data.Length - 3;
            var ptrMax = RamAddress + posMax;
            for (uint pos = 0; pos < posMax; pos += 4) {
                var posAsPtr = pos + RamAddress;
                var valuePos = pos;
                var value = (uint) Data.GetDouble((int) valuePos);

                if (knownScriptAddrs.Contains(posAsPtr)) {
                    AddScriptInfo(value, $"Referenced at 0x{posAsPtr:X8} (RAM), 0x{posAsPtr - RamAddress:X2} (file)", prepend: false);
                    _ = pointers.Add(value);
                }
                else if (value >= 0x00000000 && value < 0x0000002E) {
                    _ = scriptAddrs.Add(posAsPtr);
                    _ = maybeScriptAddrs.Add(posAsPtr);
                }
                else if (value >= 0x80000000u && value < 0x80100000u) {
                    valuePos += 4;
                    if (valuePos < posMax) {
                        value = (uint) Data.GetDouble((int) valuePos);
                        if (value >= 0x00000000 && value < 0x0000002E) {
                            _ = scriptAddrs.Add(posAsPtr);
                            _ = maybeScriptAddrs.Add(posAsPtr);
                        }
                    }
                }
                else if (value >= RamAddress && value < RamAddress + Data.Length - 3) {
                    AddScriptInfo(value, $"Referenced at 0x{posAsPtr:X8} (RAM), 0x{posAsPtr - RamAddress:X2} (file)", prepend: false);
                    _ = pointers.Add(value);
                }
            }

            // Start gathering script data, and measure their accuracy along the way.
            var accuracyByAddr = new Dictionary<uint, float>();

            int nextScriptId = 0;
            foreach (var scriptAddr in scriptAddrs) {
                var scriptReader = new ScriptReader(Data, (int) (scriptAddr - RamAddress));
                _ = scriptReader.ReadUntilDoneDetected(c_maxScriptLength);

                // Don't add scripts that overflowed. Also filter out for some very likely false-positives.
                bool isJustTen = (scriptReader.CommandsRead == 1 && scriptReader.ScriptData[0] == 0x10);
                var accuracy = scriptReader.PercentValidCommands;
                if (scriptReader.Position >= c_maxScriptLength || scriptReader.Aborted == true || (maybeScriptAddrs.Contains(scriptAddr) && isJustTen) || accuracy < 0.75f) {
                    _ = knownScriptAddrs.Remove(scriptAddr);
                    _ = maybeScriptAddrs.Remove(scriptAddr);
                    _ = scriptAddrs.Remove(scriptAddr);
                    continue;
                }

                ScriptsByAddress[scriptAddr] = new ActorScript(Data, nextScriptId, $"Script_{nextScriptId:D2}", (int) (scriptAddr - RamAddress), scriptReader.ScriptData.Count * 4);
                nextScriptId++;
                accuracyByAddr[scriptAddr] = accuracy;

                // If this is definitely a script, was originally thought to *maybe* be a script, and has a pointer to it somewhere,
                // then let's just consider this a script. Move it to the correct set.
                if (accuracy == 1.00f && pointers.Contains(scriptAddr) && maybeScriptAddrs.Contains(scriptAddr) && !knownScriptAddrs.Contains(scriptAddr)) {
                    _ = maybeScriptAddrs.Remove(scriptAddr);
                    _ = probablyScriptAddrs.Add(scriptAddr);
                    AddScriptInfo(scriptAddr, "Looks like a referenced script", prepend: true);
                }
            }

            var dataScriptBytes = new bool[Data.Length / 4];
            void MarkScriptBytes(uint addr) {
                var pos = (uint) (addr - RamAddress) / 4;
                for (int i = 0; i < ScriptsByAddress[addr].ScriptLength; i++)
                    dataScriptBytes[pos++] = true;
            }

            // Keep track of all the bytes known as scripts.
            foreach (var addr in knownScriptAddrs)
                MarkScriptBytes(addr);
            foreach (var addr in probablyScriptAddrs)
                MarkScriptBytes(addr);

            // Ugly brute-force method to keep adding the most accurate scripts to the bool and elimate overlapping ones
            var overlappingScriptsByAddr = new HashSet<uint>();

            while (maybeScriptAddrs.Count > 0) {
                // Remove 'maybe' scripts that intersect with 'knowns'.
                var maybeIterAddrs = new HashSet<uint>(maybeScriptAddrs);
                foreach (var addr in maybeIterAddrs) {
                    var pos = (addr - RamAddress) / 4;
                    for (int i = 0; i < ScriptsByAddress[addr].ScriptLength; i++) {
                        if (dataScriptBytes[pos++]) {
                            _ = overlappingScriptsByAddr.Add(addr);
                            _ = maybeScriptAddrs.Remove(addr);
                            break;
                        }
                    }
                }

                // Put the most accurate and longest maybe into the 'known' camp.
                if (maybeScriptAddrs.Count > 0) {
                    var mostAccurateMaybe = maybeScriptAddrs
                        .OrderByDescending(x => accuracyByAddr[x])
                        .ThenByDescending(x => ScriptsByAddress[x].Size)
                        .First();

                    _ = maybeScriptAddrs.Remove(mostAccurateMaybe);
                    _ = probablyScriptAddrs.Add(mostAccurateMaybe);

                    MarkScriptBytes(mostAccurateMaybe);
                    AddScriptInfo(mostAccurateMaybe, "Looks like an unreferenced script", prepend: true);
                }
            }

            // Filter out all the ones we don't think are real
            ScriptsByAddress = ScriptsByAddress
                .Where(x => !overlappingScriptsByAddr.Contains(x.Key))
                .OrderByDescending(x => knownScriptAddrs.Contains(x.Key))
                .ThenByDescending(x => pointers.Contains(x.Key))
                .ThenBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.Value);

            // Add names to common scripts that have been identified
            foreach (var addr in ScriptsByAddress.Keys) {
                ScriptsByAddress[addr].ScriptName = ActorScriptUtils.DetermineScriptName(ScriptsByAddress[addr]);
                if (scriptInfoByAddress.ContainsKey(addr))
                    ScriptsByAddress[addr].ScriptNote = string.Join("\r\n", scriptInfoByAddress[addr]);

                // Dump unknown commands to the debug console
                var scriptLines = ScriptsByAddress[addr].Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                var scriptCommands = scriptLines.Where(x => x.Contains("// ")).Select(x => x.Substring(x.IndexOf("// ") + 3)).ToList();
                if (scriptCommands.Any(x => x.StartsWith("Unknown"))) {
                    System.Diagnostics.Debug.WriteLine($"Unknown command used at 0x{addr:X8}" + (pointers.Contains(addr) ? " (has pointer):" : ":"));
                    System.Diagnostics.Debug.WriteLine(ScriptsByAddress[addr]);
                }
            }
        }

        private void AssociateScriptsWithRelevantTables() {
            if (NpcTable != null)
                foreach (var npc in NpcTable)
                    npc.ActorScripts = ScriptsByAddress;

            if (ModelMatrixGroupLinkTablesByAddress != null)
                foreach (var table in ModelMatrixGroupLinkTablesByAddress.Values)
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
        public Dictionary<uint, ModelMatrixGroupTable> ModelMatrixGroupTablesByAddress { get; private set; }
        [BulkCopyRecurse]
        public Dictionary<uint, ModelMatrixGroupLinkTable> ModelMatrixGroupLinkTablesByAddress { get; private set; }

        [BulkCopyRecurse]
        public Dictionary<uint, ActorScript> ScriptsByAddress { get; private set; }

        public Dictionary<uint, DiscoveredData> DiscoveredDataByAddress { get; private set; }
    }
}
