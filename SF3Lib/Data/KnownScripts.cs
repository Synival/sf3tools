using System.Collections.Generic;

namespace SF3.Data {
    public class KnownScripts {
        public static Dictionary<string, uint[]> AllKnownScripts = new Dictionary<string, uint[]>() {
            { "Make Unpushable", new uint[] {
                0x00000015, 0x0000001E, 0x00000001,             // Set InteractFlags to 0x01
                0x00000010                                      // Done
            }},
            { "Wander", new uint[] {
                0x00000015, 0x0000000F, 0x00010000,             // Set MaxVelocity to 0x10000
                0x00000015, 0x00000010, 0x00008000,             // Set Acceleration to 0x8000
                0x0000001C, 0x00000002,                         // Set animation to 0x02
                0x80010000,                                     // (label 0x0010000)
                0x00000008, 0x00100000, 0x00100000, 0x00600000, // Wander between 0x100000 and 0x100000 units, max distance 0x600000 from home
                0x00000001,                                     // Wait until at move target
                0x0000000C, 0x0000FFFF, 0xC0010000,             // Loop to 0xC0010000 forever
            }},
            { "Shake/Shudder", new uint[] {
                0x80010000,                                     // (label 0x0010000)
                0x00000016, 0x00000009, 0x0000147A,             // Modify ScaleX by 0x147A
                0x00000016, 0x0000000A, 0x00000F5C,             // Modify ScaleY by 0xF5C
                0x00000000, 0x00000002,                         // Wait 2 frame(s)
                0x00000016, 0x00000009, 0xFFFFEB86,             // Modify ScaleX by -0x147A
                0x00000016, 0x0000000A, 0xFFFFF0A4,             // Modify ScaleY by -0xF5C
                0x00000000, 0x00000002,                         // Wait 2 frame(s)
                0x0000000C, 0x0000FFFF, 0x00000000,             // Loop to 0x00 forever
            }},
        };
    }
}
