using System;
using System.Collections.Generic;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.KAO;
using SF3.Types;

namespace SF3.Models.Files.KAO {
    public class KAO_File : ScenarioTableFile, IKAO_File {
        // Not applicable
        public override int RamAddress => 0x00000000;
        // Not applicable
        public override int RamAddressLimit => 0x00000000;

        protected KAO_File(IByteData data, INameGetterContext nameGetterContext, ScenarioType scenario)
        : base(data, nameGetterContext, scenario) {
        }

        public static KAO_File Create(IByteData data, INameGetterContext nameGetterContext, ScenarioType scenario) {
            var newFile = new KAO_File(data, nameGetterContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize " + newFile.GetType().Name);
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            var tables = new ITable[] {
                (FaceChunkTable = FaceChunkTable.Create(Data, nameof(FaceChunkTable), 0))
            };

            foreach (var face in FaceChunkTable) {
                face.Data.IsModifiedChanged += (s, e) => {
                    if (face.Data.IsModified)
                        Data.IsModified = true;
                };
            }

            return tables;
        }

        public override bool OnFinish() {
            foreach (var face in FaceChunkTable) {
                var data = face.CompressedData;
                if (data.NeedsRecompression) {
                    data.Finish();
                    var newBytes = data.GetDataCopyOrReference();
                    if (face.MaxCompressedSize.HasValue && newBytes.Length > face.MaxCompressedSize.Value)
                        return false;

                    var newFileSize = face.ActualAddress + newBytes.Length;

                    if (newFileSize > Data.Length)
                        Data.Data.Resize(newFileSize);

                    Data.Data.SetDataAtTo(face.ActualAddress, newBytes.Length, newBytes);
                }
            }
            return true;
        }

        public FaceChunkTable FaceChunkTable { get; private set; }
    }
}
