using System;
using SF3.Types;

namespace SF3.FieldEditing {
    public class TileLayer {
        public TileLayer(TileType type, TileFill fill) {
            Type = type;
            Fill = fill;
        }

        public TileLayer(TileType type, string id) {
            Type = type;
            Fill = TileFill.None;

            foreach (var ch in id) {
                switch (ch) {
                    case '1': Fill |= TileFill.UL;      break;
                    case 'U': Fill |= TileFill.U;       break;
                    case '2': Fill |= TileFill.UR;      break;
                    case 'R': Fill |= TileFill.R;       break;
                    case '3': Fill |= TileFill.DR;      break;
                    case 'D': Fill |= TileFill.D;       break;
                    case '4': Fill |= TileFill.DL;      break;
                    case 'L': Fill |= TileFill.L;       break;
                    case 'C': Fill |= TileFill.C;       break;
                    case '*': Fill |= TileFill.Full;    break;
                    case '!': Fill |= TileFill.SteepUL; break;
                    case '@': Fill |= TileFill.SteepUR; break;
                    case '#': Fill |= TileFill.SteepDR; break;
                    case '$': Fill |= TileFill.SteepDL; break;

                    default:
                        throw new ArgumentException($"Bad TileLayer character: '{ch}'");
                }
            }
        }

        public override string ToString()
            => $"{Type} ({Fill.FlagString()})";

        public TileType Type { get; set; }
        public TileFill Fill { get; set; }
    }
}