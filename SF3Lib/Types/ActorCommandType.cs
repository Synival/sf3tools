using System;

namespace SF3.Types {
    public class ActorCommandParams : Attribute {
        public ActorCommandParams(params string[] @params) {
            Params = @params;
        }

        public string[] Params { get; }
    }

    public enum ActorCommandType {
        [ActorCommandParams("frames")] Wait = 0x00,
        [ActorCommandParams()] WaitUntilAtTargetPosition = 0x01,
        [ActorCommandParams("xPos", "yPos", "zPos")] SetPosition = 0x02,
        [ActorCommandParams("xPos", "yPos", "zPos")] SetTargetPosition = 0x03,
        [ActorCommandParams("unknown1", "unknown2", "unknown3")] UnknownMovementCommand0x04 = 0x04,
        [ActorCommandParams("unknown1", "unknown2", "unknown3")] UnknownMovementCommand0x05 = 0x05,
        [ActorCommandParams("turnAngle", "distance")] SetTargetPositionAhead = 0x06,
        [ActorCommandParams("angle", "distance")] SetTargetPositionInDirection = 0x07,
        [ActorCommandParams("dist1", "dist2", "maxFromHome")] WanderWithCollision = 0x08,
        [ActorCommandParams("dist1", "dist2", "maxFromHome")] WnaderWithoutCollision = 0x09,
        [ActorCommandParams()] UnknownMoveTowardsCommand0x0A = 0x0A,
        [ActorCommandParams()] MoveTowardTargetActor = 0x0B,
        [ActorCommandParams("times", "scriptPosition/label")] LoopNTimes = 0x0C,
        [ActorCommandParams("scriptPosition")] Goto = 0x0D,
        [ActorCommandParams("unknown")] Unknown0x0E = 0x0E,
        [ActorCommandParams("unknown")] Unknown0x0F = 0x0F,
        [ActorCommandParams()] Done = 0x10,
        [ActorCommandParams("unknown")] Unknown0x11 = 0x11,
        [ActorCommandParams("unknown")] Unknown0x12 = 0x12,
        [ActorCommandParams("unknown")] Unknown0x13 = 0x13,
        [ActorCommandParams("unknown")] Unknown0x14 = 0x14,
        [ActorCommandParams("property", "value")] SetProperty = 0x15,
        [ActorCommandParams("property", "value")] ModifyProperty = 0x16,
        [ActorCommandParams("unknown1", "unknown2")] Unknown0x17 = 0x17,
        [ActorCommandParams()] UnknownMovementCommand0x18 = 0x18,
        [ActorCommandParams()] UnknownMovementCommand0x19 = 0x19,
        [ActorCommandParams()] Unknown0x1A = 0x1A,
        [ActorCommandParams()] DeleteSelf = 0x1B,
        [ActorCommandParams("animation")] SetSpriteAnimation = 0x1C,
        [ActorCommandParams("unknown")] Unknown0x1D = 0x1D,
        [ActorCommandParams("music/sound")] PlayMusicOrSound = 0x1E,
        [ActorCommandParams("scriptAddr")] SwitchToScript = 0x1F, /* does it continue running it? */
        [ActorCommandParams()] Reserved0x20= 0x20,
        [ActorCommandParams()] Reserved0x21 = 0x21,
        [ActorCommandParams("functionAddr")] RunFunction = 0x22,
        [ActorCommandParams()] Reserved0x23 = 0x23,
        [ActorCommandParams()] Reserved0x24 = 0x24,
        [ActorCommandParams()] Reserved0x25 = 0x25,
        [ActorCommandParams()] Reserved0x26 = 0x26,
        [ActorCommandParams()] Reserved0x27 = 0x27,
        [ActorCommandParams()] Reserved0x28 = 0x28,
        [ActorCommandParams()] Reserved0x29 = 0x29,
        [ActorCommandParams()] Reserved0x2A = 0x2A,
        [ActorCommandParams()] Reserved0x2B = 0x2B,
        [ActorCommandParams()] Reserved0x2C = 0x2C,
        [ActorCommandParams()] Reserved0x2D = 0x2D,
        [ActorCommandParams()] Reserved0x2E = 0x2E,
    }
}
