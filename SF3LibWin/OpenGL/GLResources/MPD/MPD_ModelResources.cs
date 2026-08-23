using System.Collections.Generic;
using System.Linq;
using CommonLib.Imaging;
using CommonLib.SGL;
using SF3.Types;
using SF3.MPD.Interfaces;
using SF3.MPD.Project;
using SF3.MPD.Extensions;
using SF3.Win.OpenGL.GLResources.Shared;

namespace SF3.Win.OpenGL.GLResources.MPD {
    public class MPD_ModelResources : ModelResources, IMPD_Resources {
        public const float ModelOffsetX = SurfaceModelResources.WidthInTiles / -2f;
        public const float ModelOffsetZ = SurfaceModelResources.HeightInTiles / -2f;

        public MPD_ModelResources(bool applyShadowTags, bool applyHideTags)
        : base(applyShadowTags, applyHideTags) {
        }

        private void InitDictsForType(MPD_CollectionType collection)
            => InitDictsForType((int) collection);

        private Dictionary<int, IAnimatableTexture> GetTextureDictionaryByCollection(IMPD_ModelCollection modelCollection, IMPD mpdFile) {
            var hasIgnored = !mpdFile.Settings.AreIgnoredTexturesDummiedOut;
            return modelCollection.Textures
                .Where(x => !hasIgnored || !x.IsIgnored)
                .ToDictionary(x => x.TextureID, x => (IAnimatableTexture) x);
        }

        private float? GetForceSemiTransparencyAlpha(IMPD mpdFile)
            => 1.0f - (mpdFile.BinaryReproductionFlags.PaletteAdjustmentIsTruncated ? 0x0F : mpdFile.Settings.ShadowTransparency / (float) 0x1F);

        public void Update(IMPD mpdFile) {
            Reset();

            if (mpdFile?.ModelCollections == null)
                return;

            var isForcedSemiTransparentAlpha = GetForceSemiTransparencyAlpha(mpdFile);

            var modelInstanceList = new List<IMPD_ModelInstance>();
            foreach (var mc in mpdFile.ModelCollections.Values) {
                if (mc.Collection.IsHeaderModelCollection())
                    continue;

                // Get all instances of models in this collection.
                var instances = mc.ModelInstances;
                modelInstanceList.AddRange(instances);

                // There is a function that scans for models with the tag '2000' and forcibly changes all the textures
                // in their PDATAs to be semi-transparent. Yes, this is redundant to have on the *model* instead of the *PDATA*,
                // so who knows why it works this way.
                var modelsWith2000Tag = ApplyShadowTags ? instances
                    .Where(x => x.Tag >= 2000 && x.Tag < 2100)
                    .Select(x => x.ModelID)
                    .Distinct()
                    .ToHashSet()
                    : [];

                // There are some (usually) bright-red models in Scenario 3 that are removed when
                // the 3000 tag is present. They are used to crop out models so the ground texture (VDP2) is visible instead.
                var modelsWith3000Tag = ApplyHideTags ? instances
                    .Where(x => x.Tag == 3000)
                    .Select(x => x.ModelID)
                    .Distinct()
                    .ToHashSet()
                    : [];

                InitDictsForType(mc.Collection);
                var sglModelsByID = SGL_ModelsByIDByCollection[(int) mc.Collection];

                var uniqueModelIDs = instances
                    .Select(x => x.ModelID)
                    .Distinct()
                    .ToArray();

                var texturesById = GetTextureDictionaryByCollection(mc, mpdFile);

                foreach (var id in uniqueModelIDs) {
                    if (id == -1)
                        continue;

                    var sglModel = sglModelsByID.TryGetValue(id, out var sglModelOut) ? sglModelOut : null;
                    if (sglModel == null)
                        sglModelsByID[id] = sglModel = mc.GetModel(id, 0);
                    if (sglModel == null)
                        continue;

                    // Don't render movable models; they're not placed on the map in that way.
                    if (!mc.IsHeaderModelCollection()) {
                        var isForcedSemiTransparent = modelsWith2000Tag.Contains(id);
                        var isHideMesh = modelsWith3000Tag.Contains(id);
                        CreateAndAddQuadModels((int) mc.Collection, sglModel, texturesById, isForcedSemiTransparent ? isForcedSemiTransparentAlpha : null, isHideMesh);
                    }
                }
            }

            ModelInstances = modelInstanceList.ToArray();
        }

        public void Update(IMPD mpdFile, IMPD_ModelCollection models, ISGL_Model sglModel,
            bool forceSemiTransparent = false, bool isHideMesh = false,
            float rotX = 0f, float rotY = 0f, float rotZ = 0f,
            float scaleX = 1f, float scaleY = 1f, float scaleZ = 1f
        ) {
            Reset();
            if (models == null || sglModel == null)
                return;

            InitDictsForType(models.Collection);
            SGL_ModelsByIDByCollection[(int) models.Collection][sglModel.ModelID] = sglModel;

            var texturesById = GetTextureDictionaryByCollection(models, mpdFile);
            CreateAndAddQuadModels((int) models.Collection, sglModel, texturesById, forceSemiTransparent ? GetForceSemiTransparencyAlpha(mpdFile) : null, isHideMesh);

            var modelInstance = new MPD_ModelInstance() {
                Collection = models,
                ModelInstanceID = 0,
                ModelID = sglModel.ModelID,
                PositionX = 32 * 32,
                PositionZ = 32 * 32,
                AngleX = rotX,
                AngleY = rotY,
                AngleZ = rotZ,
                ScaleX = scaleX,
                ScaleY = scaleY,
                ScaleZ = scaleZ,
            };

            ModelInstances = [modelInstance];
        }
    }
}
