using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommonLib.Arrays;
using CommonLib.Extensions;
using CommonLib.Rigging;
using SF3.Models.Files.X014;
using SF3.Models.Files.X8AN;
using SF3.Models.Files.X8PC;
using SF3.Models.Structs.X014;
using SF3.Models.Structs.X8AN;
using SF3.Models.Structs.X8PC;
using SF3.Models.Tables.X8PC;
using SF3.NamedValues;
using SF3.Types;
using static SF3.Editor.Forms.SF3EditorForm;
using static SF3.Utils.FileUtils;

namespace SF3.Editor.Utils {
    public static class X8AnimLoader {
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
            IX8AN_File? x8anFile = null;
            bool wasX014Loaded = false;
            bool wasX8anLoaded = false;

            try {
                var scenario = polyChar.Scenario;
                x014File = GetOrLoadX014File(loadedFiles, loadDir, scenario, out wasX014Loaded);
                if (x014File == null)
                    return null;

                var animLoadInfo = GetAnimLoadInfo(x014File, x8pcFileId, animId);
                if (animLoadInfo == null)
                    return null;

                x8anFile = GetOrLoadX8ANFile(loadedFiles, loadDir, scenario, animLoadInfo.AnimFileID, out wasX8anLoaded);
                if (x8anFile == null)
                    return null;

                return GetBoneKeyframesTableFromAnimLoadInfo(x8anFile, animLoadInfo);
            }
            finally {
                if (wasX014Loaded && x014File is X014_File x014File2)
                    x014File2.Dispose();
                if (wasX8anLoaded && x8anFile is X8AN_File x8anFile2)
                    x8anFile2.Dispose();
            }
        }

        private static ModelAnimLoadInfoSc1? GetAnimLoadInfo(IX014_File x014File, int x8pcFileId, PCAnimationType animId)
            => x014File.CharacterBattleModelAnimsSc1Table.FirstOrDefault(x => x.ModelFileID == x8pcFileId && x.AnimID == (int) animId);

        private static PCBoneKeyframesTable? GetBoneKeyframesTableFromAnimLoadInfo(IX8AN_File x8anFile, ModelAnimLoadInfoSc1 animLoadInfo) {
            var keyframes = ((IReadOnlyList<X8ANAttackAnimChunkStruct>) x8anFile.AttackAnimChunkTable).FirstOrDefault(x => x.Address / 0x800 == animLoadInfo.FilePosInSectors);
            return keyframes?.BoneKeyframesTable;
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
            var loadedX8anFiles = loadedFiles
                .Select(x => x.Loader.Model as IX8AN_File)
                .Where(x => x != null && x.Scenario == scenario && x.FileID == fileId)
                .Select(x => x!)
                .ToArray();
            if (loadedX8anFiles.Length == 1)
                return loadedX8anFiles[0];

            // No single file found, so try to load from the filesystem.
            try {
                var x8anFilename = GetFilenameFromFileID(scenario, fileId);
                var targetPath = Path.Combine(loadDir, x8anFilename);
                if (!File.Exists(targetPath))
                    return null;
                var x8anFile = X8AN_File.Create(new ByteData.ByteData(new ByteArray(File.ReadAllBytes(targetPath))), new NameGetterContext(scenario), scenario, fileId);
                wasLoadedOut = true;
                return x8anFile;
            }
            catch {
                return null;
            }
        }

