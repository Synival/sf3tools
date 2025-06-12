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
using SF3.Models.Files.X011;
using SF3.Models.Files.X012;
using SF3.Models.Files.X013;
using SF3.Models.Files.X014;
using SF3.Models.Files.X019;
using SF3.Models.Files.X021;
using SF3.Models.Files.X023;
using SF3.Models.Files.X024;
using SF3.Models.Files.X026;
using SF3.Models.Files.X027;
using SF3.Models.Files.X031;
using SF3.Models.Files.X033;
using SF3.Models.Files.X044;
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
            var filenameUpper = filename.ToUpper();
            if (filter != null) {
                var filterFileTypes = GetFileTypesForFileFilter(filter);
                if (filterFileTypes.Length == 1)
                    return filterFileTypes[0];

                // It looks like this filter is for more on than type; we have to guess a little.
                var allFileTypes = (SF3FileType[]) Enum.GetValues(typeof(SF3FileType));
                foreach (var ft in allFileTypes)
                    if (filterFileTypes.Contains(ft) && filenameUpper.Contains(ft.ToString()))
                        return ft;
            }

            // No explicit filter. Look at the filename and guess.
            var preExtension = Path.GetFileNameWithoutExtension(filenameUpper);
            if (filenameUpper.Contains(".MPD"))
                return SF3FileType.MPD;
            else if (filenameUpper.Contains(".BIN")) {
                     if (preExtension.Contains("X1BTL99")) return SF3FileType.X1BTL99;
                else if (preExtension.Contains("X1"))      return SF3FileType.X1;
                else if (preExtension.Contains("X002"))    return SF3FileType.X002;
                else if (preExtension.Contains("X005"))    return SF3FileType.X005;
                else if (preExtension.Contains("X011"))    return SF3FileType.X011;
                else if (preExtension.Contains("X012"))    return SF3FileType.X012;
                else if (preExtension.Contains("X013"))    return SF3FileType.X013;
                else if (preExtension.Contains("X014"))    return SF3FileType.X014;
                else if (preExtension.Contains("X019"))    return SF3FileType.X019;
                else if (preExtension.Contains("X021"))    return SF3FileType.X021;
                else if (preExtension.Contains("X023"))    return SF3FileType.X023;
                else if (preExtension.Contains("X024"))    return SF3FileType.X024;
                else if (preExtension.Contains("X026"))    return SF3FileType.X026;
                else if (preExtension.Contains("X027"))    return SF3FileType.X027;
                else if (preExtension.Contains("X031"))    return SF3FileType.X031;
                else if (preExtension.Contains("X033"))    return SF3FileType.X033;
                else if (preExtension.Contains("X044"))    return SF3FileType.X044;
            }

            // Couldn't figure it out; it's unknown.
            return null;
        }

        /// <summary>
        /// Attempts to guess the scenario of the file based on the filename's full path.
        /// </summary>
        /// <param name="filename">The name of a SF3 file with its full path.</param>
        /// <param name="fileType">The type of file, if available. Some files, like MPD files, are detectable.</param>
        /// <returns>A scenario if it could be determined. Otherwise 'null'.</returns>
        public static ScenarioType? DetermineScenario(string filename, SF3FileType? fileType = null) {
            // MPD's are automatically detected; default to Scenario 1.
            if (fileType == SF3FileType.MPD)
                return ScenarioType.Scenario1;

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
                case SF3FileType.X1:      return X1_File  .Create(byteData, ngc, scenario, false);
                case SF3FileType.X1BTL99: return X1_File  .Create(byteData, ngc, scenario, true);
                case SF3FileType.X002:    return X002_File.Create(byteData, ngc, scenario);
                case SF3FileType.X005:    return X005_File.Create(byteData, ngc, scenario);
                case SF3FileType.X011:    return X011_File.Create(byteData, ngc, scenario);
                case SF3FileType.X021:    return X021_File.Create(byteData, ngc, scenario);
                case SF3FileType.X023:    return X023_File.Create(byteData, ngc, scenario);
                case SF3FileType.X024:    return X024_File.Create(byteData, ngc, scenario);
                case SF3FileType.X026:    return X026_File.Create(byteData, ngc, scenario);
                case SF3FileType.X027:    return X027_File.Create(byteData, ngc, scenario);
                case SF3FileType.X012:    return X012_File.Create(byteData, ngc, scenario);
                case SF3FileType.X013:    return X013_File.Create(byteData, ngc, scenario);
                case SF3FileType.X014:    return X014_File.Create(byteData, ngc, scenario);
                case SF3FileType.X019:    return X019_File.Create(byteData, ngc, scenario);
                case SF3FileType.X031:    return X031_File.Create(byteData, ngc, scenario);
                case SF3FileType.X033:    return X033_File.Create(byteData, ngc, scenario);
                case SF3FileType.X044:    return X044_File.Create(byteData, ngc, scenario);
                case SF3FileType.MPD:     return MPD_File .Create(byteData, nameGetterContexts);
                default:
                    throw new InvalidOperationException($"Unhandled file type '{fileType}'");
            }
        }

        public static string GetFileFilterNameForFileType(SF3FileType type) {
            switch (type) {
                case SF3FileType.X1:      return "X1 Files";
                case SF3FileType.X1BTL99: return "X1BTL99 File";
                case SF3FileType.X002:    return "X002 File";
                case SF3FileType.X005:    return "X005 File";
                case SF3FileType.X011:    return "X011 File";
                case SF3FileType.X012:    return "X012 File";
                case SF3FileType.X013:    return "X013 File";
                case SF3FileType.X014:    return "X014 File";
                case SF3FileType.X019:    return "X019 File";
                case SF3FileType.X021:    return "X021 File";
                case SF3FileType.X023:    return "X023 File";
                case SF3FileType.X024:    return "X024 File";
                case SF3FileType.X026:    return "X026 File";
                case SF3FileType.X027:    return "X027 File";
                case SF3FileType.X031:    return "X031 File";
                case SF3FileType.X033:    return "X033 File";
                case SF3FileType.X044:    return "X044 File";
                case SF3FileType.MPD:     return "MPD Files";
                default:
                    throw new ArgumentException($"Unhandled value '{type}' for '{nameof(type)}'");
            }
        }

        public static string GetFileFilterForFileType(SF3FileType type) {
            switch (type) {
                case SF3FileType.X1:      return "*X1*.BIN";
                case SF3FileType.X1BTL99: return "*X1BTL99*.BIN";
                case SF3FileType.X002:    return "*X002*.BIN";
                case SF3FileType.X005:    return "*X005*.BIN";
                case SF3FileType.X011:    return "*X011*.BIN";
                case SF3FileType.X012:    return "*X012*.BIN";
                case SF3FileType.X013:    return "*X013*.BIN";
                case SF3FileType.X014:    return "*X014*.BIN";
                case SF3FileType.X019:    return "*X019*.BIN";
                case SF3FileType.X021:    return "*X021*.BIN";
                case SF3FileType.X023:    return "*X023*.BIN";
                case SF3FileType.X024:    return "*X024*.BIN";
                case SF3FileType.X026:    return "*X026*.BIN";
                case SF3FileType.X027:    return "*X027*.BIN";
                case SF3FileType.X031:    return "*X031*.BIN";
                case SF3FileType.X033:    return "*X033*.BIN";
                case SF3FileType.X044:    return "*X044*.BIN";
                case SF3FileType.MPD:     return "*.MPD";
                default:
                    throw new ArgumentException($"Unhandled value '{type}' for '{nameof(type)}'");
            }
        }

        public static SF3FileType[] GetFileTypesForFileFilter(string filter) {
            switch (filter) {
                case "*X1BTL99*.BIN": return new SF3FileType[] { SF3FileType.X1BTL99 };
                case "*X1*.BIN":      return new SF3FileType[] { SF3FileType.X1 };
                case "*X002*.BIN":    return new SF3FileType[] { SF3FileType.X002 };
                case "*X005*.BIN":    return new SF3FileType[] { SF3FileType.X005 };
                case "*X011*.BIN":    return new SF3FileType[] { SF3FileType.X011 };
                case "*X012*.BIN":    return new SF3FileType[] { SF3FileType.X012 };
                case "*X013*.BIN":    return new SF3FileType[] { SF3FileType.X013 };
                case "*X014*.BIN":    return new SF3FileType[] { SF3FileType.X014 };
                case "*X019*.BIN":    return new SF3FileType[] { SF3FileType.X019 };
                case "*X021*.BIN":    return new SF3FileType[] { SF3FileType.X021 };
                case "*X023*.BIN":    return new SF3FileType[] { SF3FileType.X023 };
                case "*X024*.BIN":    return new SF3FileType[] { SF3FileType.X024 };
                case "*X026*.BIN":    return new SF3FileType[] { SF3FileType.X026 };
                case "*X027*.BIN":    return new SF3FileType[] { SF3FileType.X027 };
                case "*X031*.BIN":    return new SF3FileType[] { SF3FileType.X031 };
                case "*X033*.BIN":    return new SF3FileType[] { SF3FileType.X033 };
                case "*X044*.BIN":    return new SF3FileType[] { SF3FileType.X044 };
                case "*.MPD":         return new SF3FileType[] { SF3FileType.MPD };
                default:
                    return new SF3FileType[] {};
            }
        }

        public static string GetFileDialogFilterForFileType(SF3FileType type) {
            var name = GetFileFilterNameForFileType(type);
            var filter = GetFileFilterForFileType(type);
            return $"{name} ({filter})|{filter}";
        }
    }
}
