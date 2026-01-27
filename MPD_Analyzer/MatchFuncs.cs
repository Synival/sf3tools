using CommonLib.Arrays;
using CommonLib.Imaging;
using SF3.ByteData;
using SF3.Imaging;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD.Animation;
using SF3.MPD.Interfaces;
using SF3.MPD.Writer;
using SF3.Types;
using SF3.Utils;

namespace MPD_Analyzer {
    public static class MatchFuncs {
        public static readonly Dictionary<string, HashSet<(string Hash, int ID)>> s_texturesByFile = [];
        public static readonly Dictionary<string, HashSet<(string Hash, int ID)>> s_referencedTexturesByFile = [];
        public static readonly Dictionary<string, HashSet<(string Hash, int ID)>> s_unreferencedTexturesByFile = [];
        public static string[,]? s_bochiTextureHashes = null;

        public static string[]? GetDuplicatedTextures(MPD_File mpdFile) {
            var duplicatedTextures = mpdFile.ModelCollections[MPD_CollectionType.Primary].Textures.GroupBy(x => x.Hash).Where(x => x.Count() > 1).Select(x => x.ToArray()).ToArray();
            if (duplicatedTextures.Length == 0)
                return null;
            return duplicatedTextures.Select(x => x[0].Hash + ": " + string.Join(", ", x.Select(y => $"0x{y.ID:X2}"))).ToArray();
        }

        public static string[]? GetAnimationsWithDifferentFirstFrameThanAssignedTexture(MPD_File mpdFile, Dictionary<int, IMPD_AnimatableTexture> texturesById) {
            if (mpdFile.Animations == null)
                return null;
            if (mpdFile.Animations.GroupBy(x => x.TextureID).Any(x => x.Count() > 1))
                return ["Has duplicate animations!!"];

            var firstFrameByTexId = mpdFile.Animations.Where(x => x.NumFrames > 0).ToDictionary(x => x.TextureID, x => (ITextureData) x.AnimationFrameTable.First());
            var nonMatchingTextures = texturesById.Values.Where(x => firstFrameByTexId.ContainsKey(x.ID) && x.Hash != firstFrameByTexId[x.ID].Hash).ToArray();
            return nonMatchingTextures.Select(x => $"Tex0x{x.ID:X2}: Expected '{texturesById[x.ID].Hash}', was '{firstFrameByTexId[x.ID].Hash}'").ToArray();
        }

        public static string[]? GetAnimationsWithAssignedTextureMissingFromAnimation(MPD_File mpdFile, Dictionary<int, IMPD_AnimatableTexture> texturesById) {
            if (mpdFile.Animations == null)
                return null;
            if (mpdFile.Animations.GroupBy(x => x.TextureID).Any(x => x.Count() > 1))
                return ["Has duplicate animations!!"];

            var framesByTexId = mpdFile.Animations.ToDictionary(x => x.TextureID, x => (ITextureData[]) x.AnimationFrameTable.ToArray());
            var nonMatchingTextures = texturesById.Values.Where(x => framesByTexId.ContainsKey(x.ID) && !framesByTexId[x.ID].Any(y => y.Hash == x.Hash)).ToArray();
            return nonMatchingTextures.Select(x => $"Tex0x{x.ID:X2}: Expected '{texturesById[x.ID].Hash}' to be in animation").ToArray();
        }

        public static string[]? GetModelsWithDuplicateInternalTextures(Dictionary<int, IMPD_AnimatableTexture> texturesById, Dictionary<int, IMPD_Model> modelsById) {
            var allModelsWithDuplicateTexturesInternally = modelsById
                .ToDictionary(x => x.Value, x => x.Value.Faces
                    .Select((x, i) => (Face: x, FaceIndex: i))
                    .Where(y => y.Face.Attributes.UseTexture)
                    .GroupBy(y => y.Face.Attributes.TextureNo)
                    .ToDictionary(y => y.Key, y => y.Select(z => (z.Face, z.FaceIndex, Texture: texturesById[z.Face.Attributes.TextureNo])).ToArray())
                    .GroupBy(y => y.Value.First().Texture.Hash)
                    .Where(y => y.Count() > 1)
                    .ToDictionary(y => y.Key, y => y.ToDictionary())
                )
                .Where(x => x.Value.Count > 0)
                .ToDictionary();

            if (allModelsWithDuplicateTexturesInternally.Count == 0)
                return null;

            return allModelsWithDuplicateTexturesInternally
                .Select(x => $"Model 0x{x.Key.ModelID:X2}:\r\n  " + string.Join("\r\n  ", x.Value
                    .Select(y => $"{y.Key}: " + string.Join("; ", y.Value
                        .Select(z => $"Tex0x{z.Key:X2} (Faces: " + string.Join(",", z.Value.Select(a => $"0x{a.FaceIndex:X2}")) + ")")
                    ))
                )).ToArray();
        }

