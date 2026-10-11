using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommonLib.Arrays;
using CommonLib.Rigging;
using SF3.Models.Files.X014;
using SF3.Models.Files.X8AN;
using SF3.Models.Structs.X8AN;
using SF3.Models.Structs.X8PC;
using SF3.NamedValues;
using SF3.Types;
using static SF3.Editor.Forms.SF3EditorForm;
using static SF3.Utils.FileUtils;

namespace SF3.Editor.Utils {
    public class X8AnimLoader {
        /// <summary>
        /// Gets a set of bone animation for a Scenario 1 or Prototype PolyChar based on files already open or files at the path specified.
        /// Any non-loaded X014 and X8AN files are opened temporarily if they can be found.
        /// </summary>
        /// <param name="loadedFiles">Collection of files already loaded in SF3Editor.</param>
        /// <param name="loadDir">Directory to look for files that need to be loaded on-demand.</param>
        /// <param name="polyChar">PolyChar that needs an animation.</param>
        /// <param name="x8pcFileId">FileID of the X8PC file that needs to be loaded. Required for finding the appropiate X8AN file.</param>
        /// <param name="animId">Animation to be loaded.</param>
        /// <returns>A new table of bone keyframes on success, otherwise null.</returns>
        public static IReadOnlyList<IBoneKeyframe>? GetPolyCharAnimationFrromOpenFilesOrPath(IEnumerable<LoadedFile> loadedFiles, string loadDir, PolyChar polyChar, int x8pcFileId, PCAnimationType animId) {
            IX014_File? x014File = null;
            IX8AN_File? x8ANFile = null;
            bool wasX014Loaded = false;
            bool wasX8ANLoaded = false;

            try {
                var scenario = polyChar.Scenario;
                x014File = GetOrLoadX014File(loadedFiles, loadDir, scenario, out wasX014Loaded);
                if (x014File == null)
                    return null;

                var animToLoadRow = x014File.CharacterBattleModelAnimsSc1Table.FirstOrDefault(x => x.ModelFileID == x8pcFileId && x.AnimID == (int) animId);
                if (animToLoadRow == null)
                    return null;

                x8ANFile = GetOrLoadX8ANFile(loadedFiles, loadDir, scenario, animToLoadRow.AnimFileID, out wasX8ANLoaded);
                if (x8ANFile == null)
                    return null;

                var keyframes = ((IReadOnlyList<X8ANAttackAnimChunkStruct>) x8ANFile.AttackAnimChunkTable).FirstOrDefault(x => {
                    return (x.Address / 0x800) == animToLoadRow.FilePosInSectors;
                });
                return keyframes?.BoneKeyframesTable;
            }
            finally {
                if (wasX014Loaded && x014File is X014_File x014File2)
                    x014File2.Dispose();
                if (wasX8ANLoaded && x8ANFile is X8AN_File x8ANFile2)
                    x8ANFile2.Dispose();
            }
        }

        /// <summary>
        /// Fetches a single X014 file loaded if one is already loaded. If it's not loaded, it attempts to load it from a given path.
        /// If more than one X014 file for the requested scenario is currently loaded, it falls back to loading from the filesystem as the tie-breaker.
        /// </summary>
        /// <param name="loadedFiles">Collection of files already loaded in SF3Editor.</param>
        /// <param name="loadDir">Directory to look for files that need to be loaded on-demand.</param>
        /// <param name="scenario">Scenario of the file to be loaded.</param>
        /// <param name="wasLoadedOut">When set, the file returned was loaded from the filesystem.</param>
        /// <returns>A matching file that is already loaded or, if found, a matching file from the filesystem.</returns>
        public static IX014_File? GetOrLoadX014File(IEnumerable<LoadedFile> loadedFiles, string loadDir, ScenarioType scenario, out bool wasLoadedOut) {
            wasLoadedOut = false;

            // If one single matching file was loaded, use that one.
            var loadedX014Files = loadedFiles
                .Select(x => x.Loader.Model as IX014_File)
                .Where(x => x != null && x.Scenario == scenario)
                .Select(x => x!)
                .ToArray();
            if (loadedX014Files.Length == 1)
                return loadedX014Files[0];

            // No single file found, so try to load from the filesystem.
            try {
                var targetPath = Path.Combine(loadDir, "X014.BIN");
                if (!File.Exists(targetPath))
                    return null;
                var x014File = X014_File.Create(
                    new ByteData.ByteData(new ByteArray(File.ReadAllBytes(targetPath))), new NameGetterContext(scenario), scenario,
                    DetermineFileID(scenario, "X014.BIN")
                );
                wasLoadedOut = true;
                return x014File;
            }
            catch {
                return null;
            }
        }

        /// <summary>
        /// Fetches a single X8AN file with the requested FileID if one is already loaded. If it's not loaded, it attempts to load it from a given path.
        /// If more than one X8AN file for the requested scenario and FileID is currently loaded, it falls back to loading from the filesystem as the tie-breaker.
        /// </summary>
        /// <param name="loadedFiles">Collection of files already loaded in SF3Editor.</param>
        /// <param name="loadDir">Directory to look for files that need to be loaded on-demand.</param>
        /// <param name="scenario">Scenario of the file to be loaded.</param>
        /// <param name="wasLoadedOut">When set, the file returned was loaded from the filesystem.</param>
        /// <returns>A matching file that is already loaded or, if found, a matching file from the filesystem.</returns>
        public static IX8AN_File? GetOrLoadX8ANFile(IEnumerable<LoadedFile> loadedFiles, string loadDir, ScenarioType scenario, int fileId, out bool wasLoadedOut) {
            wasLoadedOut = false;

            // If one single matching file was loaded, use that one.
            var loadedX8ANFiles = loadedFiles
                .Select(x => x.Loader.Model as IX8AN_File)
                .Where(x => x != null && x.Scenario == scenario && x.FileID == fileId)
                .Select(x => x!)
                .ToArray();
            if (loadedX8ANFiles.Length == 1)
                return loadedX8ANFiles[0];

            // No single file found, so try to load from the filesystem.
            try {
                var x8ANFilename = GetFilenameFromFileID(scenario, fileId);
                var targetPath = Path.Combine(loadDir, x8ANFilename);
                if (!File.Exists(targetPath))
                    return null;
                var x8ANFle = X8AN_File.Create(new ByteData.ByteData(new ByteArray(File.ReadAllBytes(targetPath))), new NameGetterContext(scenario), scenario, fileId);
                wasLoadedOut = true;
                return x8ANFle;
            }
            catch {
                return null;
            }
        }
    }
}
