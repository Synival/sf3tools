using System.Collections.Generic;
using System.Linq;
using CommonLib.Attributes;
using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.MPD.Animation;
using SF3.Models.Tables.Shared;

namespace SF3.Models.Structs.KAO {
    public class FaceChunk : Struct, ITableContainer {
        public FaceChunk(IByteData data, int id, string name, int address, int actualAddress, CompressedData compressedData)
        : base(data, id, name, address, 0x22) {
            ActualAddress      = actualAddress;
            CompressedData     = compressedData;

            Header              = new FaceHeader(data, 0, nameof(FaceHeader), 0);
            PaletteTable        = ColorTable.Create(Data, "Palette", 0x22, 0x100);
            ImageTable          = FaceImageTable.Create(Data, nameof(FaceImageTable), this);
            CompositeImageTable = FaceCompositeImageTable.Create(Data, nameof(FaceCompositeImageTable), this);
            Spritesheet         = new FaceSpritesheet(this);

            Header.OnDimensionsChanged += (s, e) => {
                ImageTable[0].InvalidateImage();
            };

            Tables = new ITable[] {
                PaletteTable,
                ImageTable,
                CompositeImageTable
            };
        }

        public FaceHeader Header { get; }
        public ColorTable PaletteTable { get; }
        public FaceImageTable ImageTable { get; }
        public FaceCompositeImageTable CompositeImageTable { get; }
        public FaceSpritesheet Spritesheet { get; }

        [TableViewModelColumn(displayOrder: -1.5f, displayFormat: "X4", displayGroup: "Metadata")]
        public int ActualAddress { get; }

        public CompressedData CompressedData { get; }

        [TableViewModelColumn(displayOrder: -1.4f, displayFormat: "X4", displayGroup: "Metadata")]
        public int CompressedSize => CompressedData.Length;

        [TableViewModelColumn(displayOrder: -1.3f, displayFormat: "X4", isReadOnly: true, displayGroup: "Metadata")]
        public int? MaxCompressedSize { get; set; }

        [TableViewModelColumn(displayOrder: -1.2f, displayFormat: "X4", displayGroup: "Metadata")]
        public int DecompressedSize => Data.Length;

        public Palette Palette {
            get => new Palette(PaletteTable.Select(x => x.ColorABGR1555).ToArray());
            set {
                for (int i = 0; i < 0x100; i++)
                    PaletteTable[i].ColorABGR1555 = value[i].ToARGB1555();
                foreach (var image in ImageTable)
                    image.InvalidateImage();
            }
        }

        public IEnumerable<ITable> Tables { get; }
    }
}