        public static string[]? GetTexturesSharedBetweenModels(Dictionary<int, IMPD_Model> modelsById) {
            var texturesUsedByModel = modelsById.Values
                .ToDictionary(x => x.ModelID, x => x.Faces
                    .Where(x => x.Attributes.UseTexture)
                    .Select(x => x.Attributes.TextureNo)
                    .Distinct()
                    .ToHashSet()
            );
            var allTexturesUsedInModels = texturesUsedByModel.SelectMany(x => x.Value).Distinct().ToHashSet();

            var texturesSharedBetweenModels = allTexturesUsedInModels
                .ToDictionary(x => x, x => texturesUsedByModel.Where(y => y.Value.Contains(x)).Select(y => modelsById[y.Key]).ToArray())
                .Where(x => x.Value.Length > 1)
                .ToDictionary();

            if (texturesSharedBetweenModels.Count == 0)
                return null;
            return texturesSharedBetweenModels.Select(x => $"Tex0x{x.Key:X2}: " + string.Join(", ", x.Value.Select(y => $"Model0x{y.ModelID:X2}"))).ToArray();
        }

        public static string[]? GetTexturesUsedInBothModelsAndSurfaceModel(MPD_File mpdFile, Dictionary<int, IMPD_AnimatableTexture> texturesById) {
            if (!mpdFile.Surface.HasModel)
                return null;
            var surfaceMapTextures = mpdFile.Surface.GetAllTiles().Select(x => (int) x.TextureID).Distinct().Where(x => x != 0xFF).ToHashSet();
            var modelTextures = mpdFile.ModelCollections[MPD_CollectionType.Primary].Models.SelectMany(x => x.Faces.Select(y => (int) y.Attributes.TextureNo)).Distinct().ToHashSet();

            var texturesInBoth = surfaceMapTextures.Where(modelTextures.Contains).Select(x => texturesById[x]).ToArray();
            if (texturesInBoth.Length == 0)
                return null;
            return texturesInBoth.Select(x => x.Hash + $": 0x{x.ID:X2}").ToArray();
        }

        public static string[]? GetDifferentTexturesBetweenBochiAndBochiM(MPD_File mpdFile, string filename, Dictionary<int, IMPD_AnimatableTexture> texturesById) {
            if (filename != "BOCHI" && filename != "BOCHIM")
                return null;

            bool setting = s_bochiTextureHashes == null;
            if (s_bochiTextureHashes == null)
                s_bochiTextureHashes = new string[64, 64];

            var results = new List<string>();
            foreach (var tile in mpdFile.Surface.GetAllTiles()) {
                var texture = (tile.TextureID == 0xFF) ? null : texturesById[tile.TextureID];
                var hash = texture?.Hash ?? "(none)";
                if (setting)
                    s_bochiTextureHashes[tile.X, tile.Y] = hash;
                else if (s_bochiTextureHashes[tile.X, tile.Y] != hash)
                    results.Add($"Different texture at ({tile.X}, {tile.Y})");
            }

            return results.ToArray();
        }

        public static string[]? GetAllTextureSurfaceTileAppearances(MPD_File mpdFile, Dictionary<int, IMPD_AnimatableTexture> texturesById) {
            if (!mpdFile.Surface.HasModel)
                return null;
            var surfaceMapTextures = mpdFile.Surface
                .GetAllTiles()
                .Where(x => x.TextureID != 0xFF)
                .GroupBy(x => x.TextureID)
                .OrderBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.OrderBy(y => y.X).ThenBy(y => y.Y).ToArray());

