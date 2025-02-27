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
                foreach (var mapLeader in (MapLeaderType[]) Enum.GetValues(typeof(MapLeaderType))) {
                    var mapIndex = (int) mapLeader;
                    var battleTableAddress = BattlePointersTable[mapIndex].Pointer;
                    if (battleTableAddress != 0)
                        Battles.Add(mapLeader, Battle.Create(Data, NameGetterContext, mapLeader, battleTableAddress - sub, hasLargeEnemyTable));
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
            if (treasureAddress >= 0)
                tables.Add(InteractableTable = InteractableTable.Create(Data, "Treasure", treasureAddress));
            if (warpAddress >= 0)
                tables.Add(WarpTable = WarpTable.Create(Data, "Warps", null, warpAddress));
            if (battlePointersAddress >= 0)
                tables.Add(BattlePointersTable);
            if (npcAddress >= 0)
                tables.Add(NpcTable = NpcTable.Create(Data, "NPCs", npcAddress));
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
                tables.Add(TileMovementTable = TileMovementTable.Create(Data, "TileMovement", ResourceFile("MovementTypes.xml"), tileMovementAddress, true));

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
    }
}
