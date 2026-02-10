using System;
using System.Collections.Generic;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.KAO;
using SF3.Types;

namespace SF3.Models.Files.KAO {
    public class KAO_File : GameTableFile, IKAO_File {
        // Not applicable
        public override int RamAddress => 0x00000000;
        // Not applicable
        public override int RamAddressLimit => 0x00000000;

        protected KAO_File(IByteData data, INameGetterContext nameGetterContext, ScenarioType? scenario)
        : base(data, nameGetterContext, scenario) {
        }

        public static KAO_File Create(IByteData data, INameGetterContext nameGetterContext, ScenarioType? scenario) {
            var newFile = new KAO_File(data, nameGetterContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize " + newFile.GetType().Name);
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            var tables = new List<ITable> {
                (FaceChunkTable = FaceChunkTable.Create(Data, nameof(FaceChunkTable), 0))
            };
            foreach (var chunk in FaceChunkTable) {
                tables.Add(chunk.PaletteTable);
                tables.Add(chunk.ImageTable);
                tables.Add(chunk.CompositeImageTable);
            }

            foreach (var face in FaceChunkTable) {
                face.Data.IsModifiedChanged += (s, e) => {
                    if (face.Data.IsModified)
                        Data.IsModified = true;
                };
                tables.AddRange(face.Tables);
            }

            return tables.ToArray();
        }

        public override bool OnFinish() {
            // Make sure each face's data is recompressed.
            // TODO: Make this a separate function that can be accessed from the menu, similar to how MPDs work.
            // TODO: Saving should report errors if they're not recompressed.
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

            // Make sure the length of the data is a multiple of 0x800.
            if (Data.Length % 0x800 != 0)
                Data.Data.Resize(((Data.Length + 0x7FF) / 0x800) * 0x800);

            return true;
        }

        public FaceChunkTable FaceChunkTable { get; private set; }
    }
}