            return surfaceMapTextures
                .Select(x => $"Tex0x{x.Key:X2} ({texturesById[x.Key].Hash}): " + string.Join(", ", x.Value.Select(y => $"({y.X},{y.Y})"))).ToArray();
        }

        public static string[]? GetAllUnusedTextures(MPD_File mpdFile, string filename, Dictionary<int, IMPD_AnimatableTexture> texturesById, Dictionary<int, IMPD_Model> modelsById) {
            var textureIdsFromModels = modelsById.Values
                .SelectMany(x => x.Faces.Where(y => y.Attributes.UseTexture).Select(y => (int) y.Attributes.TextureNo))
                .Distinct()
                .ToHashSet();

            var textureIdsFromSurfaceMap = (!mpdFile.Surface.HasModel) ? [] : mpdFile.Surface.GetAllTiles()
                .Where(x => x.TextureID != 0xFF)
                .Select(x => (int) x.TextureID)
                .Distinct()
                .ToHashSet();

            var usedTextureIds = new HashSet<int>();
            foreach (var id in textureIdsFromModels)
                usedTextureIds.Add(id);
            foreach (var id in textureIdsFromSurfaceMap)
                usedTextureIds.Add(id);

            // (count the animation frames as used textures, since they're still referenced and *probably* used)
            if (mpdFile.IgnoredTextureTable != null)
                foreach (var entry in mpdFile.IgnoredTextureTable)
                    usedTextureIds.Add(entry.TextureID);

            var usedTextures = usedTextureIds
                .OrderBy(x => x)
                .Where(texturesById.ContainsKey)
                .Select(x => texturesById[x])
                .ToArray();
            var unusedTextures = texturesById
                .Where(x => !usedTextureIds.Contains(x.Key))
                .OrderBy(x => x.Key)
                .Select(x => x.Value)
                .ToArray();

            string GetShortScenarioName() {
                switch (mpdFile.Scenario) {
                    case ScenarioType.Scenario1:   return "S1";
                    case ScenarioType.Scenario2:   return "S2";
                    case ScenarioType.Scenario3:   return "S3";
                    case ScenarioType.PremiumDisk: return "PD";
                    default:                       return "??";
                }
            }
            var fileKey = $"{GetShortScenarioName()}|{filename}";
            s_texturesByFile[fileKey] = texturesById.Values.Select(x => (x.Hash, x.ID)).Distinct().ToHashSet();
            s_referencedTexturesByFile[fileKey] = usedTextures.Select(x => (x.Hash, x.ID)).Distinct().ToHashSet();
            s_unreferencedTexturesByFile[fileKey] = unusedTextures.Select(x => (x.Hash, x.ID)).Distinct().ToHashSet();

            return []; //unusedTextures.Select(x => $"Tex0x{x.ID:X2} ({x.Hash})").ToArray();
        }

        public static string[]? GetAllMissingTextures(MPD_File mpdFile, Dictionary<int, IMPD_AnimatableTexture> texturesById, Dictionary<int, IMPD_Model> modelsById) {
            var lastTexture = texturesById.Max(x => x.Key);

            var missingTextureIdsFromModels = modelsById.Values
                .SelectMany(x => x.Faces.Where(y => y.Attributes.UseTexture).Select(y => (int) y.Attributes.TextureNo))
                .Distinct()
                .Where(x => x > lastTexture)
                .ToHashSet();

            var missingTextureIdsFromSurfaceMap = (!mpdFile.Surface.HasModel) ? [] : mpdFile.Surface.GetAllTiles()
                .Where(x => x.TextureID != 0xFF)
                .Select(x => (int) x.TextureID)
                .Distinct()
                .Where(x => x > lastTexture)
                .ToHashSet();

            var missingTextureIds = new HashSet<int>();
            foreach (var id in missingTextureIdsFromModels)
                missingTextureIds.Add(id);
            foreach (var id in missingTextureIdsFromSurfaceMap)
                missingTextureIds.Add(id);

            if (mpdFile.IgnoredTextureTable != null)
                foreach (var entry in mpdFile.IgnoredTextureTable.OrderBy(x => x.TextureID))
                    if (entry.TextureID > lastTexture)
                        missingTextureIds.Add(entry.TextureID);

            return missingTextureIds.Select(x => $"0x{x:X2}").ToArray();
        }

