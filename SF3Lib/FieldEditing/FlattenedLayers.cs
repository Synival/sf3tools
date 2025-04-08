using System;
using System.Collections.Generic;
using System.Linq;
using SF3.Types;

namespace SF3.FieldEditing {
    /// <summary>
    /// NOTE: Everything here is an experimental proof-of-concept for editing. This will likely not reflect how it really works.
    ///       It only works with a very modified version of FIELD.MPD from the Premium Disk.
    /// </summary>
    public class FlattenedLayers : IEqualityComparer<FlattenedLayers> {
        public FlattenedLayers(TileType?[,] tileTypes) {
            if (tileTypes.GetLength(0) != 3 || tileTypes.GetLength(1) != 3)
                throw new ArgumentException(nameof(tileTypes));

            Types = tileTypes;
            _hashCode = CreateHashCode();
        }

        public FlattenedLayers(TileLayer[] layers, TileType? underlyingType = TileType.Water) {
            layers = layers.ToArray();

            TileFill GetFillBitForCoordinate(int x, int y) {
                     if (x == 0 && y == 0) return TileFill.UL;
                else if (x == 1 && y == 0) return TileFill.U;
                else if (x == 2 && y == 0) return TileFill.UR;
                else if (x == 2 && y == 1) return TileFill.R;
                else if (x == 2 && y == 2) return TileFill.DR;
                else if (x == 1 && y == 2) return TileFill.D;
                else if (x == 0 && y == 2) return TileFill.DL;
                else if (x == 0 && y == 1) return TileFill.L;
                else if (x == 1 && y == 1) return TileFill.C;
                else
                    throw new InvalidOperationException();
            }

            TileType? GetTopmostLayerWithFillBit(TileFill bit)
                => layers.LastOrDefault(l => l.Fill.HasFlag(bit))?.Type;

            for (int ix = 0; ix <= 2; ix++) {
                for (int iy = 0; iy <= 2; iy++) {
                    var bit = GetFillBitForCoordinate(ix, iy);
                    Types[ix, iy] = GetTopmostLayerWithFillBit(bit) ?? underlyingType;
                    _hashCode = _hashCode * (_hashCode * 31) + ((int) (Types[ix, iy] ?? (TileType) (-1))) + 1;
                }
            }

            _hashCode = CreateHashCode();
        }

        private int CreateHashCode() {
            int hashCode = 0;
            for (int ix = 0; ix <= 2; ix++)
                for (int iy = 0; iy <= 2; iy++)
                    hashCode = _hashCode * (_hashCode * 31) + ((int) (Types[ix, iy] ?? (TileType) (-1))) + 1;
            return _hashCode;
        }

        public static bool operator==(FlattenedLayers lhs, FlattenedLayers rhs) {
            for (int ix = 0; ix <= 2; ix++)
                for (int iy = 0; iy <= 2; iy++)
                    if (!TypesEqual(lhs.Types[ix, iy], rhs.Types[ix, iy], lhs.Types[1, 1], rhs.Types[1, 1]))
                        return false;
            return true;
        }

        public static bool operator!=(FlattenedLayers lhs, FlattenedLayers rhs) {
            for (int ix = 0; ix <= 2; ix++)
                for (int iy = 0; iy <= 2; iy++)
                    if (TypesEqual(lhs.Types[ix, iy], rhs.Types[ix, iy], lhs.Types[1, 1], rhs.Types[1, 1]))
                        return false;
            return true;
        }

        private static bool TypesEqual(TileType? tt1, TileType? tt2, TileType? ttc1, TileType? ttc2) {
            if (tt1.HasValue && tt2.HasValue)
                return tt1.Value == tt2.Value;
            else if (!tt1.HasValue && tt2.HasValue && ttc1.HasValue)
                return tt2.Value != ttc1.Value;
            if (!tt2.HasValue && tt1.HasValue && ttc2.HasValue)
                return tt1.Value != ttc2.Value;
            else
                return tt1.Value == tt2.Value;
        }

        public TileType?[,] Types { get; private set; } = new TileType?[3, 3];

        public override bool Equals(object obj)
            => obj is FlattenedLayers layers && (FlattenedLayers) obj == this;

        private readonly int _hashCode;
        public override int GetHashCode()
            => _hashCode;

        public bool Equals(FlattenedLayers x, FlattenedLayers y)
            => x == y;

        public int GetHashCode(FlattenedLayers obj)
            => GetHashCode();
    }
}
