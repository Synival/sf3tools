using System.Collections.Generic;

namespace CHRTool {
    public static class Constants {
        public const string Version = "1.1.2";

        public const string VersionString =
            "CHRTool v" + Version + "\n";

        public const string ShortUsageString =
            "Usage:\n" +
            "  Compile SF3CHR/SF3CHP file into a CHR/CHP file:\n" +
            "    chrtool [GENERAL_OPTIONS]... compile [OPTIONS]... [SF3CHR/SF3CHP file/path]...\n" +
            "  Decompile a CHR/CHP file into a SF3CHR/SF3CHP file:\n" +
            "    chrtool [GENERAL_OPTIONS]... decompile [OPTIONS]... [CHR/CHP file/path]...\n" +
            "  Extract spritesheet frames from CHR/CHP files:\n" +
            "    chrtool [GENERAL_OPTIONS]... extract-sheets [OPTIONS]... [CHR/CHP file/path]...\n" +
            "  Update the lookup table for frame and animation hashes in SF3Lib:\n" +
            "    chrtool [GENERAL_OPTIONS]... update-hash-lookups [OPTIONS]...\n" +
            "  Show brief or verbose (-v) contents of an SF3CHR, SF3CHP, CHR, CHP, or SF3Sprite\n" +
            "    chrtool [GENERAL_OPTIONS]... describe [OPTIONS]... [file/path]...\n";

        public const string ErrorUsageString =
            "Try 'chrtool --help' for more information.\n";

        public const string FullUsageString =
            ShortUsageString +
            "\n" +
            "General Options:\n" +
            "  -h, --help                print this help message\n" +
            "  --version                 print CHRTool version\n" +
            "  -v, --verbose             verbose output\n" +
            "  --sprite-dir=<dir>        directory for sprites (SF3Sprite files)\n" +
            "      (default='<program-dir>/Resources/Sprites')\n" +
            "  --spritesheet-dir=<dir>   directory for spritesheets (PNG files)\n" +
            "      (default='<program-dir>/Resources/Spritesheets')\n" +
            "  --frame-hash-lookups-file=<file>\n" +
            "      the file for lookup up frames based on hashes\n" +
            "      (default='<program-dir>/Resources/FrameHashLookups.json')\n" +
            "\n" +
            "'compile' Options:\n" +
            "  -O, --optimize            optimizes frames, ignoring anything explicit\n" +
            "  -o, --output=<file>       specify output file for one CHR/CHP file\n" +
            "      (can' be used with multiple files or with paths)\n" +
            "  -d, --output-dir=<dir>    specify output path for all CHR/CHP file(s)\n" +
            "  --add-sprite=<file>       adds an SF3CHRSprite file\n" +
            "  --padding-from=<file>     specify a file to use when adding padding\n" +
            "                            (useful for the junk data in vanilla files)\n" +
            "\n" +
            "'decompile' Options:\n" +
            "  -S --simplify             ignore accuracy and simplify output\n" +
            "  -o, --output=<file>       specify output file for one SF3CHR/SF3CHP file\n" +
            "      (can' be used with multiple files or with paths)\n" +
            "  -d, --output-dir=<dir>    specify output path for all SF3CHR/SF3CHP file(s)\n" +
            "\n" +
            "'extract-sheets' Options:\n" +
            "  (none)\n" +
            "\n" +
            "'update-hash-lookups' Options:\n" +
            "  -r, --replace             replaces the entire file rather than only update\n" +
            "\n" +
            "'describe' Options:\n" +
            "  (none)\n";

        public static readonly Dictionary<string, CommandType> CommandKeywords = new() {
            { "compile",             CommandType.Compile },
            { "decompile",           CommandType.Decompile },
            { "extract-sheets",      CommandType.ExtractSheets },
            { "update-hash-lookups", CommandType.UpdateHashLookups },
            { "describe",            CommandType.Describe },
        };
    }
}
