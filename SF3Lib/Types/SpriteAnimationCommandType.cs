namespace SF3.Types {
    public enum SpriteAnimationCommandType {
        Frame             = 0x00, // Actually 0x00 through (inclusive) 0xF0, and 0xFC
        SetDirectionCount = 0xF1,
        Stop              = 0xF2,
        SetVerticalOffset = 0xF3, // Unused
                         // 0xF4 ??  Unused
                         // 0xF5 ??  Unused
        SetDuation        = 0xF6, // Unused
        // Nothing for 0xF7 through 0xFB.
        // 0xFC is broken, it's just a normal frame.
        PlaySound         = 0xFD, // Unused
        GotoFrameOffset   = 0xFE,
        GotoAnimation     = 0xFF,
    }

    public static class SpriteAnimationCommandTypeExtensions {
        public static bool IsEndingCommand(this SpriteAnimationCommandType cmd)
            => cmd == SpriteAnimationCommandType.Stop || cmd == SpriteAnimationCommandType.GotoFrameOffset || cmd == SpriteAnimationCommandType.GotoAnimation;

        public static bool NeedsParameter(this SpriteAnimationCommandType cmd)
            => cmd != SpriteAnimationCommandType.Stop;
    }
}
