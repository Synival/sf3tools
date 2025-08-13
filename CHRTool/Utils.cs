using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CHRTool {
    public static class Utils {
        public static (string[] outFiles, string[] outArgs) GetFilesAndPathsFromAgs(string[] args, Func<string, string[]> pathSearchFunc) {
            var results = new List<string>();

            var outArgs = args.Where(x => x.StartsWith("-")).ToArray();
            foreach (string arg in args.Where(x => !x.StartsWith("-"))) {
                if (Directory.Exists(arg))
                    results.AddRange(pathSearchFunc(arg));
                else
                    results.Add(arg);
            }
            return (results.ToArray(), outArgs);
        }
    }
}
