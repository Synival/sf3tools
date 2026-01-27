using System.Collections.Generic;
using System.Linq;
using CommonLib;
using SF3.ByteData;
using SF3.Models.Structs;

namespace SF3.Models.Tables.MPD {
    public class GroundAnimationTable : TerminatedTable<UnknownUInt8Struct>, IIndexedEnumerableWithLength<byte> {
        protected GroundAnimationTable(IByteData data, string name, int address, int? readUntil, int? maxSize)
        : base(data, name, GetRealAddress(data, address, readUntil), 1, maxSize) {
            IsDummiedOut = Address != address;
        }

        public static int GetRealAddress(IByteData data, int address, int? readUntil) {
            return (readUntil.HasValue && readUntil - address > 1 && data.GetByte(address) == 0xFF && data.GetByte(readUntil.Value - 1) == 0xFF)
                ? address + 1 : address;
        }

        public static GroundAnimationTable Create(IByteData data, string name, int address, int? readUntil, int? maxSize)
            => Create(() => new GroundAnimationTable(data, name, address, readUntil, maxSize));

        public override bool Load() {
            return Load(
                (id, address) => new UnknownUInt8Struct(Data, id, $"GrounAnimByte_{id:X2}", address),
                (currentRows, model) => model.Value != 0xFF, addEndModel: false);
        }

        byte IIndexedEnumerableWithLength<byte>.this[int index] => Rows[index].Value;

        IEnumerator<byte> IEnumerable<byte>.GetEnumerator() {
            foreach (var row in Rows)
                yield return row.Value;
        }

        byte[] IIndexedEnumerableWithLength<byte>.AsArray() => ((IEnumerable<byte>) this).ToArray();

        /// <summary>
        /// When true, the ground animation table is serialized, but "dummied-out" with a preceeding 0xFF that prevents it from loading.
        /// </summary>
        public bool IsDummiedOut { get; }
    }
}
