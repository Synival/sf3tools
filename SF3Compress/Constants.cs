using System.Collections.Generic;

namespace SF3Compress {
    public static class Constants {
        public const string Version = "0.1.0";

        public const string VersionString =
            "SF3Compress v" + Version + "\n";

        public const string ShortUsageString =
            "Usage:\n" +
            "  Compress a file or chunk with Shining Force 3's LZSS encoding:\n" +
            "    sf3compress [GENERAL_OPTIONS]... compress [OPTIONS]... <input file> <output file>\n" +
            "  Decompress a file or chunk in Shining Force 3's LZSS encoding:\n" +
            "    sf3compress [GENERAL_OPTIONS]... decompress [OPTIONS]... <input file> <output file>\n";

        public const string ErrorUsageString =
            "Try 'sf3compress --help' for more information.\n";

        public const string FullUsageString =
            ShortUsageString +
            "\n" +
            "General Options:\n" +
            "  -h, --help                print this help message\n" +
            "  --version                 print SF3Compress version\n" +
            "  -v, --verbose             verbose output\n" +
            "\n" +
            "'compress' Options:\n" +
            "  -o, --offset=<pos>        offset of data to compress in the file\n" +
            "                            (default is 0)\n" +
            "  -s, --size=<size>         amount of data to compress in the file\n" +
            "                            (default is until EOF)\n" +
            "\n" +
            "'decompress' Options:\n" +
            "  -o, --offset=<pos>        offset of data to decompress in the file\n" +
            "                            (default is 0)\n" +
            "  -s, --size=<size>         max amount of data to decompress in the file\n" +
            "                            (default is until ending code is reached)\n";

        public static readonly Dictionary<string, CommandType> CommandKeywords = new() {
            { "compress",   CommandType.Compress },
            { "decompress", CommandType.Decompress },
        };
    }
}
