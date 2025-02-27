using System;
using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.Shared;
using SF3.Models.Tables.X012;
using SF3.Types;
using static CommonLib.Utils.ResourceUtils;

namespace SF3.Models.Files.X012 {
    public class X012_File : ScenarioTableFile, IX012_File {
        public readonly int c_ramOffset = 0x06070000;

        protected X012_File(IByteData data, INameGetterContext nameContext, ScenarioType scenario) : base(data, nameContext, scenario) {
        }

        public static X012_File Create(IByteData data, INameGetterContext nameContext, ScenarioType scenario) {
            var newFile = new X012_File(data, nameContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize tables");
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            var tables = new List<ITable>() {
            };

            if (Scenario == ScenarioType.Scenario1) {
                TileMovementTable = TileMovementTable.Create(Data, "TileMovement", ResourceFile("MovementTypes.xml"), 0xB65C, false);

                ClassTargetPriorityTables = new ClassTargetPriorityTable[16];
                var tablePointerAddr = 0xB7AC;
                for (int i = 0; i < 16; i++) {
                    var tableAddr = Data.GetDouble(tablePointerAddr) - c_ramOffset;
                    var tableName = "CharacterTargetPriorityTable 0x" + i.ToString("X") + ": " + NameGetterContext.GetName(null, null, i, new object[] { NamedValueType.MovementType });
                    tables.Add(ClassTargetPriorityTables[i] = ClassTargetPriorityTable.Create(Data, tableName, tableAddr));
                    tablePointerAddr += 0x04;
                }

                ClassTargetUnknownTables = new ClassTargetUnknownTable[16];
                tablePointerAddr = 0xB8DC;
                for (int i = 0; i < 16; i++) {
                    var tableAddr = Data.GetDouble(tablePointerAddr) - c_ramOffset;
                    var tableName = "UnknownTable 0x" + i.ToString("X") + ": " + NameGetterContext.GetName(null, null, i, new object[] { NamedValueType.MovementType });
                    tables.Add(ClassTargetUnknownTables[i] = ClassTargetUnknownTable.Create(Data, tableName, tableAddr));
                    tablePointerAddr += 0x04;
                }
            }

            return tables;
        }

        public override void Dispose() {
        }

        [BulkCopyRecurse]
        public TileMovementTable TileMovementTable { get; private set; }

        [BulkCopyRecurse]
        public ClassTargetPriorityTable[] ClassTargetPriorityTables { get; private set; }

        [BulkCopyRecurse]
        public ClassTargetUnknownTable[] ClassTargetUnknownTables { get; private set; }
    }
}