        public static string[]? GetUniqueAnimationFramesMissingFromTextureChunks(MPD_File mpdFile, Dictionary<int, IMPD_AnimatableTexture> texturesById) {
            if (mpdFile.AnimationFrameChunk?.UniqueAnimationFrameTable == null)
                return null;

            var output = new List<string>();
            foreach (var frame in mpdFile.AnimationFrameChunk.UniqueAnimationFrameTable) {
                var correspondingTexture = texturesById.Values.FirstOrDefault(x => x.Hash == frame.Hash);
                if (correspondingTexture == null)
                    output.Add($"Tex@{frame.ImageDataOffset:X4}");
            }

            return output.ToArray();
        }

        public static string[]? GetIgnoredTexturesMissingFromUniqueAnimationFrames(MPD_File mpdFile, Dictionary<int, IMPD_AnimatableTexture> texturesById) {
            if (mpdFile.AnimationFrameChunk?.UniqueAnimationFrameTable == null || mpdFile.IgnoredTextureTable == null)
                return null;
            var ignoredTexturesNotInFrames = mpdFile.IgnoredTextureTable
                .Select(x => texturesById[x.TextureID])
                .Where(x => !mpdFile.AnimationFrameChunk.UniqueAnimationFrameTable.Any(y => y.Hash == x.Hash))
                .ToArray();
            return ignoredTexturesNotInFrames.Select(x => $"Tex0x{x.ID}: Skipped, but not in Chunk[3]").ToArray();
        }

        public static string[]? GetUniqueAnimationFramesWithMatchingTexturesButNotSkipped(MPD_File mpdFile, Dictionary<int, IMPD_AnimatableTexture> texturesById) {
            if (mpdFile.AnimationFrameChunk?.UniqueAnimationFrameTable == null || mpdFile.IgnoredTextureTable == null)
                return null;
            var nonSkippedChunk3Textures = mpdFile.AnimationFrameChunk.UniqueAnimationFrameTable
                .Where(x => !mpdFile.IgnoredTextureTable.Any(y => texturesById[y.TextureID].Hash == x.Hash))
                .ToArray();
            return nonSkippedChunk3Textures.Select(x => $"Chunk[3] Tex0x{x.ID:X2}: In Chunk[3], but not skipped").ToArray();
        }

        public static string[]? IsUniqueAnimationFrameOrderDifferentFromUniqueAssignedFrameOrder(MPD_File mpdFile) {
            if (mpdFile.AnimationFrameChunk?.UniqueAnimationFrameTable == null || mpdFile.Animations == null)
                return null;
            var frameOffsets  = mpdFile.Animations.SelectMany(x => x.AnimationFrameTable).Select(x => x.ImageDataOffset).Distinct().ToArray();
            var chunk3Offsets = mpdFile.AnimationFrameChunk.UniqueAnimationFrameTable.Select(x => x.ImageDataOffset).ToArray();
            return Enumerable.SequenceEqual(frameOffsets, chunk3Offsets) ? [] : ["Order is different"];
        }

        public static string[]? GetOutOfOrderAssignedFrames(MPD_File mpdFile, Dictionary<int, IMPD_AnimatableTexture> texturesById) {
            if (mpdFile.AnimationFrameChunk?.UniqueAnimationFrameTable == null || mpdFile.Animations == null)
                return null;
            var firstCorrespondingAnimationFrames = mpdFile.AnimationFrameChunk.UniqueAnimationFrameTable
                .ToDictionary(x => x.ID, x => mpdFile.Animations
                    .SelectMany(y => y.AnimationFrameTable)
                    .First(y => y.ImageDataOffset == x.ImageDataOffset)
                )
                .ToDictionary(x => x.Key, x => {
                    var texId = texturesById.Values.First(y => y.Hash == x.Value.Hash).ID;
                    return (
                        AnimID:    x.Value.TexAnimID,
                        Frame:     x.Value.Frame,
                        TextureID: texId,
                        SkipID:    mpdFile.IgnoredTextureTable.FirstOrDefault(x => x.TextureID == texId)?.ID
                    );
                });

            var outOfOrderDictionary = firstCorrespondingAnimationFrames
                .ToDictionary(x => x.Key, x => {
                    if (x.Key == 0)
                        return false;
                    if (!firstCorrespondingAnimationFrames.ContainsKey(x.Key - 1))
                        return true;

                    var prev = firstCorrespondingAnimationFrames[x.Key - 1];
                    if (prev.AnimID > x.Value.AnimID)
                        return true;
                    if (prev.AnimID == x.Value.AnimID && prev.Frame > x.Value.Frame)
                        return true;

                    return false;
                });

            var outOfOrder = outOfOrderDictionary.Any(x => x.Value == true);
            return (outOfOrder) ? firstCorrespondingAnimationFrames
                .Select(x =>
                    (outOfOrderDictionary[x.Key] ? "!! " : "   ") +
                    $"UniqueFrame0x{x.Key:X2}: Anim0x{x.Value.AnimID:X2}_{x.Value.Frame:X2} (TexID = 0x{x.Value.TextureID:X2}, SkipID = " +
                    (x.Value.SkipID.HasValue ? $"0x{x.Value.SkipID:X2}" : "(none)") + ")")
                .ToArray() : [];
        }

