using System;
using System.Collections.Generic;
using SF3.MPD.Interfaces;
using SF3.MPD.Project;

namespace SF3.Extensions {
    public static class IMPD_CollisionLineExtensions {
        public static bool BlockShouldCheck(this IMPD_CollisionLine line, int blockX, int blockY) {
            const short padding = 63;

            // Determine the boundaries of the block.
            short minX = (short) (blockX * 0x80 - padding);
            short maxX = (short) (blockX * 0x80 + 0x80 + padding);
            short minY = (short) (blockY * 0x80 - padding);
            short maxY = (short) (blockY * 0x80 + 0x80 + padding);

            // These bogus corrections brings accuracy waaaay up. No rational reason why.
            if (line.X1 == line.X2 && line.X1 % 0x80 == 0x40)
                maxX += 1;
            if (line.Y1 == line.Y2 && line.Y1 % 0x80 == 0x40)
                maxY += 1;

            // Return 'false' early if the line is out of bounds of any axis.
            if ((line.X1 < minX && line.X2 < minX) || (line.X1 > maxX && line.X2 > maxX))
                return false;
            if ((line.Y1 < minY && line.Y2 < minY) || (line.Y1 > maxY && line.Y2 > maxY))
                return false;

            var sqP1 = new MPD_CollisionPoint(minX, minY);
            var sqP2 = new MPD_CollisionPoint(maxX, minY);
            var sqP3 = new MPD_CollisionPoint(maxX, maxY);
            var sqP4 = new MPD_CollisionPoint(minX, maxY);

            var sqL1 = new MPD_CollisionLine(sqP1, sqP2);
            var sqL2 = new MPD_CollisionLine(sqP2, sqP3);
            var sqL3 = new MPD_CollisionLine(sqP3, sqP4);
            var sqL4 = new MPD_CollisionLine(sqP4, sqP1);

            bool IsPointInsideSquare(IMPD_CollisionPoint p)
                => p.X >= minX && p.X <= maxX && p.Y >= minY && p.Y <= maxY;

            if (IsPointInsideSquare(line.Point1) || IsPointInsideSquare(line.Point2))
                return true;

            bool LineSegmentsCross(IMPD_CollisionLine other) {
                // Find the four orientations needed for general and special cases
                int Orientation(IMPD_CollisionPoint a, IMPD_CollisionPoint b, IMPD_CollisionPoint c)
                {
                    long val = (long) (b.Y - a.Y) * (c.X - b.X) - (b.X - a.X) * (c.Y - b.Y);
                    if (val == 0)
                        return 0; // Collinear
                    return (val > 0) ? 1 : 2; // Clockwise or Counterclockwise
                }
                int o1 = Orientation(line.Point1, line.Point2, other.Point1);
                int o2 = Orientation(line.Point1, line.Point2, other.Point2);
                int o3 = Orientation(other.Point1, other.Point2, line.Point1);
                int o4 = Orientation(other.Point1, other.Point2, line.Point2);

                // General case
                if (o1 != o2 && o3 != o4)
                    return true;

                // Special Cases (collinear and overlapping)
                bool OnSegment(IMPD_CollisionPoint a, IMPD_CollisionPoint b, IMPD_CollisionPoint c) {
                    return b.X <= Math.Max(a.X, c.X) && b.X >= Math.Min(a.X, c.X) &&
                           b.Y <= Math.Max(a.Y, c.Y) && b.Y >= Math.Min(a.Y, c.Y);
                }
                return (o1 == 0 && OnSegment(line.Point1, other.Point1, line.Point2)) ||
                       (o2 == 0 && OnSegment(line.Point1, other.Point2, line.Point2)) ||
                       (o3 == 0 && OnSegment(other.Point1, line.Point1, other.Point2)) ||
                       (o4 == 0 && OnSegment(other.Point1, line.Point2, other.Point2));
            }

            return IsPointInsideSquare(line.Point1) ||
                   IsPointInsideSquare(line.Point2) ||
                   LineSegmentsCross(sqL1) ||
                   LineSegmentsCross(sqL2) ||
                   LineSegmentsCross(sqL3) ||
                   LineSegmentsCross(sqL4);
        }

        public static HashSet<(int X, int Y)> GetBlocksForChecking(this IMPD_CollisionLine line) {
            var blocks = new List<(int X, int Y)>();
            for (int y = 0; y < 16; y++)
                for (int x = 0; x < 16; x++)
                    if (line.BlockShouldCheck(x, y))
                        blocks.Add((x, y));
            return new HashSet<(int X, int y)>(blocks);
        }
    }
}
