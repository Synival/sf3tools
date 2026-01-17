using System.Collections.Generic;
using SF3.ByteData;
using SF3.Models.Structs.MPD.Main;

namespace SF3.Models.Tables.MPD.Main {
    public class GradientTable : Table<Gradient> {
        protected GradientTable(IByteData data, string name, int address, int? readUntil) : base(data, name, address) {
            ReadUntil = readUntil;
        }

        public static GradientTable Create(IByteData data, string name, int address, int? readUntil)
            => Create(() => new GradientTable(data, name, address, readUntil));

        public override bool Load() {
            var rows = new List<Gradient>();
            var addr = Address;
            var nextId = 0;
            var isDummiedOut = false;

            try {
                while (!ReadUntil.HasValue || addr < ReadUntil.Value) {
                    // If we encounter 0xFFFF, that doesn't necessarily mean we've reached the end.
                    // There's often a hidden unused gradient behind it. Check for it in the next pass.
                    if (Data.GetWord(addr) == 0xFFFF) {
                        addr += 2;
                        if (ReadUntil.HasValue) {
                            isDummiedOut = true;
                            continue;
                        }
                        else
                            break;
                    }

                    // Could be a gradient, but it needs to be big enough.
                    if (ReadUntil.HasValue && addr + 0x18 > ReadUntil.Value)
                        break;

                    rows.Add(new Gradient(Data, nextId, $"Gradient_{nextId}", addr, isDummiedOut));
                    nextId++;
                    addr += 0x18;
                }
            }
            catch {
                return false;
            }
            finally {
                _rows = rows.ToArray();
            }

            return true;
        }

        public override int TerminatorSize => 2;
        public override bool IsContiguous => true;
        public int? ReadUntil { get; }
    }
}
