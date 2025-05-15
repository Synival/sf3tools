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

namespace SF3.Models.Files.X1 {
    public class X1_File : ScenarioTableFile, IX1_File {
        protected X1_File(IByteData data, INameGetterContext nameContext, ScenarioType scenario, bool isBTL99) : base(data, nameContext, scenario) {
            IsBTL99 = isBTL99;
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

            int sub;
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
            sub = IsBTL99 ? 0x06060000 : Scenario == ScenarioType.Scenario1 ? 0x0605f000 : 0x0605e000;

            RamAddress = (uint) sub;

            battlePointersPointerAddress = Data.GetDouble(battlePointersPointerPointerAddress) - sub;
            battlePointersAddress = Data.GetDouble(battlePointersPointerAddress);

            // A value higher means a pointer is on the address, meaning we are in a battle. If it is not a
            // pointer we are at our destination so we know a town is loaded.
            if (Scenario == ScenarioType.Scenario1 && battlePointersAddress > 0x0605F000 || battlePointersAddress > 0x0605e000) {
                battlePointersAddress -= sub;
                IsBattle = true;
            }
            else {
                battlePointersAddress = -1;
                IsBattle = false;
            }

            // The "Treasure" table is the only table present in all X1 files regardless of scenario or town/battle status.
            treasureAddress = Data.GetDouble(0x000c) - sub;

            if (isScn1OrBTL99) {
                hasLargeEnemyTable = true;

                warpAddress          = -1; // X002 file has Scenario1 WarpTable, and provides the address itself.
                npcAddress           = IsBattle == true ? -1 : battlePointersPointerAddress; // same address
                enterAddress         = Data.GetDouble(0x0024) - sub;
                arrowAddress         = -1; // Not present in Scenario1
            }
            else {
                hasLargeEnemyTable = false;

                warpAddress          = Data.GetDouble(0x0018) - sub;
                npcAddress           = IsBattle == true ? -1 : battlePointersPointerAddress; // same address
                enterAddress         = IsBattle == true ? -1 : Data.GetDouble(0x0030) - sub;
                arrowAddress         = IsBattle == true ? -1 : Data.GetDouble(0x0060) - sub;
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
                        lastBattle = Battle.Create(Data, NameGetterContext, mapLeader, battleTableAddress - sub, hasLargeEnemyTable, Scenario, lastBattle);
                        Battles.Add(mapLeader, lastBattle);
                    }
                }