        public static string[]? GetOutOfOrderIgnoredTextures(MPD_File mpdFile) {
            if (mpdFile.IgnoredTextureTable == null)
                return null;
            var outOfOrderArray = mpdFile.IgnoredTextureTable
                .Select((x, i) => (i == 0) ? false : mpdFile.IgnoredTextureTable[i - 1].TextureID >= x.TextureID)
                .ToArray();
            var outOfOrder = outOfOrderArray.Any(x => x == true);
            return outOfOrder ? mpdFile.IgnoredTextureTable.Select((x, i) => (outOfOrderArray[i] ? "!! " : "   ") + $"0x{x.ID:X2}: Tex0x{x.TextureID:X2}").ToArray() : [];
        }

        public static string[]? GetUniqueAnimationFramesInDifferentOrderThanAssignmentOrder(MPD_File mpdFile, Dictionary<int, IMPD_AnimatableTexture> texturesById) {
            if (mpdFile.IgnoredTextureTable == null || mpdFile.AnimationFrameChunk?.UniqueAnimationFrameTable == null || mpdFile.Animations == null)
                return null;

            var frames = mpdFile.Animations
                .SelectMany(x => x.AnimationFrameTable)
                .GroupBy(x => x.ImageDataOffset)
                .Select((x, i) => (
                    Assigned: x.First(),
                    InChunk3: mpdFile.AnimationFrameChunk.UniqueAnimationFrameTable[i],
                    Texture:  texturesById.Values.First(y => y.Hash == mpdFile.AnimationFrameChunk.UniqueAnimationFrameTable[i].Hash))
                )
                .GroupBy(x => x.Assigned.TexAnimID)
                .Select(x => x.ToArray())
                .ToArray();

            var outOfOrderArray = frames
                .Select(x => x.Select((y, i) => x[i].Assigned.ImageDataOffset != y.InChunk3.ImageDataOffset).ToArray())
                .ToArray();

            var outOfOrder = outOfOrderArray.Any(x => x.Any(y => y == true));
            return outOfOrder
                ? frames.SelectMany((x, i) => x
                    .Select((y, j) => (outOfOrderArray[i][j] ? "!! " : "   ") + $"[{i}][{j}]: " +
                        $"{y.InChunk3.Name} (Offset=0x{y.InChunk3.ImageDataOffset:X4}), " +
                        $"{y.Assigned.Name} (Offset=0x{y.Assigned.ImageDataOffset:X4}) (TexID=0x{y.Texture.ID:X2})"
                    )
                ).ToArray()
                : [];
        }

