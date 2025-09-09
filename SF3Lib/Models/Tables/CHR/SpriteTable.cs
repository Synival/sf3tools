using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Structs.CHR;

namespace SF3.Models.Tables.CHR {
    public class SpriteTable : TerminatedTable<Sprite> {
        protected SpriteTable(IByteData data, string name, int address, int startId, uint dataOffset, INameGetterContext ngc, bool isInCHP)
        : base(data, name, address, 4, maxSize: 300) {
            StartID = startId;
            DataOffset = dataOffset;
            NameGetterContext = ngc;
            IsInCHP = isInCHP;
        }

        public static SpriteTable Create(IByteData data, string name, int address, int startId, uint dataOffset, INameGetterContext ngc, bool isInCHP)
            => CreateBase(() => new SpriteTable(data, name, address, startId, dataOffset, ngc, isInCHP));

        public override bool Load() {
            int globalId = StartID;
            return Load(
                (id, addr) => {
                    var name = IsInCHP ? $"{nameof(Sprite)}{globalId - id:D2}_{id:D2}" : $"{nameof(Sprite)}{globalId:D2}";
                    return new Sprite(Data, globalId++, id, name, addr, DataOffset, NameGetterContext);
                },
                (rows, prevRow) => prevRow.Header.SpriteID != 0xFFFF, false
            );
        }

        public int StartID { get; }
        public uint DataOffset { get; }
        public INameGetterContext NameGetterContext { get; }
        public bool IsInCHP { get; }
    }
}
