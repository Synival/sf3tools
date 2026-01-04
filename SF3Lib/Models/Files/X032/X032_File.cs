using System;
using System.Collections.Generic;
using System.Reflection;
using CommonLib.Discovery;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Types;

namespace SF3.Models.Files.X032 {
    public class X032_File : ScenarioTableFile, IX032_File {
        public override int RamAddress => 0x06088000;
        public override int RamAddressLimit => 0x0609D800;

        protected X032_File(IByteData data, INameGetterContext nameContext, ScenarioType scenario) : base(data, nameContext, scenario) {
            Discoveries = new DiscoveryContext(Data.GetDataCopy(), (uint) RamAddress);
            Discoveries.DiscoverUnknownPointersToValueRange((uint) RamAddress, (uint) RamAddressLimit - 1);
        }

        public static X032_File Create(IByteData data, INameGetterContext nameContext, ScenarioType scenario) {
            var newFile = new X032_File(data, nameContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException($"Couldn't initialize {MethodBase.GetCurrentMethod().DeclaringType.Name}");
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            // TODO: no modifiable data yet
            return new ITable[0];
        }
    }
}