        /// <summary>
        /// Attempts to load all missing X8PC animations. Returns a report as a string array.
        /// </summary>
        /// <param name="loadedFiles">Collection of files already loaded in SF3Editor.</param>
        /// <param name="loadDir">Directory to look for files that need to be loaded on-demand.</param>
        /// <param name="x8pc">X8PC file whose animations need loading.</param>
        /// <returns>An array of messages you might want to log or report.</returns>
        public static string[] LoadAllAnimationsForX8PC(IEnumerable<LoadedFile> loadedFiles, string loadDir, IX8PC_File x8pc) {
            if (!x8pc.FileID.HasValue)
                return ["Error: Can't determine FileID of current X8PC file which is required for matching X8AN file. Aborting."];
            var x8pcFileId = x8pc.FileID.Value;

            var report = new List<string>();

            IX014_File? x014File = null;
            bool x014FileLoaded = false;
            bool triedLoadingX014 = false;

            var x8anFiles = new Dictionary<int, IX8AN_File>();
            var x8anFilesLoaded = new Dictionary<int, bool>();
            var triedLoadingX8anFiles = new HashSet<int>();

            string? GetX014File() {
                if (x014File != null)
                    return null;

                try {
                    x014File = triedLoadingX014 ? null : GetOrLoadX014File(loadedFiles, loadDir, x8pc.Scenario, out x014FileLoaded);
                    return x014File == null ? $"Error: X014.BIN for {x8pc.Scenario} not loaded and couldn't be found at '{loadDir}'." : null;
                }
                catch (Exception e) {
                    return $"Error: Couldn't find or load X014.BIN:\r\n{e.GetTypeAndMessage()}";
                }
                finally {
                    triedLoadingX014 = true;
                }
            }

            string? GetAnimLoadInfo_(PCAnimationType animId, out ModelAnimLoadInfoSc1? loadInfoOut) {
                loadInfoOut = GetAnimLoadInfo(x014File!, x8pcFileId, animId);
                return (loadInfoOut == null) ? $"Error: Couldn't find animation load info for FileID 0x{x8pcFileId:X03}, animation {animId} (0x{(int) animId:X02})." : null;
            }

            string? GetX8ANFile(ModelAnimLoadInfoSc1 loadInfo, out IX8AN_File? x8anFileOut) {
                x8anFileOut = null;

                var animFileId = loadInfo.AnimFileID;
                var animFileName = GetFilenameFromFileID(x8pc.Scenario, animFileId);

                if (x8anFiles.TryGetValue(animFileId, out var x8anFileOutTry)) {
                    x8anFileOut = x8anFileOutTry;
                    return null;
                }

                try {
                    bool animFileLoaded = false;
                    var animFile = triedLoadingX8anFiles.Contains(animFileId) ? null : GetOrLoadX8ANFile(loadedFiles, loadDir, x8pc.Scenario, animFileId, out animFileLoaded);
                    if (animFile == null)
                        return $"Error: X8AN file 0x{animFileId:X03} ({animFileName}) for {x8pc.Scenario} not loaded and couldn't be found at '{loadDir}'.";
                    x8anFileOut = animFile;
                    x8anFiles.Add(animFileId, animFile);
                    x8anFilesLoaded.Add(animFileId, animFileLoaded);
                    return null;
                }
                catch (Exception e) {
                    return $"Error: Couldn't find or load file 0x{animFileId} ({animFileName}):\r\n{e.GetTypeAndMessage()}";
                }
                finally {
                    triedLoadingX8anFiles.Add(animFileId);
                }
            }

            try {
                // Technically only applies to an X8PC file with a single PolyChar, but... why not? Let's do a loop.
                foreach (var pc in x8pc.PolyCharTable) {
                    var animDefs = pc.AnimationChunkHeader.AnimationDefTable
                        .Where(x => x.IsInAttackAnimChunk && x.LoadedKeyframes == null)
                        .ToArray();
                    if (animDefs.Length == 0)
                        continue;

                    string? error;
                    if ((error = GetX014File()) != null) {
                        report.Add(error);
                        continue;
                    }

                    foreach (var animDef in animDefs) {
                        var animId = (PCAnimationType) animDef.AnimID;
                        if ((error = GetAnimLoadInfo_(animId, out var loadInfo)) != null) {
                            report.Add(error);
                            continue;
                        }
                        var animFileId = loadInfo!.AnimFileID;
                        var animFileName = GetFilenameFromFileID(x8pc.Scenario, animFileId);

                        if ((error = GetX8ANFile(loadInfo, out var x8anFile)) != null) {
                            report.Add(error);
                            continue;
                        }

                        var keyframes = GetBoneKeyframesTableFromAnimLoadInfo(x8anFile!, loadInfo);
                        if (keyframes == null) {
                            report.Add($"Error: X8AN file 0x{animFileId:X03} ({animFileName}) didn't contain keyframes for {animId} (0x{(int) animId:X02}) at sector 0x{loadInfo.FilePosInSectors:X02}");
                            continue;
                        }

                        animDef.SetLoadedKeyframes(keyframes);
                        report.Add($"Added keyframes for {animId} (0x{(int) animId:X02}) from X8AN file 0x{animFileId:X03} ({animFileName}) at sector 0x{loadInfo.FilePosInSectors:X02}");
                    }
                }
            }
            finally {
                if (x014File is X014_File x014File2 && x014FileLoaded)
                    x014File2.Dispose();
                foreach (var x8anKv in x8anFiles)
                    if (x8anFilesLoaded[x8anKv.Key] && x8anKv.Value is X8AN_File x8anFile)
                        x8anFile.Dispose();
            }

            return report.Count == 0 ? ["No animations to load."] : report.ToArray();
        }
    }
}
