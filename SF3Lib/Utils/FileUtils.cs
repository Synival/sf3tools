using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Files;
using SF3.Models.Files.MPD;
using SF3.Models.Files.X002;
using SF3.Models.Files.X005;
using SF3.Models.Files.X012;
using SF3.Models.Files.X013;
using SF3.Models.Files.X014;
using SF3.Models.Files.X019;
using SF3.Models.Files.X033_X031;
using SF3.Models.Files.X1;
using SF3.Types;

namespace SF3.Utils {
    public static class FileUtils {
        /// <summary>
        /// Attempts to guess at the type of file based on the filename.
        /// </summary>
        /// <param name="filename">The name of a SF3 file.</param>
        /// <returns>A file type if the type could be determined. Otherwise 'null'.</returns>
        public static SF3FileType? DetermineFileType(string filename)
            => DetermineFileType(filename, null);

        /// <summary>
        /// Attempts to guess at the type of file based on the filename and an optional file filter.
        /// </summary>
        /// <param name="filename">The name of a SF3 file.</param>
        /// <param name="filter">An optional file filter, such as "*.MPD" as provided by an "Open..." dialog.</param>
        /// <returns>A file type if the type could be determined. Otherwise 'null'.</returns>
        public static SF3FileType? DetermineFileType(string filename, string filter) {
            if (filename == null)
                throw new ArgumentNullException(nameof(filename));

            // If the filter is explicit, don't guess.
            if (filter != null) {
                switch (filter.ToUpper()) {
                    case "X1*.BIN": {
                        if (filename == "X1BTL99.BIN")
                            return SF3FileType.X1BTL99;
                        else
                            return SF3FileType.X1;
                    }

                    case "X1BTL99.BIN":
                        return SF3FileType.X1BTL99;
                    case "X002.BIN":
                        return SF3FileType.X002;
                    case "X005.BIN":
                        return SF3FileType.X005;
                    case "X012.BIN":
                        return SF3FileType.X012;
                    case "X013.BIN":
                        return SF3FileType.X013;
                    case "X014.BIN":
                        return SF3FileType.X014;
                    case "X019.BIN":
                        return SF3FileType.X019;
                    case "X031.BIN":
                        return SF3FileType.X031;
                    case "X033.BIN":
                        return SF3FileType.X033;
                    case "*.MPD":
                        return SF3FileType.MPD;
                }
            }

            // No explicit filter. Look at the filename and guess.
            var filenameUpper = filename.ToUpper();
            var preExtension = Path.GetFileNameWithoutExtension(filenameUpper);

            if (filenameUpper.Contains(".MPD"))
                return SF3FileType.MPD;
            else if (filenameUpper.Contains(".BIN")) {
                if (preExtension.Contains("X1BTL99"))
                    return SF3FileType.X1BTL99;
                else if (preExtension.Contains("X1"))
                    return SF3FileType.X1;
                else if (preExtension.Contains("X002"))
                    return SF3FileType.X002;
                else if (preExtension.Contains("X005"))
                    return SF3FileType.X005;
                else if (preExtension.Contains("X012"))
                    return SF3FileType.X012;
                else if (preExtension.Contains("X013"))
                    return SF3FileType.X013;
                else if (preExtension.Contains("X014"))
                    return SF3FileType.X014;
                else if (preExtension.Contains("X019"))
                    return SF3FileType.X019;
                else if (preExtension.Contains("X031"))
                    return SF3FileType.X031;
                else if (preExtension.Contains("X033"))
                    return SF3FileType.X033;
            }

            // Couldn't figure it out; it's unknown.
            return null;
        }

        /// <summary>
        /// Attempts to guess the scenario of the file based on the filename's full path.
        /// </summary>
        /// <param name="filename">The name of a SF3 file with its full path.</param>
        /// <returns>A scenario if it could be determined. Otherwise 'null'.</returns>
        public static ScenarioType? DetermineScenario(string filename) {
            if (filename == null)
                throw new ArgumentNullException(nameof(filename));

            // Determine the scenario based on the path.
            var directory = Path.GetDirectoryName(filename.ToUpper()).Replace('\\', '/');
            if (directory.EndsWith("/"))
                directory = directory.Substring(0, directory.Length - 1);
            var paths = directory.Split('/');
            if (paths.Length == 0)
                return null;

            // These paths are used for mods in the SF3 Patch Wizard. Those should be enough to know the scenario.
            switch (paths[paths.Length - 1]) {
                case "GS-9175":
                case "MK-81383":
                    return ScenarioType.Scenario1;
                case "GS-9188":
                    return ScenarioType.Scenario2;
                case "GS-9203":
                    return ScenarioType.Scenario3;
                case "6106979":
                    return ScenarioType.PremiumDisk;
            }

            // If the files are on a mounted drive, look at the drive volume label.
            if (paths[0].Length == 2 && paths[0][1] == ':') {
                var drivePath = paths[0][0] + ":" + Path.DirectorySeparatorChar;
                var drives = DriveInfo.GetDrives();
                var drive = drives.FirstOrDefault(x => x.Name == drivePath);
                if (drive != null) {
                    switch (drive.VolumeLabel) {
                        case "SHINING_FORCE_3_1":
                            return ScenarioType.Scenario1;
                        case "SHINING_FORCE_3_2":
                            return ScenarioType.Scenario2;
                        case "SHINING_FORCE_3_3":
                            return ScenarioType.Scenario3;
                        case "SHINING_FORCE_3_P":
                            return ScenarioType.PremiumDisk;
                    }
                }
            }

            // Couldn't figure it out; it's unknown.
            return null;
        }

        /// <summary>
        /// Factory function for creating an IBaseFile based on the SF3FileType.
        /// </summary>
        /// <param name="byteData">IByteData that contains the contexts of the file.</param>
        /// <param name="fileType">The type of file to be created.</param>
        /// <param name="nameGetterContexts">Dictionary with all INameGetterContext's. Necessary to provide all
        /// for files that can auto-determine their scenario.</param>
        /// <param name="scenario">The scenario for the file.</param>
        /// <returns>A newly-created IBaseFile of the type requested.</returns>
        /// <exception cref="InvalidOperationException">Thrown if 'fileType' is not supported.</exception>
        public static IBaseFile CreateFile(IByteData byteData, SF3FileType fileType, Dictionary<ScenarioType, INameGetterContext> nameGetterContexts, ScenarioType scenario) {
            var ngc = nameGetterContexts[scenario];
            switch (fileType) {
                case SF3FileType.X1:
                    return X1_File.Create(byteData, ngc, scenario, false);
                case SF3FileType.X1BTL99:
                    return X1_File.Create(byteData, ngc, scenario, true);
                case SF3FileType.X002:
                    return X002_File.Create(byteData, ngc, scenario);
                case SF3FileType.X005:
                    return X005_File.Create(byteData, ngc, scenario);
                case SF3FileType.X012:
                    return X012_File.Create(byteData, ngc, scenario);
                case SF3FileType.X013:
                    return X013_File.Create(byteData, ngc, scenario);
                case SF3FileType.X014:
                    return X014_File.Create(byteData, ngc, scenario);
                case SF3FileType.X019:
                    return X019_File.Create(byteData, ngc, scenario);
                case SF3FileType.X031:
                case SF3FileType.X033:
                    return X033_X031_File.Create(byteData, ngc, scenario);
                case SF3FileType.MPD:
                    return MPD_File.Create(byteData, nameGetterContexts);
                default:
                    throw new InvalidOperationException($"Unhandled file type '{fileType}'");
            }
        }
    }
}