        public static string[]? GetExpectedIgnoredTextureTable(MPD_File mpdFile, Dictionary<int, IMPD_AnimatableTexture> texturesById, Dictionary<int, IMPD_Model> modelsById) {
            if (mpdFile.IgnoredTextureTable == null || mpdFile.AnimationFrameChunk?.UniqueAnimationFrameTable == null || mpdFile.Animations == null)
                return null;

            var textureIdsFromModels = modelsById.Values
                .SelectMany(x => x.Faces.Where(y => y.Attributes.UseTexture).Select(y => (int) y.Attributes.TextureNo)).Distinct().ToHashSet();
            var textureIdsFromSurfaceMap = (!mpdFile.Surface.HasModel) ? [] : mpdFile.Surface.GetAllTiles()
                .Where(x => x.TextureID != 0xFF).Select(x => (int) x.TextureID).Distinct().ToHashSet();
            var textureIdsFromAnimations = mpdFile.Animations
                .Select(x => x.TextureID).Distinct().ToHashSet();

            var usedFrames = textureIdsFromModels
                .Concat(textureIdsFromSurfaceMap)
                .Concat(textureIdsFromAnimations)
                .Distinct().ToHashSet();

            var unusedTextures = texturesById.Values.Where(x => !usedFrames.Contains(x.ID)).ToArray();
            var possibleSkippedAnimationFrames = mpdFile.Animations
                .SelectMany(x => x.AnimationFrameTable)
                .GroupBy(x => x.Hash)
                .Where(x => unusedTextures.Any(y => y.Hash == x.Key))
                .ToDictionary(x => x.Key, x => unusedTextures.Where(y => y.Hash == x.Key).ToArray());
            var definiteSkippedAnimationFrames = possibleSkippedAnimationFrames
                //.Where(x => x.Value.Length == 1)
                .Select(x => x.Value[0])
                .ToArray();

            var expectedIgnoredTextureList = definiteSkippedAnimationFrames.Select(x => x.ID).Order().ToArray();
            var actualIgnoredTextureList = mpdFile.IgnoredTextureTable.Select(x => (int) x.TextureID).ToArray();

            return !Enumerable.SequenceEqual(expectedIgnoredTextureList, actualIgnoredTextureList)
                ? ["Expected: [" + string.Join(", ", expectedIgnoredTextureList.Select(x => $"0x{x:X2}")) + "]",
                    "  Actual: [" + string.Join(", ", actualIgnoredTextureList.Select(x => $"0x{x:X2}")) + "]"]
                : [];
        }

        public static string[]? HasBigDumbGradients(MPD_File mpdFile) {
            if (!(mpdFile.GradientTable?.Length >= 1))
                return null;
            var gradient = mpdFile.GradientTable[0];

            var errors = new List<string>();

            if (!gradient.AffectsGround && gradient.GroundIntensityRaw > 0)
                errors.Add($"Ground OFF: 0x{gradient.GroundIntensityRaw:X2}");
            if (gradient.AffectsGround && gradient.GroundIntensityRaw == 0)
                errors.Add($"Ground ON: 0x{gradient.GroundIntensityRaw:X2}");

            if (!gradient.AffectsSky && gradient.SkyIntensityRaw > 0)
                errors.Add($"Sky OFF: 0x{gradient.SkyIntensityRaw:X2}");
            if (gradient.AffectsSky && gradient.SkyIntensityRaw == 0)
                errors.Add($"Sky ON: 0x{gradient.SkyIntensityRaw:X2}");

            if (!gradient.AffectsModelsAndSurface && gradient.ModelsAndSurfaceIntensityRaw > 0)
                errors.Add($"Models OFF: 0x{gradient.ModelsAndSurfaceIntensityRaw:X2}");
            if (gradient.AffectsModelsAndSurface && gradient.ModelsAndSurfaceIntensityRaw == 0)
                errors.Add($"Models ON: 0x{gradient.ModelsAndSurfaceIntensityRaw:X2}");

            return errors.ToArray();
        }

        public static string[]? HasMismatchedAnimationDimensions(Dictionary<int, IMPD_AnimatableTexture> texturesById) {
            var anims = texturesById.Values.Where(x => x.Animation != null && !x.IsIgnored && !x.Animation.IsIgnored).ToDictionary(x => x, x => (AnimationStruct) x.Animation);
            var mismatches = anims.Where(x => x.Key.Width != x.Value.Width || x.Key.Height != x.Value.Height).ToDictionary();
            return mismatches.Select(x => $"0x{x.Key.ID:X2}: ({x.Key.Width}x{x.Key.Height}) => ({x.Value.Width}x{x.Value.Height})").ToArray();
        }