                // Determine the location of the TileMovementTable, which isn't so straight-forward.
                // This table is not present in Scenario 1.
                if (!isScn1OrBTL99) {
                    // First, look inside a function for its address.
                    // The value we want is 0xac bytes later always (except for X1BTL330-339 and X1BTLP05)
                    var tileMovementAddressPointer = Data.GetDouble(0x000001c4) - sub + 0x00ac;

                    var priorityTablesOffset =
                        (Scenario == ScenarioType.Scenario2) ? 0x78 :
                        (Scenario == ScenarioType.Scenario3) ? 0x5c :
                                  /*ScenarioType.PremiumDisk*/ 0x5c;

                    var funcAddr = Data.GetDouble(0x01DC) - sub;
                    characterTargetPriorityTablesAddresses = Data.GetDouble(funcAddr + priorityTablesOffset) - sub;

                    // No problems with this method in Scenario 2.
                    if (Scenario == ScenarioType.Scenario2)
                        tileMovementAddress = Data.GetDouble(tileMovementAddressPointer) - sub;
                    else {
                        tileMovementAddress = Data.GetDouble(tileMovementAddressPointer);

                        // Is this a valid pointer to memory?
                        if (tileMovementAddress < 0x06070000 && tileMovementAddress > 0)
                            tileMovementAddress -= sub;
                        // If not, emply the workaround for X1BTL330-339 and X1BTLP05 not being consistant with everything else
                        // and locate the table directly.
                        // TODO: does this pointer exist in other X1BTL* files?
                        else
                            tileMovementAddress = Data.GetDouble(0x0024) - sub + 0x14;
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
                tables.Add(NpcTable = NpcTable.Create(Data, "NPCs", npcAddress));
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
                    var tableAddr = Data.GetDouble(tablePointerAddr) - sub;
                    var tableName = "CharacterTargetPriorities 0x" + i.ToString("X") + ": " + NameGetterContext.GetName(null, null, i, new object[] { NamedValueType.MovementType });
                    tables.Add(CharacterTargetPriorityTables[i] = CharacterTargetPriorityTable.Create(Data, tableName, tableAddr));
                    tablePointerAddr += 0x04;
                }

                CharacterTargetUnknownTables = new CharacterTargetUnknownTable[16];
                tablePointerAddr = characterTargetPriorityTablesAddresses + 0x140;
                for (int i = 0; i < 16; i++) {
                    var tableAddr = Data.GetDouble(tablePointerAddr) - sub;
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

            // TODO: Lazy! Let's do something better.
            PopulateScripts((uint) sub);

            // Locate difficult-to-find common functions/data that are shared between X1 files.
            var x1Data = Data.GetDataCopy();
            KnownDataByAddress =
                KnownX1Functions.AllKnownFunctions
                    .Select(x => new { Name = x.Key, Index = x1Data.IndexOfSubset(x.Value.ToByteArray()) })
                    .Where(x => x.Index >= 0)
                    .ToDictionary(x => (uint) x.Index + RamAddress, x => x.Name);

            int modelMatrixInitTableAddr = -1;

            // Look for references to that function.
            var instantiateModelsAddr = KnownDataByAddress.Where(x => x.Value == "InstantiateModels(ModelMatrixInit &initTable)").Select(x => (uint?) x.Key).FirstOrDefault();
            if (instantiateModelsAddr.HasValue) {
                var addrAsBytes = instantiateModelsAddr.Value.ToByteArray();
                var indexOfAddr = x1Data.IndexOfSubset(addrAsBytes);
                if (indexOfAddr >= 0) {
                    KnownDataByAddress[(uint) (indexOfAddr + sub)] = $"Pointer to InstantiateModels() (0x{instantiateModelsAddr.Value:X8})";

                    var preAddr = indexOfAddr - 4;
                    var preAddrValue = (x1Data[preAddr + 0] << 24) |
                                       (x1Data[preAddr + 1] << 16) |
                                       (x1Data[preAddr + 2] <<  8) |
                                       (x1Data[preAddr + 3] <<  0);

                    if (preAddrValue >= sub && preAddrValue < sub + x1Data.Length) {
                        KnownDataByAddress[(uint) (preAddr + sub)] = $"Pointer to ModelMatrixIndex[] (0x{preAddrValue:X8})";
                        modelMatrixInitTableAddr = preAddrValue - sub;
                    }
                }
            }

            if (modelMatrixInitTableAddr >= 0)
                tables.Add(ModelMatrixInitTable = ModelMatrixGroupTable.Create(Data, "ModelMatrixInit", modelMatrixInitTableAddr, addEndModel: false));

            return tables;
        }

        private void PopulateScripts(uint sub) {
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

            // Look for anything that potentially could be a script (filter out obvious negatives)
            var posMax = Data.Length - 3;
            var ptrMax = sub + posMax;
            for (uint pos = 0; pos < posMax; pos += 4) {
                var posAsPtr = pos + sub;
                var valuePos = pos;
                var value = (uint) Data.GetDouble((int) valuePos);

                if (knownScriptAddrs.Contains(posAsPtr)) {
                    AddScriptInfo(value, $"Referenced at 0x{posAsPtr:X8} (RAM), 0x{posAsPtr - sub:X2} (file)", prepend: false);
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
                else if (value >= sub && value < sub + Data.Length - 3) {
                    AddScriptInfo(value, $"Referenced at 0x{posAsPtr:X8} (RAM), 0x{posAsPtr - sub:X2} (file)", prepend: false);
                    _ = pointers.Add(value);
                }
            }

            // Start gathering script data, and measure their accuracy along the way.
            var accuracyByAddr = new Dictionary<uint, float>();

            int nextScriptId = 0;
            foreach (var scriptAddr in scriptAddrs) {
                var scriptReader = new ScriptReader(Data, (int) (scriptAddr - sub));
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

                ScriptsByAddress[scriptAddr] = new ActorScript(Data, nextScriptId, $"Script_{nextScriptId:D2}", (int) (scriptAddr - sub), scriptReader.ScriptData.Count * 4);
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
                var pos = (uint) (addr - sub) / 4;
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
                    var pos = (addr - sub) / 4;
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

        public override void Dispose() {
            if (Battles != null) {
                foreach (var b in Battles.Where(x => x.Value != null))
                    b.Value.Dispose();
                Battles.Clear();
            }
        }

        public override string Title => base.Title + " Type: " + (IsBTL99 ? "BTL99" : IsBattle == true ? "Battle" : "Town");

        public uint RamAddress { get; private set; }
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
        public ModelMatrixGroupTable ModelMatrixInitTable { get; private set; }

        [BulkCopyRecurse]
        public Dictionary<uint, ActorScript> ScriptsByAddress { get; private set; }

        public Dictionary<uint, string> KnownDataByAddress { get; private set; }
    }
}
