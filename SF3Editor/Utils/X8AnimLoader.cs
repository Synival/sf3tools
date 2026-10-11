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
        /// 
        /// </summary>
        /// <param name="loadedFiles">Collection of files already loaded in SF3Editor.</param>
        /// <param name="loadedFileDirectory">Directory to look for files that need to be loaded on-demand.</param>
        /// <param name="polyChar">PolyChar that needs an animation.</param>
        /// <param name="x8pcFileId">FileID of the X8PC file that needs to be loaded. Required for finding the appropiate X8AN file.</param>
        /// <param name="animId">Animation to be loaded.</param>
        /// <returns></returns>
        public static IReadOnlyList<IBoneKeyframe>? PolyCharKeyframeLoader(IEnumerable<LoadedFile> loadedFiles, string loadedFileDirectory, PolyChar polyChar, int x8pcFileId, PCAnimationType animId) {
            IX014_File? x014File = null;
            IX8AN_File? x8ANFile = null;
            bool isX014Temporary = false;
            bool isX8ANTemporary = false;

            try {
                var scenario = polyChar.Scenario;
                x014File = GetOrTemporarilyLoadX014File(loadedFiles, loadedFileDirectory, scenario, out isX014Temporary);
                if (x014File == null)
                    return null;

                var animToLoadRow = x014File.CharacterBattleModelAnimsSc1Table.FirstOrDefault(x => x.ModelFileID == x8pcFileId && x.AnimID == (int) animId);
                if (animToLoadRow == null)
                    return null;

                x8ANFile = GetOrTemporarilyLoadX8ANFile(loadedFiles, loadedFileDirectory, scenario, animToLoadRow.AnimFileID, out isX8ANTemporary);
                if (x8ANFile == null)
                    return null;

                var keyframes = ((IReadOnlyList<X8ANAttackAnimChunkStruct>) x8ANFile.AttackAnimChunkTable).FirstOrDefault(x => {
                    return (x.Address / 0x800) == animToLoadRow.FilePosInSectors;
                });
                return keyframes?.BoneKeyframesTable;
            }
            finally {
                if (isX014Temporary && x014File is X014_File x014File2)
                    x014File2.Dispose();
                if (isX8ANTemporary && x8ANFile is X8AN_File x8ANFile2)
                    x8ANFile2.Dispose();
            }
        }

        public static IX014_File? GetOrTemporarilyLoadX014File(IEnumerable<LoadedFile> loadedFiles, string loadedFileDirectory, ScenarioType scenario, out bool isTemporary) {
            isTemporary = false;
            var loadedX014Files = loadedFiles
                .Select(x => x.Loader.Model as IX014_File)
                .Where(x => x != null && x.Scenario == scenario)
                .Select(x => x!)
                .ToArray();

            if (loadedX014Files.Length == 1)
                return loadedX014Files[0];

            // Try to load the file!
            var targetPath = Path.Combine(loadedFileDirectory, "X014.BIN");
            if (!File.Exists(targetPath))
                return null;

            try {
                var x014File = X014_File.Create(
                    new ByteData.ByteData(new ByteArray(File.ReadAllBytes(targetPath))), new NameGetterContext(scenario), scenario,
                    DetermineFileID(scenario, "X014.BIN")
                );
                isTemporary = true;
                return x014File;
            }
            catch {
                return null;
            }
        }

        public static IX8AN_File? GetOrTemporarilyLoadX8ANFile(IEnumerable<LoadedFile> loadedFiles, string loadedFileDirectory, ScenarioType scenario, int fileId, out bool isTemporary) {
            isTemporary = false;

            var loadedX8ANFiles = loadedFiles
                .Select(x => x.Loader.Model as IX8AN_File)
                .Where(x => x != null && x.Scenario == scenario && x.FileID == fileId)
                .Select(x => x!)
                .ToArray();

            if (loadedX8ANFiles.Length == 1)
                return loadedX8ANFiles[0];

            // Try to load the file!
            var x8ANFilename = GetFilenameFromFileID(scenario, fileId);
            var targetPath = Path.Combine(loadedFileDirectory, x8ANFilename);
            if (!File.Exists(targetPath))
                return null;

            try {
                var x8ANFle = X8AN_File.Create(new ByteData.ByteData(new ByteArray(File.ReadAllBytes(targetPath))), new NameGetterContext(scenario), scenario, fileId);
                isTemporary = true;
                return x8ANFle;
            }
            catch {
                return null;
            }
        }
    }
}
