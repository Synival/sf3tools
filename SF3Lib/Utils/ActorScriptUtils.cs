using System.Collections.Generic;
using SF3.Types;
using static CommonLib.Utils.EnumHelpers;
using static CommonLib.Utils.ValueUtils;

namespace SF3.Utils {
    public static class ActorScriptUtils {
        public static string GetCommentForCommand(ActorCommandType command, uint[] param) {
            switch ((int) command) {
                case 0x00: return $"Wait {param[0]} frame(s)";
                case 0x01: return "Wait until at move target";
                case 0x02: return $"Set position to ({SignedHexStr(param[0], "X2")}, {SignedHexStr(param[1], "X2")}, {SignedHexStr(param[2], "X2")})";
                case 0x03: return $"Set target position to ({SignedHexStr(param[0], "X2")}, {SignedHexStr(param[1], "X2")}, {SignedHexStr(param[2], "X2")})";
                case 0x04: return $"Set target position to ({SignedHexStr(param[0], "X2")}, {SignedHexStr(param[1], "X2")}, {SignedHexStr(param[2], "X2")}) away from current posittion";
                case 0x05: return $"UnknownMovementCommand0x05 (p1=0x{param[0]:X8}, p2=0x{param[1]:X8}, p3=0x{param[2]:X8})";
                case 0x06: return $"Turn by {SignedHexStr((short) param[0], "X4")} and set target position to 0x{param[1]} ahead";
                case 0x07: return $"Turn to 0x{param[0]:X4} degrees and set target position to 0x{param[1]} ahead";
                case 0x08: return $"Wander between 0x{param[0]:X2} and 0x{param[1]:X2} units, max distance 0x{param[2]:X2} from home";
                case 0x09: return $"Wander (ignoring walls) between 0x{param[0]:X2} and 0x{param[1]:X2} units, max distance 0x{param[2]:X2} from home";
                case 0x0A: return "UnknownMoveTowardsCommand0x0A";
                case 0x0B: return "Move towards target actor";
                case 0x0C: return $"Loop to 0x{param[1]:X2} " + ((param[0] == 0xFFFF) ? "forever" : $"{param[0]} time(s)");
                case 0x0D: return $"Goto 0x{param[0]:X2}";
                case 0x0E: return $"Unknown0x0E (p1=0x{param[0]:X8})";
                case 0x0F: return $"Unknown0x0F (p1=0x{param[0]:X8})";
                case 0x10: return "Done";
                case 0x11: return $"Unknown0x11 (p1=0x{param[0]:X8})";
                case 0x12: return $"Unknown0x12 (p1=0x{param[0]:X8})";
                case 0x13: return $"Unknown0x13 (p1=0x{param[0]:X8})";
                case 0x14: return $"Unknown0x14 (p1=0x{param[0]:X8})";

                case 0x15: {
                    var property = (ActorPropertyCommandType) param[0];
                    var propertyName = EnumNameOr(property, p => $"Unknown0x{(int) p:X2}");
                    return $"Set {propertyName} to {SignedHexStr(param[1], "X2")}";
                }

                case 0x16: {
                    var property = (ActorPropertyCommandType) param[0];
                    var propertyName = EnumNameOr(property, p => $"Unknown0x{(int) p:X2}");
                    return $"Modify {propertyName} by {SignedHexStr(param[1], "X2")}";
                }

                case 0x17: return $"Unknown0x17 (p1=0x{param[0]:X8}, p2=0x{param[1]:X8})";
                case 0x18: return "UnknownMovementCommand0x18";
                case 0x19: return "UnknownMovementCommand0x19";
                case 0x1A: return "Unknown0x1A";

                case 0x1B: return "Delete Self";
                case 0x1C: return $"Set animation to 0x{param[0]:X2}";
                case 0x1D: return $"Unknown0x1D (p1=0x{param[0]:X8})";
                case 0x1E: return $"Play music/sound 0x{param[0]:X3}";
                case 0x22: return $"Execute function 0x{param[0]:X8}";

                // Reserved commands
                case 0x1F:
                case 0x20:
                case 0x21:
                case 0x23:
                case 0x24:
                case 0x25:
                case 0x26:
                case 0x27:
                case 0x28:
                case 0x29:
                case 0x2A:
                case 0x2B:
                case 0x2C:
                case 0x2D:
                case 0x2E:
                    return $"Done ({command})";

                default:
                    return "??? (" + EnumNameOr(command, c => $"Command_0x{(int) command:X2}");
            }
        }

        public static bool CommandEndsScript(ActorCommandType command, uint[] param, int commandPos, Dictionary<uint, int> labelPositions) {
            switch ((int) command) {
                case 0x0C: {
                    var gotoLower = param[1] & ~0xF0000000u;
                    var gotoPos = ((param[1] & 0xF0000000u) == 0xC0000000u && (labelPositions.ContainsKey(gotoLower))) ? labelPositions[gotoLower] : (int) param[1];
                    return (param[0] == 0xFFFF) && gotoPos <= commandPos;
                }

                case 0x0D: {
                    var gotoLower = param[0] & ~0xF0000000u;
                    var gotoPos = ((param[0] & 0xF0000000u) == 0xC0000000u && (labelPositions.ContainsKey(gotoLower))) ? labelPositions[gotoLower] : (int) param[0];
                    return gotoPos <= commandPos;
                }

                case 0x10:
                    return true;

                default:
                    return false;
            }
        }

        public static bool CommandIsValid(ActorCommandType command, int commandsRead, uint[] param) {
            // Check for command logic / validity
            switch ((int) command) {
                case 0x00:
                    // Waiting 0 or thousands of frames probably isn't a thing
                    // Don't trust scripts that script with a 'wait' command
                    return param[0] != 0x0000 && param[0] < 1000 && commandsRead != 1;

                case 0x04:
                    // Common false positive
                    return param[2] != 0x10;

                case 0x0C:
                    // Don't trust loops with a count of zero
                    return param[0] != 0;

                case 0x1E:
                    // Probably not actually this command
                    return param[0] < 0x1000;

                default:
                    return true;
            }
        }
    }
}
