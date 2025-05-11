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
                case 0x04: return $"Modify target position by ({SignedHexStr(param[0], "X2")}, {SignedHexStr(param[1], "X2")}, {SignedHexStr(param[2], "X2")})";
                case 0x06: return $"Start moving to relative angle 0x{(short) param[0]:X4}, ahead 0x{param[1]}";
                case 0x08: return $"Wander between 0x{param[0]:X2} and 0x{param[1]:X2} units, max distance 0x{param[2]:X2} from home";
                case 0x09: return $"Wander (ignoring walls) between 0x{param[0]:X2} and 0x{param[1]:X2} units, max distance 0x{param[2]:X2} from home";
                case 0x0B: return "Move towards target actor";
                case 0x0C: return $"Loop to 0x{param[1]:X2} " + ((param[0] == 0xFFFF) ? "forever" : $"{param[0]} time(s)");
                case 0x0D: return $"Goto 0x{param[0]:X2}";
                case 0x10: return "Done";

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

                case 0x1B: return "Delete Self";
                case 0x1C: return $"Set animation to 0x{param[0]:X2}";
                case 0x1E: return $"Play music/sound 0x{param[0]:X3}";
                case 0x22: return $"Execute function 0x{param[0]:X8}";

                default:
                    return "??? (" + EnumNameOr(command, c => $"Command_0x{(int) command:X2}");
            }
        }
    }
}
