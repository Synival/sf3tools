using System;

namespace CHRTool {
    public static class UpdateLookupHashes {
        public static int Run(string[] args, string spriteDir, string spritesheetDir, string hashLookupDir) {
            // (any extra options would go here.)

            // There shouldn't be any unrecognized arguments at this point.
            if (args.Length > 0) {
                Console.Error.WriteLine("Unrecognized arguments in 'update-hashes' command:");
                Console.Error.Write($"    {string.Join(" ", args)}");
                Console.Error.Write(Constants.ErrorUsageString);
                return 1;
            }

            // It looks like we're ready to go! Fetch the file data.
            Console.WriteLine($"Sprite directory:      {spriteDir}");
            Console.WriteLine($"Spritesheet directory: {spritesheetDir}");
            Console.WriteLine($"Hash lookup directory: {hashLookupDir}");

            // TODO: finish this!!
            Console.WriteLine("Coming soon (tm)!");
            return 0;
        }
    }
}
