namespace CHRTool {
    public static class Constants {
        public const string Version = "0.1";

        public const string VersionString =
            "CHRTool v" + Version + "\n";

        public const string ShortUsageString =
            "Usage:\n" +
            "  chrtool [GENERAL_OPTIONS]... compile [OPTIONS]... SF3CHR_file\n" +
            "  chrtool [GENERAL_OPTIONS]... decompile [OPTIONS]... CHR_file\n" +
            "  chrtool [GENERAL_OPTIONS]... extract-sheets [OPTIONS]... game_data_dir\n";

        public const string ErrorUsageString =
            ShortUsageString +
            "Try 'chrtool --help' for more information.\n";

        public const string FullUsageString =
            ShortUsageString +
            "\n" +
            "Compiles or decompiles CHR files from/to SF3CHR files.\n" +
            "\n" +
            "General Options:\n" +
            "  -h, --help                print this help message\n" +
            "  --version                 print CHRTool version\n" +
            "  --sprite-dir=<dir>        directory for sprites (.SF3Sprite files)\n" +
            "      (default='<program-dir>/Resources/Sprites')\n" +
            "  --spritesheet-dir=<dir>   directory for spritesheets (.PNG files)\n" +
            "      (default='<program-dir>/Resources/Spritesheets')\n" +
            "\n" +
            "Compile Options:\n" +
            "  -O, --optimize            optimizes frames, ignoring anything explicit\n" +
            "  --output=<output-file>    specify output .CHR file\n" +
            "  --add-sprite=<file>       adds an SF3CHRSprite file\n" +
            "\n" +
            "Decompile Options:\n" +
            "  --output=<output-file>    specify output .SF3CHR file\n" +
            "\n" +
            "Extract Sheets Options:\n" +
            "  (none)\n" +
            "\n";
    }
}
