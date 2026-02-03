using System;
using System.Text;
using Newtonsoft.Json.Linq;
using SF3.MPD.Interfaces;

namespace SF3.MPD.Extensions {
    public static class IMPD_PlaneTileAssignmentExtensions {
        public static JArray ToJArray(this IMPD_PlaneTileAssignment assignment) {
            JArray array = new JArray();

            var height = assignment.Height;
            var width = assignment.Width;

            for (int y = 0; y < height; y++) {
                var str = new StringBuilder();
                for (int x = 0; x < width; x++) {
                    const string base64Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
                    var (tileX, tileY) = assignment[(byte) x, (byte) y];
                    var value = tileX + (tileY * 64);
                    str.Append(new char[] {
                        tileY < 64 ? base64Chars[tileY] : (char) 0,
                        tileX < 64 ? base64Chars[tileX] : (char) 0,
                    });
                }
                array.Add(str.ToString());
            }

            return array;
        }
    }
}
