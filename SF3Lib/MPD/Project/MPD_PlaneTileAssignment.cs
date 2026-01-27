using SF3.MPD.Interfaces;

namespace SF3.MPD.Project {
    public class MPD_PlaneTileAssignment : IMPD_PlaneTileAssignment {
        public MPD_PlaneTileAssignment(IMPD_PlaneTileAssignment original) {
            Width  = original.Width;
            Height = original.Height;
            _assignments = new (byte X, byte Y)[Width, Height];

            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    _assignments[x, y] = original[(byte) x, (byte) y];
        }

        private (byte X, byte Y)[,] _assignments;
        public (byte X, byte Y) this[byte x, byte y] {
            get => _assignments[x, y];
            set => _assignments[x, y] = value;
        }

        public int Width { get; }
        public int Height { get; }
    }
}
