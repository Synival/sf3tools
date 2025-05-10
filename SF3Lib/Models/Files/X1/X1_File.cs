using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using CommonLib.Utils;
using SF3.ByteData;
using SF3.Data;
using SF3.Models.Tables;
using SF3.Models.Tables.Shared;
using SF3.Models.Tables.X1;
using SF3.Models.Tables.X1.Battle;
using SF3.Models.Tables.X1.Town;
using SF3.Types;
using static CommonLib.Utils.ResourceUtils;
using static CommonLib.Utils.ValueUtils;

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

            ScriptsByAddress = new Dictionary<uint, string>();
            ScriptNameByAddress = new Dictionary<uint, string>();

            var scriptAddrs = new HashSet<uint>();
            var knownScriptAddrs = new HashSet<uint>();
            var maybeScriptAddrs = new HashSet<uint>();
            var probablyScriptAddrs = new HashSet<uint>();
            var pointers = new HashSet<uint>();

            // Add known references to scripts
            if (NpcTable != null) {
                var addrs = NpcTable
                    .Select(x => (uint) (x.ScriptOffset))
                    .Where(x => x >= 0)
                    .OrderBy(x => x)
                    .Distinct()
                    .ToArray();

                foreach (var addr in addrs) {
                    scriptAddrs.Add(addr);
                    knownScriptAddrs.Add(addr);
                    ScriptNameByAddress.Add(addr, "NPC Script");
                }
            }

            // Look for anything that potentially could be a script (filter out obvious negatives)
            var posMax = Data.Length - 3;
            var ptrMax = sub + posMax;
            for (uint pos = 0; pos < posMax; pos += 4) {
                var posAsPtr = (uint) (pos + sub);
                if (knownScriptAddrs.Contains(posAsPtr))
                    continue;

                var valuePos = pos;
                var value = (uint) Data.GetDouble((int) valuePos);

                if (value >= 0x00000000 && value < 0x0000002E) {
                    scriptAddrs.Add(posAsPtr);
                    maybeScriptAddrs.Add(posAsPtr);
                }
                else if (value >= 0x80000000u && value < 0x80100000u) {
                    valuePos += 4;
                    if (valuePos < posMax) {
                        value = (uint) Data.GetDouble((int) valuePos);
                        if (value >= 0x00000000 && value < 0x0000002E) {
                            scriptAddrs.Add(posAsPtr);
                            maybeScriptAddrs.Add(posAsPtr);
                        }
                    }
                }
                else if (value >= sub && value < sub + Data.Length - 3)
                    pointers.Add(value);
            }

            // Start gathering script data, and measure their accuracy along the way.
            var scriptDataByAddr = new Dictionary<uint, uint[]>();
            var accuracyByAddr = new Dictionary<uint, float>();

            foreach (var scriptAddr in scriptAddrs) {
                var pos = 0;
                var scriptData = new List<uint>();

                uint ReadInt() {
                    var addr = (int) (scriptAddr - sub + pos * 4);
                    var i = (addr + 3 >= Data.Length) ? 0xFFFFFFFFu : (uint) Data.GetDouble(addr);
                    pos++;
                    scriptData.Add(i);
                    return i;
                };

                bool done = false;
                var script = "";
                int commandsRead = 0;
                int commandsKnown = 0;

                // Reads commands until we can't anymore.
                const int c_maxScriptLength = 0x200;
                bool exceeded = false;

                while (!done && pos < c_maxScriptLength) {
                    int commandPos = pos;

                    var command = ReadInt();
                    commandsRead++;
                    string note = "???";

                    // Get known commands
                    if (Enum.IsDefined(typeof(ActorCommandType), (int) command)) {
                        var commandType = (ActorCommandType) command;
                        var commandParams = EnumHelpers.GetAttributeOfType<ActorCommandParams>(commandType).Params;
                        commandsKnown++;

                        // Add commands for known commands.
                        var param = commandParams.Select(x => ReadInt()).ToArray();
                        switch (command) {
                            case 0x00: {
                                note = $"Wait {param[0]} frame(s)";
                                if (param[0] == 0x0000 || param[0] >= 1000) // Waiting 0 or thousands of frames probably isn't a thing
                                    commandsKnown--;
                                break;
                            }

                            case 0x01: note = "Wait until at move target"; break;
                            case 0x02: note = $"Set position to ({SignedHexStr(param[0], "X2")}, {SignedHexStr(param[1], "X2")}, {SignedHexStr(param[2], "X2")})"; break;
                            case 0x03: note = $"Set target position to ({SignedHexStr(param[0], "X2")}, {SignedHexStr(param[1], "X2")}, {SignedHexStr(param[2], "X2")})"; break;

                            case 0x04: {
                                note = $"Modify target position by ({SignedHexStr(param[0], "X2")}, {SignedHexStr(param[1], "X2")}, {SignedHexStr(param[2], "X2")})";
                                if (param[2] == 0x10) // common false positive
                                    commandsKnown--;
                                break;
                            }
                            case 0x06: note = $"Start moving to relative angle 0x{(short) param[0]:X4}, ahead 0x{param[1]}"; break;
                            case 0x08: note = $"Wander between 0x{param[0]:X2} and 0x{param[1]:X2} units, max distance 0x{param[2]:X2} from home"; break;
                            case 0x09: note = $"Wander (ignoring walls) between 0x{param[0]:X2} and 0x{param[1]:X2} units, max distance 0x{param[2]:X2} from home"; break;
                            case 0x0B: note = "Move towards target actor"; break;

                            case 0x0C: {
                                done = (param[0] == 0xFFFF);
                                note = $"Loop to 0x{param[1]:X2} " + (done ? "forever" : $"{param[0]} time(s)");
                                break;
                            }

                            case 0x0D: {
                                done = (param[0] <= pos);
                                note = $"Goto 0x{param[0]:X2}";
                                break;
                            }

                            case 0x10: {
                                note = "Done";
                                done = true;
                                break;
                            }

                            case 0x15: {
                                var property = (ActorPropertyCommandType) param[0];
                                var propertyName = EnumHelpers.EnumNameOr(property, p => $"Unknown0x{(int) p:X2}");
                                note = $"Set {propertyName} to {SignedHexStr(param[1], "X2")}";
                                break;
                            }

                            case 0x16: {
                                var property = (ActorPropertyCommandType) param[0];
                                var propertyName = EnumHelpers.EnumNameOr(property, p => $"Unknown0x{(int) p:X2}");
                                note = $"Modify {propertyName} by {SignedHexStr(param[1], "X2")}"; break;
                            }

                            case 0x1C:
                                note = $"Set animation to 0x{param[0]:X2}";
                                break;

                            case 0x1E: note = $"Play music/sound 0x{param[0]:X3}"; break;
                            case 0x22: note = $"Execute function 0x{param[0]:X8}"; break;

                            default:
                                commandsKnown--;
                                break;
                        }
                    }
                    // Get labels
                    else if (command >= 0x80000000u && command <= 0x80100000u) {
                        note = $"(label 0x{(command & 0x0FFFFFFF):X7})";
                        commandsKnown++;
                    }

                    // Add text to the script
                    for (int i = commandPos; i < pos; i++) {
                        script += (i == commandPos) ? "" : " ";
                        script += $"{(scriptData[i]):X8}";
                    }

                    var paramCount = (pos - commandPos);
                    var paramsMissing = (paramCount < 4) ? (4 - paramCount) : 0;
                    script += new string(' ', paramsMissing * 9) + $" ; {note}\r\n";

                    if ((int) (scriptAddr - sub) + (pos * 4) - 1 >= Data.Length) {
                        exceeded = true;
                        break;
                    }
                }

                // Don't add scripts that overflowed.
                if (pos >= c_maxScriptLength || exceeded == true) {
                    knownScriptAddrs.Remove(scriptAddr);
                    maybeScriptAddrs.Remove(scriptAddr);
                    scriptAddrs.Remove(scriptAddr);
                    continue;
                }

                scriptDataByAddr[scriptAddr] = scriptData.ToArray();
                ScriptsByAddress[scriptAddr] = script;

                var accuracy = (float) commandsKnown / commandsRead;
                accuracyByAddr[scriptAddr] = accuracy;

                // If this is definitely a script, was originally thought to *maybe* be a script, and has a pointer to it somewhere,
                // then let's just consider this a script. Move it to the correct set.
                if (accuracy == 1.00f && pointers.Contains(scriptAddr) && maybeScriptAddrs.Contains(scriptAddr)) {
                    maybeScriptAddrs.Remove(scriptAddr);
                    probablyScriptAddrs.Add(scriptAddr);
                    ScriptNameByAddress[scriptAddr] = "Has Pointer";
                }
            }

            var dataScriptBytes = new bool[Data.Length / 4];
            void MarkScriptBytes(uint addr) {
                var pos = (uint) (addr - sub) / 4;
                for (int i = 0; i < scriptDataByAddr[addr].Length; i++)
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
                    var pos = (uint) (addr - sub) / 4;
                    for (int i = 0; i < scriptDataByAddr[addr].Length; i++) {
                        if (dataScriptBytes[pos++]) {
                            overlappingScriptsByAddr.Add(addr);
                            maybeScriptAddrs.Remove(addr);
                            break;
                        }
                    }
                }

                // Put the most accurate and longest maybe into the 'known' camp.
                if (maybeScriptAddrs.Count > 0) {
                    var mostAccurateMaybe = maybeScriptAddrs
                        .OrderByDescending(x => accuracyByAddr[x])
                        .ThenByDescending(x => scriptDataByAddr[x].Length)
                        .First();

                    maybeScriptAddrs.Remove(mostAccurateMaybe);
                    probablyScriptAddrs.Add(mostAccurateMaybe);

                    MarkScriptBytes(mostAccurateMaybe);
                    ScriptNameByAddress[mostAccurateMaybe] = "Looks Like Script";
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
                var data = scriptDataByAddr[addr];
                var matchingData = KnownScripts.AllKnownScripts.Cast<KeyValuePair<string, uint[]>?>().FirstOrDefault(x => Enumerable.SequenceEqual(data, x.Value.Value));
                if (matchingData != null) {
                    if (!ScriptNameByAddress.ContainsKey(addr))
                        ScriptNameByAddress[addr] = $"({matchingData.Value.Key})";
                    else
                        ScriptNameByAddress[addr] += $" ({matchingData.Value.Key})";
                }
            }

            return tables;
        }

        public override void Dispose() {
            if (Battles != null) {
                foreach (var b in Battles.Where(x => x.Value != null))
                    b.Value.Dispose();
                Battles.Clear();
            }
        }

        public override string Title => base.Title + " Type: " + (IsBTL99 ? "BTL99" : IsBattle == true ? "Battle" : "Town");

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

        public Dictionary<uint, string> ScriptsByAddress { get; private set; }
        public Dictionary<uint, string> ScriptNameByAddress { get; private set; }
    }
}