        public static string[]? GetMultiReferenceModelSwitchGroups(MPD_File mpdFile) {
            if (!(mpdFile.ModelSwitchGroupsTable?.Length > 0))
                return null;

            var errors = new List<string>();

            var offTables      = mpdFile.ModelSwitchGroups.Select(x => x.ModelInstancesVisibleWhenOff).ToArray();
            var offModelCount  = offTables.SelectMany(x => x).GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
            var multiOffModels = offModelCount.Where(x => x.Value > 1).ToDictionary();
            errors.AddRange(multiOffModels.Select(x => $"Off:  0x{x.Key:X2} x{x.Value}"));

            var onTables      = mpdFile.ModelSwitchGroups.Select(x => x.ModelInstancesVisibleWhenOn).ToArray();
            var onModelCount  = onTables.SelectMany(x => x).GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
            var multiOnModels = onModelCount.Where(x => x.Value > 1).ToDictionary();
            errors.AddRange(multiOnModels.Select(x => $"On:   0x{x.Key:X2} x{x.Value}"));

            var offModels  = offModelCount.Keys.ToHashSet();
            var onModels   = onModelCount.Keys.ToHashSet();
            var bothModels = offModels.Where(onModels.Contains).ToHashSet();
            errors.AddRange(bothModels.Select(x => $"Both: 0x{x:X2}"));

            return errors.ToArray();
        }

        public static string[]? DoesntHaveGradientTableOffset(MPD_File mpdFile)
            => mpdFile.MPDHeader.OffsetGradient == 0 ? ["Yes"] : [];

        public static string[]? HasWeirdGradients(MPD_File mpdFile) {
            if (mpdFile.GradientTable == null)
                return null;

            var errors = new List<string>();
            if (mpdFile.GradientTable.Length > 1)
                errors.Add($"{mpdFile.GradientTable.Length} tables!!");

            foreach (var table in mpdFile.GradientTable)
                if (table.IsDummiedOut)
                    errors.Add($"{table.Name} is dummied-out");

            return errors.ToArray();
        }

        public static string[]? SerializedContentIsIdentical(MPD_File mpdFile) {
            MPD_File newMpd;
            using (var stream = new MemoryStream()) {
                var writer = new MPD_Writer(stream, mpdFile.Scenario);
                writer.WriteMPD(mpdFile);
                newMpd = MPD_File.Create(new ByteData(new ByteArray(stream.ToArray())), mpdFile.NameGetterContext, mpdFile.Scenario);
            }

            return AnalysisUtils.GetMPDContentComparisonErrors(mpdFile, newMpd);
        }

        public static string[]? DoubleSerializeIsIdentical(MPD_File mpdFile) {
            byte[] bytes1;
            using (var stream = new MemoryStream()) {
                var writer = new MPD_Writer(stream, mpdFile.Scenario);
                writer.WriteMPD(mpdFile);
                bytes1 = stream.ToArray();
            }

            byte[] bytes2;
            using (var stream = new MemoryStream()) {
                var writer = new MPD_Writer(stream, mpdFile.Scenario);
                var mpd = MPD_File.Create(new ByteData(new ByteArray(bytes1)), mpdFile.NameGetterContext, mpdFile.Scenario);
                writer.WriteMPD(mpdFile);
                bytes2 = stream.ToArray();
            }

            return AnalysisUtils.GetByteComparisonErrors(bytes1, bytes2);
        }

        public static string[]? GetAllModelInstancesWithLessThanEightLoDs(MPD_File mpdFile) {
            return mpdFile.ModelCollections[MPD_CollectionType.Primary].ModelInstances
                .Where(x => x.LevelsOfDetail != 8)
                .Select(x => $"0x{x.ID:X3} (ModelID=0x{x.ModelID:X03})")
                .ToArray();
        }

        public static string[]? HasDummiedOutIgnoredTexturesTable(MPD_File mpdFile)
            => mpdFile.IgnoredTextureTable?.IsDummiedOut == true ? ["Yes"] : [];

        public static string[]? HasMisplacedChunks(MPD_File mpdFile) {
            string[] misplaced1 = mpdFile.BinaryReproductionFlags.MisplacedModelsChunkIndex.HasValue ? ["Misplaced Models"] : [];
            string[] misplaced2 = mpdFile.BinaryReproductionFlags.MisplacedSurfaceModelChunkIndex.HasValue ? ["Misplaced Surface Model"] : [];
            return [.. misplaced1, .. misplaced2];
        }
    }
}
