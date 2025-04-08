using System.Linq;
using SF3.Types;

namespace SF3.FieldEditing {
    /// <summary>
    /// NOTE: Everything here is an experimental proof-of-concept for editing. This will likely not reflect how it really works.
    ///       It only works with a very modified version of FIELD.MPD from the Premium Disk.
    /// </summary>
    public class TileDef {
        public TileDef(byte texId, TileLayer[] layers, TileOrientation orientation) {
            TexID           = texId;
            Layers          = layers;
            Orientation     = orientation;
            FlattenedLayers = new FlattenedLayers(Layers);
        }

        public static TileLayer[] TransformLayers(TileLayer[] layers, TileOrientation orientation)
            => layers.Select(x => TransformLayer(x, orientation)).ToArray();

        public static TileLayer TransformLayer(TileLayer layer, TileOrientation orientation) {
            var fill    = layer.Fill;
            var newFill = layer.Fill & TileFill.C;

            switch (orientation) {
                case TileOrientation.Normal:
                    newFill = fill;
                    break;

                case TileOrientation.FlipH:
                    newFill |=
                        (fill.HasFlag(TileFill.UR) ? TileFill.UL : 0) |
                        (fill.HasFlag(TileFill.U)  ? TileFill.U  : 0) |
                        (fill.HasFlag(TileFill.UL) ? TileFill.UR : 0) |
                        (fill.HasFlag(TileFill.L)  ? TileFill.R  : 0) |
                        (fill.HasFlag(TileFill.DL) ? TileFill.DR : 0) |
                        (fill.HasFlag(TileFill.D)  ? TileFill.D  : 0) |
                        (fill.HasFlag(TileFill.DR) ? TileFill.DL : 0) |
                        (fill.HasFlag(TileFill.R)  ? TileFill.L  : 0) |
                        (fill.HasFlag(TileFill.SteepUR) ? TileFill.SteepUL : 0) |
                        (fill.HasFlag(TileFill.SteepUL) ? TileFill.SteepUR : 0) |
                        (fill.HasFlag(TileFill.SteepDL) ? TileFill.SteepDR : 0) |
                        (fill.HasFlag(TileFill.SteepDR) ? TileFill.SteepDL : 0);
                    break;

                case TileOrientation.FlipV:
                    newFill |=
                        (fill.HasFlag(TileFill.DL) ? TileFill.UL : 0) |
                        (fill.HasFlag(TileFill.D)  ? TileFill.U  : 0) |
                        (fill.HasFlag(TileFill.DR) ? TileFill.UR : 0) |
                        (fill.HasFlag(TileFill.R)  ? TileFill.R  : 0) |
                        (fill.HasFlag(TileFill.UR) ? TileFill.DR : 0) |
                        (fill.HasFlag(TileFill.U)  ? TileFill.D  : 0) |
                        (fill.HasFlag(TileFill.UL) ? TileFill.DL : 0) |
                        (fill.HasFlag(TileFill.L)  ? TileFill.L  : 0) |
                        (fill.HasFlag(TileFill.SteepDL) ? TileFill.SteepUL : 0) |
                        (fill.HasFlag(TileFill.SteepDR) ? TileFill.SteepUR : 0) |
                        (fill.HasFlag(TileFill.SteepUR) ? TileFill.SteepDR : 0) |
                        (fill.HasFlag(TileFill.SteepUL) ? TileFill.SteepDL : 0);
                    break;

                case TileOrientation.FlipHV:
                    newFill |=
                        (fill.HasFlag(TileFill.DR) ? TileFill.UL : 0) |
                        (fill.HasFlag(TileFill.D)  ? TileFill.U  : 0) |
                        (fill.HasFlag(TileFill.DL) ? TileFill.UR : 0) |
                        (fill.HasFlag(TileFill.L)  ? TileFill.R  : 0) |
                        (fill.HasFlag(TileFill.UL) ? TileFill.DR : 0) |
                        (fill.HasFlag(TileFill.U)  ? TileFill.D  : 0) |
                        (fill.HasFlag(TileFill.UR) ? TileFill.DL : 0) |
                        (fill.HasFlag(TileFill.R)  ? TileFill.L  : 0) |
                        (fill.HasFlag(TileFill.SteepDR) ? TileFill.SteepUL : 0) |
                        (fill.HasFlag(TileFill.SteepDL) ? TileFill.SteepUR : 0) |
                        (fill.HasFlag(TileFill.SteepUL) ? TileFill.SteepDR : 0) |
                        (fill.HasFlag(TileFill.SteepUR) ? TileFill.SteepDL : 0);
                    break;

                case TileOrientation.Rotate90CW:
                    newFill |=
                        (fill.HasFlag(TileFill.DL) ? TileFill.UL : 0) |
                        (fill.HasFlag(TileFill.L)  ? TileFill.U  : 0) |
                        (fill.HasFlag(TileFill.UL) ? TileFill.UR : 0) |
                        (fill.HasFlag(TileFill.U)  ? TileFill.R  : 0) |
                        (fill.HasFlag(TileFill.UR) ? TileFill.DR : 0) |
                        (fill.HasFlag(TileFill.R)  ? TileFill.D  : 0) |
                        (fill.HasFlag(TileFill.DR) ? TileFill.DL : 0) |
                        (fill.HasFlag(TileFill.D)  ? TileFill.L  : 0) |
                        (fill.HasFlag(TileFill.SteepDL) ? TileFill.SteepUL : 0) |
                        (fill.HasFlag(TileFill.SteepUL) ? TileFill.SteepUR : 0) |
                        (fill.HasFlag(TileFill.SteepUR) ? TileFill.SteepDR : 0) |
                        (fill.HasFlag(TileFill.SteepDR) ? TileFill.SteepDL : 0);
                    break;

                case TileOrientation.Rotate270CW:
                    newFill |=
                        (fill.HasFlag(TileFill.UR) ? TileFill.UL : 0) |
                        (fill.HasFlag(TileFill.R)  ? TileFill.U  : 0) |
                        (fill.HasFlag(TileFill.DR) ? TileFill.UR : 0) |
                        (fill.HasFlag(TileFill.D)  ? TileFill.R  : 0) |
                        (fill.HasFlag(TileFill.DL) ? TileFill.DR : 0) |
                        (fill.HasFlag(TileFill.L)  ? TileFill.D  : 0) |
                        (fill.HasFlag(TileFill.UL) ? TileFill.DL : 0) |
                        (fill.HasFlag(TileFill.U)  ? TileFill.L  : 0) |
                        (fill.HasFlag(TileFill.SteepUR) ? TileFill.SteepUL : 0) |
                        (fill.HasFlag(TileFill.SteepDR) ? TileFill.SteepUR : 0) |
                        (fill.HasFlag(TileFill.SteepDL) ? TileFill.SteepDR : 0) |
                        (fill.HasFlag(TileFill.SteepUL) ? TileFill.SteepDL : 0);
                    break;

                case TileOrientation.FlipCornersURandDL:
                    newFill |=
                        (fill.HasFlag(TileFill.UL) ? TileFill.UL : 0) |
                        (fill.HasFlag(TileFill.L)  ? TileFill.U  : 0) |
                        (fill.HasFlag(TileFill.DL) ? TileFill.UR : 0) |
                        (fill.HasFlag(TileFill.D)  ? TileFill.R  : 0) |
                        (fill.HasFlag(TileFill.DR) ? TileFill.DR : 0) |
                        (fill.HasFlag(TileFill.R)  ? TileFill.D  : 0) |
                        (fill.HasFlag(TileFill.UR) ? TileFill.DL : 0) |
                        (fill.HasFlag(TileFill.U)  ? TileFill.L  : 0) |
                        (fill.HasFlag(TileFill.SteepUL) ? TileFill.SteepUL : 0) |
                        (fill.HasFlag(TileFill.SteepDL) ? TileFill.SteepUR : 0) |
                        (fill.HasFlag(TileFill.SteepDR) ? TileFill.SteepDR : 0) |
                        (fill.HasFlag(TileFill.SteepUR) ? TileFill.SteepDL : 0);
                    break;

                case TileOrientation.FlipCornersULandDR:
                    newFill |=
                        (fill.HasFlag(TileFill.DR) ? TileFill.UL : 0) |
                        (fill.HasFlag(TileFill.R)  ? TileFill.U  : 0) |
                        (fill.HasFlag(TileFill.UR) ? TileFill.UR : 0) |
                        (fill.HasFlag(TileFill.U)  ? TileFill.R  : 0) |
                        (fill.HasFlag(TileFill.UL) ? TileFill.DR : 0) |
                        (fill.HasFlag(TileFill.L)  ? TileFill.D  : 0) |
                        (fill.HasFlag(TileFill.DL) ? TileFill.DL : 0) |
                        (fill.HasFlag(TileFill.D)  ? TileFill.L  : 0) |
                        (fill.HasFlag(TileFill.SteepDR) ? TileFill.SteepUL : 0) |
                        (fill.HasFlag(TileFill.SteepUR) ? TileFill.SteepUR : 0) |
                        (fill.HasFlag(TileFill.SteepUL) ? TileFill.SteepDR : 0) |
                        (fill.HasFlag(TileFill.SteepDL) ? TileFill.SteepDL : 0);
                    break;
            };

            return new TileLayer(layer.Type, newFill);
        }

        public override string ToString()
            => $"0x{TexID:X2}@{Orientation} [" + string.Join(", ", Layers.Select(x => x.ToString())) + "]";

        public byte TexID { get; private set; }
        public TileLayer[] Layers { get; private set; }
        public TileOrientation Orientation { get; private set; }
        public FlattenedLayers FlattenedLayers { get; private set; }
    }
}
