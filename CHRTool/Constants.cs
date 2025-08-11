using System.Collections.Generic;

namespace CHRTool {
    public static class Constants {
        public const string Version = "0.0.2";

        public const string VersionString =
            "CHRTool v" + Version + "\n";

        public const string ShortUsageString =
            "Usage:\n" +
            "  Compile .SF3CHR file into a .CHR file:\n" +
            "    chrtool [GENERAL_OPTIONS]... compile [OPTIONS]... SF3CHR_file\n" +
            "  Decompile a .CHR file into a .SF3CHR file:\n" +
            "    chrtool [GENERAL_OPTIONS]... decompile [OPTIONS]... CHR_file\n" +
            "  Extract spritesheet frames from .CHR and .CHP files:\n" +
            "    chrtool [GENERAL_OPTIONS]... extract-sheets [OPTIONS]... [game_data_dir/file]...\n" +
            "  Update the lookup table for frame and animation hashes in SF3Lib:\n" +
            "    chrtool [GENERAL_OPTIONS]... update-hash-lookups [OPTIONS]...\n";

        public const string ErrorUsageString =
            ShortUsageString +
            "Try 'chrtool --help' for more information.\n";

        public const string FullUsageString =
            ShortUsageString +
            "\n" +
            "General Options:\n" +
            "  -h, --help                print this help message\n" +
            "  --version                 print CHRTool version\n" +
            "  --sprite-dir=<dir>        directory for sprites (.SF3Sprite files)\n" +
            "      (default='<program-dir>/Resources/Sprites')\n" +
            "  --spritesheet-dir=<dir>   directory for spritesheets (.PNG files)\n" +
            "      (default='<program-dir>/Resources/Spritesheets')\n" +
            "  --frame-hash-lookups-file=<file>\n" +
            "      The file for lookup up frames based on hashes\n" +
            "      (default='<program-dir>/Resources/FrameHashLookups.json')\n" +
            "\n" +
            "'compile' Options:\n" +
            "  -O, --optimize            optimizes frames, ignoring anything explicit\n" +
            "  --output=<output-file>    specify output .CHR file\n" +
            "  --add-sprite=<file>       adds an SF3CHRSprite file\n" +
            "\n" +
            "'decompile' Options:\n" +
            "  --output=<output-file>    specify output .SF3CHR file\n" +
            "\n" +
            "'extract-sheets' Options:\n" +
            "  (none)\n" +
            "\n" +
            "'update-hash-lookups' Options:\n" +
            "  (none)\n";

        public static readonly Dictionary<string, CommandType> CommandKeywords = new() {
            { "compile",             CommandType.Compile },
            { "decompile",           CommandType.Decompile },
            { "extract-sheets",      CommandType.ExtractSheets },
            { "update-hash-lookups", CommandType.UpdateHashLookups },
        };
    }
}
