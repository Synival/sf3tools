using System.Collections.Generic;

namespace SF3.Actors {
    public class KnownScripts {
        public static Dictionary<string, uint[]> AllKnownScripts = new Dictionary<string, uint[]>() {
            { "Make Solid Unpushable", new uint[] {
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
            { "Wander (Simple Version)", new uint[] {
                0x00000015, 0x0000000F, 0x00010000,             // Set MaxVelocity to 0x10000
                0x00000015, 0x00000010, 0x00008000,             // Set Acceleration to 0x8000
                0x00000008, 0x00100000, 0x00100000, 0x00600000, // Wander between 0x100000 and 0x100000 units, max distance 0x600000 from home
                0x00000001,                                     // Wait until at move target
                0x0000000C, 0x0000FFFF, 0x00000000,             // Loop to 0x00 forever
            }},
            { "Wander (Simplest Version)", new uint[] {
                0x00000008, 0x00100000, 0x00100000, 0x00600000, // Wander between 0x100000 and 0x100000 units, max distance 0x600000 from home
                0x00000001,                                     // Wait until at move target
                0x0000000C, 0x0000FFFF, 0x00000000,             // Loop to 0x00 forever
            }},
            { "Wander (Quickly)", new uint[] {
                0x00000015, 0x0000000F, 0x00020000,             // Set MaxVelocity to 0x20000
                0x00000015, 0x00000010, 0x00010000,             // Set Acceleration to 0x10000
                0x00000008, 0x00100000, 0x00100000, 0x00600000, // Wander between 0x100000 and 0x100000 units, max distance 0x600000 from home
                0x00000001,                                     // Wait until at move target
                0x0000000C, 0x0000FFFF, 0x00000000,             // Loop to 0x00 forever
            }},
            { "Shake/Shudder (Light)", new uint[] {
                0x80010000,                                     // (label 0x0010000)
                0x00000016, 0x00000009, 0x00000A3D,             // Modify ScaleX by 0xA3D
                0x00000016, 0x0000000A, 0x000007AE,             // Modify ScaleY by 0x7AE
                0x00000000, 0x00000003,                         // Wait 3 frame(s)
                0x00000016, 0x00000009, 0xFFFFF5C3,             // Modify ScaleX by -0xA3D
                0x00000016, 0x0000000A, 0xFFFFF852,             // Modify ScaleY by -0x7AE
                0x00000000, 0x00000003,                         // Wait 3 frame(s)
                0x0000000C, 0x0000FFFF, 0x00000000,             // Loop to 0x00 forever
            }},
            { "Shake/Shudder (Heavy)", new uint[] {
                0x80010000,                                     // (label 0x0010000)
                0x00000016, 0x00000009, 0x0000147A,             // Modify ScaleX by 0x147A
                0x00000016, 0x0000000A, 0x00000F5C,             // Modify ScaleY by 0xF5C
                0x00000000, 0x00000002,                         // Wait 2 frame(s)
                0x00000016, 0x00000009, 0xFFFFEB86,             // Modify ScaleX by -0x147A
                0x00000016, 0x0000000A, 0xFFFFF0A4,             // Modify ScaleY by -0xF5C
                0x00000000, 0x00000002,                         // Wait 2 frame(s)
                0x0000000C, 0x0000FFFF, 0x00000000,             // Loop to 0x00 forever
            }},
            { "Do Nothing", new uint[] {
                0x00000010,                                     // Done
            }},
            { "Appear by Growing", new uint[] {
                0x00000015, 0x00000009, 0x00000000,             // Set ScaleX to 0x00
                0x00000015, 0x0000000A, 0x00000000,             // Set ScaleY to 0x00
                0x00000015, 0x0000000B, 0x00000000,             // Set ScaleZ to 0x00
                0x80010000,                                     // (label 0x0010000)
                0x00000016, 0x00000009, 0x00001000,             // Modify ScaleX by 0x1000
                0x00000016, 0x0000000A, 0x00001000,             // Modify ScaleY by 0x1000
                0x00000016, 0x0000000B, 0x00001000,             // Modify ScaleZ by 0x1000
                0x00000016, 0x00000003, 0x00002000,             // Modify RotationY by 0x2000
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x0000000C, 0x00000010, 0xC0010000,             // Loop to 0xC0010000 16 time(s)
                0x00000010,                                     // Done
            }},
            { "Appear by Growing and Spinning", new uint[] {
                0x00000015, 0x00000009, 0x00000000,             // Set ScaleX to 0x00
                0x00000015, 0x0000000A, 0x00000000,             // Set ScaleY to 0x00
                0x00000015, 0x0000000B, 0x00000000,             // Set ScaleZ to 0x00
                0x80010000,                                     // (label 0x0010000)
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x00000016, 0x00000009, 0x00001000,             // Modify ScaleX by 0x1000
                0x00000016, 0x0000000A, 0x00001000,             // Modify ScaleY by 0x1000
                0x00000016, 0x0000000B, 0x00001000,             // Modify ScaleZ by 0x1000
                0x00000016, 0x00000003, 0x00002000,             // Modify RotationY by 0x2000
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x0000000C, 0x00000010, 0xC0010000,             // Loop to 0xC0010000 16 time(s)
                0x00000010,                                     // Done
            }},
            { "Door Open (Clockwise v1)", new uint[] {
                0x0000001E, 0x00000234,                         // Play music/sound 0x234
                0x80010000,                                     // (label 0x0010000)
                0x00000016, 0x00000003, 0x00000222,             // Modify RotationY by 0x222
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x0000000C, 0x0000001E, 0xC0010000,             // Loop to 0xC0010000 30 time(s)
                0x00000010,                                     // Done
            }},
            { "Door Open (Clockwise v2)", new uint[] {
                0x0000001E, 0x00000234,                         // Play music/sound 0x234
                0x80010000,                                     // (label 0x0010000)
                0x00000016, 0x00000003, 0x00000200,             // Modify RotationY by 0x200
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x0000000C, 0x00000020, 0xC0010000,             // Loop to 0xC0010000 32 time(s)
                0x00000010,                                     // Done
            }},
            { "Door Open (Counter-Clockwise v1)", new uint[] {
                0x0000001E, 0x00000234,                         // Play music/sound 0x234
                0x80010000,                                     // (label 0x0010000)
                0x00000016, 0x00000003, 0xFFFFFDDE,             // Modify RotationY by -0x222
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x0000000C, 0x0000001E, 0xC0010000,             // Loop to 0xC0010000 30 time(s)
                0x00000010,                                     // Done
            }},
            { "Door Open (Counter-Clockwise v2)", new uint[] {
                0x0000001E, 0x00000234,                         // Play music/sound 0x234
                0x80010000,                                     // (label 0x0010000)
                0x00000016, 0x00000003, 0xFFFFFE00,             // Modify RotationY by -0x200
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x0000000C, 0x00000020, 0xC0010000,             // Loop to 0xC0010000 32 time(s)
                0x00000010,                                     // Done
            }},
            { "Twitch (Regularly, Early Version)", new uint[] {
                0x00000016, 0x00000009, 0xFFFFF000,             // Modify ScaleX by -0x1000
                0x00000016, 0x0000000A, 0x00000800,             // Modify ScaleY by 0x800
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x00000016, 0x00000009, 0x00000800,             // Modify ScaleX by 0x800
                0x00000016, 0x0000000A, 0xFFFFFC00,             // Modify ScaleY by -0x400
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x00000016, 0x00000009, 0x00000400,             // Modify ScaleX by 0x400
                0x00000016, 0x0000000A, 0xFFFFFE00,             // Modify ScaleY by -0x200
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x00000016, 0x00000009, 0x00000400,             // Modify ScaleX by 0x400
                0x00000016, 0x0000000A, 0xFFFFFE00,             // Modify ScaleY by -0x200
                0x00000000, 0x0000001E,                         // Wait 30 frame(s)
                0x0000000C, 0x0000FFFF, 0x00000000,             // Loop to 0x00 forever
            }},
            { "Twitch (Regularly)", new uint[] {
                0x00000015, 0x0000001E, 0x00000001,             // Set InteractFlags to 0x01
                0x80010000,                                     // (label 0x0010000)
                0x00000016, 0x00000009, 0xFFFFF000,             // Modify ScaleX by -0x1000
                0x00000016, 0x0000000A, 0x00000800,             // Modify ScaleY by 0x800
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x00000016, 0x00000009, 0x00000800,             // Modify ScaleX by 0x800
                0x00000016, 0x0000000A, 0xFFFFFC00,             // Modify ScaleY by -0x400
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x00000016, 0x00000009, 0x00000400,             // Modify ScaleX by 0x400
                0x00000016, 0x0000000A, 0xFFFFFE00,             // Modify ScaleY by -0x200
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x00000016, 0x00000009, 0x00000400,             // Modify ScaleX by 0x400
                0x00000016, 0x0000000A, 0xFFFFFE00,             // Modify ScaleY by -0x200
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x00000015, 0x00000009, 0x00010000,             // Set ScaleX to 0x10000
                0x00000015, 0x0000000A, 0x00010000,             // Set ScaleY to 0x10000
                0x00000000, 0x0000001E,                         // Wait 30 frame(s)
                0x0000000C, 0x0000FFFF, 0xC0010000,             // Loop to 0xC0010000 forever
            }},
            { "Twitch (Sporadically)", new uint[] {
                0x00000015, 0x0000001E, 0x00000001,             // Set InteractFlags to 0x01
                0x80010000,                                     // (label 0x0010000)
                0x00000016, 0x00000009, 0xFFFFF000,             // Modify ScaleX by -0x1000
                0x00000016, 0x0000000A, 0x00000800,             // Modify ScaleY by 0x800
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x00000016, 0x00000009, 0x00000800,             // Modify ScaleX by 0x800
                0x00000016, 0x0000000A, 0xFFFFFC00,             // Modify ScaleY by -0x400
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x00000016, 0x00000009, 0x00000400,             // Modify ScaleX by 0x400
                0x00000016, 0x0000000A, 0xFFFFFE00,             // Modify ScaleY by -0x200
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x00000015, 0x00000009, 0x00010000,             // Set ScaleX to 0x10000
                0x00000015, 0x0000000A, 0x00010000,             // Set ScaleY to 0x10000
                0x00000000, 0x0000001E,                         // Wait 30 frame(s)
                0x00000016, 0x00000009, 0xFFFFF000,             // Modify ScaleX by -0x1000
                0x00000016, 0x0000000A, 0x00000800,             // Modify ScaleY by 0x800
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x00000016, 0x00000009, 0x00000800,             // Modify ScaleX by 0x800
                0x00000016, 0x0000000A, 0xFFFFFC00,             // Modify ScaleY by -0x400
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x00000016, 0x00000009, 0x00000400,             // Modify ScaleX by 0x400
                0x00000016, 0x0000000A, 0xFFFFFE00,             // Modify ScaleY by -0x200
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x00000015, 0x00000009, 0x00010000,             // Set ScaleX to 0x10000
                0x00000015, 0x0000000A, 0x00010000,             // Set ScaleY to 0x10000
                0x00000000, 0x00000003,                         // Wait 3 frame(s)
                0x00000016, 0x00000009, 0xFFFFF000,             // Modify ScaleX by -0x1000
                0x00000016, 0x0000000A, 0x00000800,             // Modify ScaleY by 0x800
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x00000016, 0x00000009, 0x00000800,             // Modify ScaleX by 0x800
                0x00000016, 0x0000000A, 0xFFFFFC00,             // Modify ScaleY by -0x400
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x00000016, 0x00000009, 0x00000400,             // Modify ScaleX by 0x400
                0x00000016, 0x0000000A, 0xFFFFFE00,             // Modify ScaleY by -0x200
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x00000015, 0x00000009, 0x00010000,             // Set ScaleX to 0x10000
                0x00000015, 0x0000000A, 0x00010000,             // Set ScaleY to 0x10000
                0x00000000, 0x00000014,                         // Wait 20 frame(s)
                0x0000000C, 0x0000FFFF, 0xC0010000,             // Loop to 0xC0010000 forever
            }},
            { "Reset Scale (Sprite)", new uint[] {
                0x00000015, 0x00000009, 0x00010000,             // Set ScaleX to 0x10000
                0x00000015, 0x0000000A, 0x00010000,             // Set ScaleY to 0x10000
                0x00000010,                                     // Done
            }},
            { "Spasm Violently (v1)", new uint[] {
                0x00000015, 0x0000001E, 0x00000001,             // Set InteractFlags to 0x01
                0x00000016, 0x0000000A, 0xFFFFFB00,             // Modify ScaleY by -0x500
                0x00000016, 0x00000009, 0xFFFFEE00,             // Modify ScaleX by -0x1200
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x00000016, 0x00000009, 0x00001200,             // Modify ScaleX by 0x1200
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x00000016, 0x00000009, 0xFFFFEE00,             // Modify ScaleX by -0x1200
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x00000016, 0x00000009, 0x00001200,             // Modify ScaleX by 0x1200
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x00000016, 0x0000000A, 0x00000500,             // Modify ScaleY by 0x500
                0x0000000C, 0x0000FFFF, 0x00000000,             // Loop to 0x00 forever
            }},
            { "Spasm Violently (v2)", new uint[] {
                0x00000015, 0x0000001E, 0x00000001,             // Set InteractFlags to 0x01
                0x80010000,                                     // (label 0x0010000)
                0x00000016, 0x0000000A, 0xFFFFFB00,             // Modify ScaleY by -0x500
                0x00000016, 0x00000009, 0xFFFFEE00,             // Modify ScaleX by -0x1200
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x00000016, 0x00000009, 0x00001200,             // Modify ScaleX by 0x1200
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x00000016, 0x00000009, 0xFFFFEE00,             // Modify ScaleX by -0x1200
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x00000016, 0x00000009, 0x00001200,             // Modify ScaleX by 0x1200
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x00000016, 0x0000000A, 0x00000500,             // Modify ScaleY by 0x500
                0x0000000C, 0x0000FFFF, 0xC0010000,             // Loop to 0xC0010000 forever
            }},
            { "Stretchy Bounce", new uint[] {
                0x00000016, 0x00000009, 0x00002000,             // Modify ScaleX by 0x2000
                0x00000016, 0x0000000A, 0xFFFFE000,             // Modify ScaleY by -0x2000
                0x00000000, 0x00000002,                         // Wait 2 frame(s)
                0x0000000C, 0x00000004, 0x00000000,             // Loop to 0x00 4 time(s)
                0x00000000, 0x00000003,                         // Wait 3 frame(s)
                0x80010000,                                     // (label 0x0010000)
                0x00000016, 0x00000009, 0xFFFFC000,             // Modify ScaleX by -0x4000
                0x00000016, 0x0000000A, 0x00004000,             // Modify ScaleY by 0x4000
                0x00000000, 0x00000002,                         // Wait 2 frame(s)
                0x0000000C, 0x00000002, 0xC0010000,             // Loop to 0xC0010000 2 time(s)
                0x00000016, 0x00000009, 0xFFFFE000,             // Modify ScaleX by -0x2000
                0x00000016, 0x0000000A, 0x00002000,             // Modify ScaleY by 0x2000
                0x00000016, 0x00000006, 0xFFF60000,             // Modify PositionY by -0xA0000
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x00000016, 0x00000009, 0xFFFFE000,             // Modify ScaleX by -0x2000
                0x00000016, 0x0000000A, 0x00002000,             // Modify ScaleY by 0x2000
                0x00000016, 0x00000006, 0xFFFC0000,             // Modify PositionY by -0x40000
                0x00000000, 0x00000002,                         // Wait 2 frame(s)
                0x80030000,                                     // (label 0x0030000)
                0x00000016, 0x00000009, 0x00002000,             // Modify ScaleX by 0x2000
                0x00000016, 0x0000000A, 0xFFFFE000,             // Modify ScaleY by -0x2000
                0x00000000, 0x00000003,                         // Wait 3 frame(s)
                0x0000000C, 0x00000002, 0xC0030000,             // Loop to 0xC0030000 2 time(s)
                0x00000016, 0x00000006, 0x00010000,             // Modify PositionY by 0x10000
                0x00000000, 0x00000003,                         // Wait 3 frame(s)
                0x00000016, 0x00000006, 0x00020000,             // Modify PositionY by 0x20000
                0x00000000, 0x00000002,                         // Wait 2 frame(s)
                0x00000016, 0x00000006, 0x00040000,             // Modify PositionY by 0x40000
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x00000016, 0x00000006, 0x00070000,             // Modify PositionY by 0x70000
                0x00000000, 0x00000002,                         // Wait 2 frame(s)
                0x0000000D, 0x00000000,                         // Goto 0x00
            }},
            { "Wander (Staggering)", new uint[] {
                0x00000015, 0x0000000F, 0x00010000,             // Set MaxVelocity to 0x10000
                0x00000015, 0x00000010, 0x00008000,             // Set Acceleration to 0x8000
                0x00000008, 0x00100000, 0x00100000, 0x00600000, // Wander between 0x100000 and 0x100000 units, max distance 0x600000 from home
                0x00000001,                                     // Wait until at move target
                0x00000015, 0x0000001E, 0x00000001,             // Set InteractFlags to 0x01
                0x00000016, 0x0000000A, 0xFFFFF800,             // Modify ScaleY by -0x800
                0x00000016, 0x00000009, 0xFFFFF000,             // Modify ScaleX by -0x1000
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x00000016, 0x00000009, 0x00001000,             // Modify ScaleX by 0x1000
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x00000016, 0x00000009, 0xFFFFF000,             // Modify ScaleX by -0x1000
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x00000016, 0x00000009, 0x00001000,             // Modify ScaleX by 0x1000
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x00000015, 0x00000009, 0x00010000,             // Set ScaleX to 0x10000
                0x00000015, 0x0000000A, 0x00010000,             // Set ScaleY to 0x10000
                0x0000000C, 0x0000FFFF, 0x00000000,             // Loop to 0x00 forever
            }},
            { "Teleport In (Spinning)", new uint[] {
                0x00000016, 0x00000006, 0xFFB33334,             // Modify PositionY by -0x4CCCCC
                0x00000016, 0x00000012, 0xFFB33334,             // Modify TargetPositionY by -0x4CCCCC
                0x00000015, 0x0000001A, 0x00000000,             // Set Unknown0x1A to 0x00
                0x80030000,                                     // (label 0x0030000)
                0x00000016, 0x00000009, 0xFFFFE667,             // Modify ScaleX by -0x1999
                0x00000016, 0x00000003, 0x00004000,             // Modify RotationY by 0x4000
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x0000000C, 0x00000008, 0xC0030000,             // Loop to 0xC0030000 8 time(s)
                0x80010000,                                     // (label 0x0010000)
                0x00000016, 0x00000006, 0x00099999,             // Modify PositionY by 0x99999
                0x00000016, 0x00000012, 0x00099999,             // Modify TargetPositionY by 0x99999
                0x00000016, 0x00000009, 0x00000CCC,             // Modify ScaleX by 0xCCC
                0x00000016, 0x0000000A, 0x00004CCC,             // Modify ScaleY by 0x4CCC
                0x00000016, 0x00000003, 0x00004000,             // Modify RotationY by 0x4000
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x0000000C, 0x00000008, 0xC0010000,             // Loop to 0xC0010000 8 time(s)
                0x80020000,                                     // (label 0x0020000)
                0x00000016, 0x00000009, 0x00000CCC,             // Modify ScaleX by 0xCCC
                0x00000016, 0x0000000A, 0xFFFFB334,             // Modify ScaleY by -0x4CCC
                0x00000016, 0x00000003, 0x00004000,             // Modify RotationY by 0x4000
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x0000000C, 0x00000008, 0xC0020000,             // Loop to 0xC0020000 8 time(s)
                0x00000010,
            }},
            { "Teleport In (Quick)", new uint[] {
                0x00000016, 0x00000006, 0xFF600000,             // Modify PositionY by -0xA00000
                0x00000015, 0x00000009, 0x00000000,             // Set ScaleX to 0x00
                0x00000015, 0x0000000A, 0x000A0000,             // Set ScaleY to 0xA0000
                0x00000015, 0x0000000B, 0x00000000,             // Set ScaleZ to 0x00
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x80010000,                                     // (label 0x0010000)
                0x00000016, 0x00000006, 0x00100000,             // Modify PositionY by 0x100000
                0x00000016, 0x00000009, 0x00001999,             // Modify ScaleX by 0x1999
                0x00000016, 0x0000000A, 0xFFFF0000,             // Modify ScaleY by -0x10000
                0x00000016, 0x0000000B, 0x00001999,             // Modify ScaleZ by 0x1999
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x0000000C, 0x00000009, 0xC0010000,             // Loop to 0xC0010000 9 time(s)
                0x00000016, 0x00000006, 0x00100000,             // Modify PositionY by 0x100000
                0x00000015, 0x00000009, 0x00010000,             // Set ScaleX to 0x10000
                0x00000015, 0x0000000A, 0x00010000,             // Set ScaleY to 0x10000
                0x00000015, 0x0000000B, 0x00010000,             // Set ScaleZ to 0x10000
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x00000010,                                     // Done
            }},
            { "Teleport Out (Stretch then Delete) (v1)", new uint[] {
                0x00000016, 0x0000000A, 0x00010000,             // Modify ScaleY by 0x10000
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x0000000C, 0x00000004, 0x00000000,             // Loop to 0x00 4 time(s)
                0x80010000,                                     // (label 0x0010000)
                0x00000016, 0x0000000A, 0xFFFF0000,             // Modify ScaleY by -0x10000
                0x00000016, 0x00000006, 0xFFCE0000,             // Modify PositionY by -0x320000
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x0000000C, 0x00000004, 0xC0010000,             // Loop to 0xC0010000 4 time(s)
                0x0000001B,                                     // Delete Self
                0x00000010,                                     // Done
            }},
            { "Teleport Out (Stretch then Delete) (v2)", new uint[] {
                0x00000016, 0x0000000A, 0x00010000,             // Modify ScaleY by 0x10000
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x0000000C, 0x00000004, 0x00000000,             // Loop to 0x00 4 time(s)
                0x80010000,                                     // (label 0x0010000)
                0x00000016, 0x0000000A, 0xFFFF0000,             // Modify ScaleY by -0x10000
                0x00000016, 0x00000006, 0xFFCE0000,             // Modify PositionY by -0x320000
                0x00000015, 0x00000012, 0x80000000,             // Set TargetPositionY to -0x80000000
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x0000000C, 0x00000004, 0xC0010000,             // Loop to 0xC0010000 4 time(s)
                0x0000001B,                                     // Delete Self
                0x00000010,                                     // Done
            }},
            { "Teleport Out (Stretch+Get thin then Move to Out-of-Bounds) (v1)", new uint[] {
                0x00000016, 0x00000006, 0xFFF00000,             // Modify PositionY by -0x100000
                0x00000016, 0x00000009, 0xFFFFE667,             // Modify ScaleX by -0x1999
                0x00000016, 0x0000000A, 0x00010000,             // Modify ScaleY by 0x10000
                0x00000016, 0x0000000B, 0xFFFFE667,             // Modify ScaleZ by -0x1999
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x0000000C, 0x00000009, 0x00000000,             // Loop to 0x00 9 time(s)
                0x00000015, 0x00000005, 0x10000000,             // Set PositionX to 0x10000000
                0x00000015, 0x00000006, 0x10000000,             // Set PositionY to 0x10000000
                0x00000015, 0x00000007, 0x10000000,             // Set PositionZ to 0x10000000
                0x00000015, 0x00000011, 0x80000000,             // Set TargetPositionX to -0x80000000
                0x00000015, 0x00000012, 0x80000000,             // Set TargetPositionY to -0x80000000
                0x00000015, 0x00000013, 0x80000000,             // Set TargetPositionZ to -0x80000000
                0x00000010,                                     // Done
            }},
            { "Teleport Out (Stretch+Get thin then Move to Out-of-Bounds) (v2)", new uint[] {
                0x00000016, 0x00000006, 0xFFF00000,             // Modify PositionY by -0x100000
                0x00000016, 0x00000009, 0xFFFFE667,             // Modify ScaleX by -0x1999
                0x00000016, 0x0000000A, 0x00010000,             // Modify ScaleY by 0x10000
                0x00000016, 0x0000000B, 0xFFFFE667,             // Modify ScaleZ by -0x1999
                0x00000015, 0x00000012, 0x80000000,             // Set TargetPositionY to -0x80000000
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x0000000C, 0x00000009, 0x00000000,             // Loop to 0x00 9 time(s)
                0x00000015, 0x00000005, 0x10000000,             // Set PositionX to 0x10000000
                0x00000015, 0x00000006, 0x10000000,             // Set PositionY to 0x10000000
                0x00000015, 0x00000007, 0x10000000,             // Set PositionZ to 0x10000000
                0x00000015, 0x00000011, 0x80000000,             // Set TargetPositionX to -0x80000000
                0x00000015, 0x00000012, 0x80000000,             // Set TargetPositionY to -0x80000000
                0x00000015, 0x00000013, 0x80000000,             // Set TargetPositionZ to -0x80000000
                0x00000010,                                     // Done
            }},
            { "Spin Slowly Forever (v1)", new uint[] {
                0x00000016, 0x00000003, 0x00000222,             // Modify RotationY by 0x222
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x0000000C, 0x0000FFFF, 0x00000000,             // Loop to 0x00 forever
            }},
            { "Spin Slowly Forever (v2)", new uint[] {
                0x00000016, 0x00000003, 0x00000200,             // Modify RotationY by 0x200
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x0000000C, 0x0000FFFF, 0x00000000,             // Loop to 0x00 forever
            }},
            { "Spin Around Very Fast Forever", new uint[] {
                0x00000016, 0x00000003, 0x00002000,             // Modify RotationY by 0x2000
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x0000000C, 0x0000FFFF, 0x00000000,             // Loop to 0x00 forever
            }},
            { "Slam Door Shut (Clockwise)", new uint[] {
                0x80010000,                                     // (label 0x0010000)
                0x00000016, 0x00000003, 0x00001000,             // Modify RotationY by 0x1000
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x0000000C, 0x00000004, 0xC0010000,             // Loop to 0xC0010000 4 time(s)
                0x0000001E, 0x00000217,                         // Play music/sound 0x217
                0x00000010,                                     // Done
            }},
            { "Slam Door Shut (Counter-Clockwise)", new uint[] {
                0x80010000,                                     // (label 0x0010000)
                0x00000016, 0x00000003, 0xFFFFF000,             // Modify RotationY by -0x1000
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x0000000C, 0x00000004, 0xC0010000,             // Loop to 0xC0010000 4 time(s)
                0x0000001E, 0x00000217,                         // Play music/sound 0x217
                0x00000010,                                     // Done
            }},
            { "Stuttering Grow and Shrink", new uint[] {
                0x00000016, 0x0000000A, 0x00001C00,             // Modify ScaleY by 0x1C00
                0x00000000, 0x00000004,                         // Wait 4 frame(s)
                0x00000016, 0x0000000A, 0xFFFFF000,             // Modify ScaleY by -0x1000
                0x00000000, 0x00000004,                         // Wait 4 frame(s)
                0x00000016, 0x0000000A, 0x00000800,             // Modify ScaleY by 0x800
                0x00000000, 0x00000004,                         // Wait 4 frame(s)
                0x00000016, 0x0000000A, 0xFFFFEC00,             // Modify ScaleY by -0x1400
                0x00000000, 0x0000000F,                         // Wait 15 frame(s)
                0x00000016, 0x0000000A, 0xFFFFEC00,             // Modify ScaleY by -0x1400
                0x00000000, 0x00000008,                         // Wait 8 frame(s)
                0x00000016, 0x0000000A, 0x00000800,             // Modify ScaleY by 0x800
                0x00000000, 0x00000006,                         // Wait 6 frame(s)
                0x00000016, 0x0000000A, 0xFFFFF800,             // Modify ScaleY by -0x800
                0x00000000, 0x00000004,                         // Wait 4 frame(s)
                0x00000016, 0x0000000A, 0x00001400,             // Modify ScaleY by 0x1400
                0x00000000, 0x00000032,                         // Wait 50 frame(s)
                0x0000000C, 0x0000FFFF, 0x00000000,             // Loop to 0x00 forever
            }},
            { "Look Around (Calmly)", new uint[] {
                0x00000016, 0x00000003, 0x00000200,             // Modify RotationY by 0x200
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x0000000C, 0x00000010, 0x00000000,             // Loop to 0x00 16 time(s)
                0x00000000, 0x00000014,                         // Wait 20 frame(s)
                0x80010000,                                     // (label 0x0010000)
                0x00000016, 0x00000003, 0xFFFFFC00,             // Modify RotationY by -0x400
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x0000000C, 0x00000010, 0xC0010000,             // Loop to 0xC0010000 16 time(s)
                0x00000000, 0x00000014,                         // Wait 20 frame(s)
                0x80020000,                                     // (label 0x0020000)
                0x00000016, 0x00000003, 0x00000200,             // Modify RotationY by 0x200
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x0000000C, 0x00000010, 0xC0020000,             // Loop to 0xC0020000 16 time(s)
                0x00000010,                                     // Done
            }},
            { "Look Around (Excitedly)", new uint[] {
                0x00000016, 0x00000003, 0x00004000,             // Modify RotationY by 0x4000
                0x00000000, 0x0000000A,                         // Wait 10 frame(s)
                0x00000016, 0x00000003, 0xFFFFC000,             // Modify RotationY by -0x4000
                0x00000000, 0x00000003,                         // Wait 3 frame(s)
                0x00000016, 0x00000003, 0xFFFFC000,             // Modify RotationY by -0x4000
                0x00000000, 0x0000000A,                         // Wait 10 frame(s)
                0x00000016, 0x00000003, 0x00004000,             // Modify RotationY by 0x4000
                0x00000000, 0x00000006,                         // Wait 6 frame(s)
                0x00000010,                                     // Done
            }},
            { "Slowly Turn Right (1/2 second)", new uint[] {
                0x00000016, 0x00000003, 0x00000222,             // Modify RotationY by 0x222
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x0000000C, 0x0000001E, 0x00000000,             // Loop to 0x00 30 time(s)
                0x00000010,                                     // Done
            }},
            { "Slowly Turn Left (1/2 second)", new uint[] {
                0x00000016, 0x00000003, 0xFFFFFDDE,             // Modify RotationY by -0x222
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x0000000C, 0x0000001E, 0x00000000,             // Loop to 0x00 30 time(s)
                0x00000010,                                     // Done
            }},
            { "Spin, Shrink and Delete Self", new uint[] {
                0x00000016, 0x00000003, 0x00000E38,             // Modify RotationY by 0xE38
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x0000000C, 0x00000008, 0x00000000,             // Loop to 0x00 8 time(s)
                0x80010000,                                     // (label 0x0010000)
                0x00000016, 0x00000003, 0x00001C71,             // Modify RotationY by 0x1C71
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x0000000C, 0x00000008, 0xC0010000,             // Loop to 0xC0010000 8 time(s)
                0x80020000,                                     // (label 0x0020000)
                0x00000016, 0x00000003, 0x00002AAA,             // Modify RotationY by 0x2AAA
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x0000000C, 0x00000008, 0xC0020000,             // Loop to 0xC0020000 8 time(s)
                0x80030000,                                     // (label 0x0030000)
                0x00000016, 0x00000003, 0x000038E3,             // Modify RotationY by 0x38E3
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x0000000C, 0x00000008, 0xC0030000,             // Loop to 0xC0030000 8 time(s)
                0x80040000,                                     // (label 0x0040000)
                0x00000016, 0x00000003, 0x0000471C,             // Modify RotationY by 0x471C
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x0000000C, 0x00000008, 0xC0040000,             // Loop to 0xC0040000 8 time(s)
                0x80050000,                                     // (label 0x0050000)
                0x00000016, 0x00000003, 0x00005555,             // Modify RotationY by 0x5555
                0x00000016, 0x00000009, 0xFFFFF000,             // Modify ScaleX by -0x1000
                0x00000016, 0x0000000A, 0xFFFFF000,             // Modify ScaleY by -0x1000
                0x00000016, 0x0000000B, 0xFFFFF000,             // Modify ScaleZ by -0x1000
                0x00000000, 0x00000001,                         // Wait 1 frame(s)
                0x0000000C, 0x00000010, 0xC0050000,             // Loop to 0xC0050000 16 time(s)
                0x0000001B,                                     // Delete Self
                0x00000010,                                     // Done
            }},
            { "Bounce Happily", new uint[] {
                0x00000015, 0x0000000F, 0x00010000,             // Set MaxVelocity to 0x10000
                0x00000015, 0x00000010, 0x00008000,             // Set Acceleration to 0x8000
                0x00000015, 0x0000001A, 0x00000003,             // Set Unknown0x1A to 0x03
                0x00000015, 0x00000015, 0x00008000,             // Set HoverMaxOrFallFelocity to 0x8000
                0x80010000,                                     // (label 0x0010000)
                0x00000008, 0x00100000, 0x00100000, 0x00600000, // Wander between 0x100000 and 0x100000 units, max distance 0x600000 from home
                0x00000015, 0x0000000D, 0xFFFC0000,             // Set Unknown0x0D to -0x40000
                0x00000000, 0x00000012,                         // Wait 18 frame(s)
                0x0000000C, 0x0000FFFF, 0xC0010000,             // Loop to 0xC0010000 forever
            }},
        };
    }
}
