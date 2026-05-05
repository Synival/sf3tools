using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X1.Battle {
    public class MapMoveCoord : XYCoord {
        public MapMoveCoord(IByteData data, int id, string name, int address, MapLeaderType mapLeader, BattleHeader battleHeader)
        : base(data, id, name, address) {
            MapLeader    = mapLeader;
            BattleHeader = battleHeader;
        }

        public MapLeaderType MapLeader { get; }
        public BattleHeader BattleHeader { get; }
        public MapMoveCoordFlags Flags => BattleHeader.MapMoveCoordFlagsPointerTable?[(int) MapLeader]?.MapMoveCoordFlagsTable?[ID];

        [TableViewModelColumn(displayOrder: 2, minWidth: 100)]
        public MapMoveTeamType? Teams {
            get => Flags?.Teams;
            set {
                if (value != null && Flags != null)
                    Flags.Teams = value.Value;
            }
        }

        [TableViewModelColumn(displayOrder: 3)]
        public bool? AnyEventID {
            get => Flags?.AnyEventID;
            set {
                if (value != null && Flags != null)
                    Flags.AnyEventID = value.Value;
            }
        }

        [TableViewModelColumn(displayOrder: 4, displayFormat: "X2")]
        public byte? EventID {
            get => Flags?.EventID;
            set {
                if (value != null && Flags != null)
                    Flags.EventID = value.Value;
            }
        }
    }
}
