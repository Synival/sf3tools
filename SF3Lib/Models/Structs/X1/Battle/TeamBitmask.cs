using System.Linq;
using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X1.Battle {
    public class TeamBitmask : Struct {
        public TeamBitmask(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x02) {
        }

        [TableViewModelColumn(displayOrder: 0, displayFormat: "X4")]
        [BulkCopy]
        public ushort Bitmask {
            get => (ushort) Data.GetWord(Address);
            set => Data.SetWord(Address, value);
        }

        [TableViewModelColumn(displayOrder: 1, minWidth: 250)]
        public string Teams {
            get {
                short bit = 0x0001;
                string str = "";

                var bitmask = Bitmask;
                for (int team = 0; team < 16; team++, bit <<= 1) {
                    if ((bitmask & bit) != 0)
                        str = str + (str == "" ? "" : ", ") + team;
                }

                return str;
            }

            set {
                value = value ?? "";
                var teams = value.Split(',').Select(x => int.TryParse(x.Trim(), out var i) ? (int?) i : null).Where(x => x != null).ToArray();
                ushort newValue = 0;
                foreach (var team in teams) {
                    if (team >= 0 && team <= 0x0F)
                        newValue |= (ushort) (1 << team);
                }

                Bitmask = newValue;
            }
        }
    }
}
