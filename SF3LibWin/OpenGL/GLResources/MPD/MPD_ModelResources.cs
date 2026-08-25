using System.Collections.Generic;
using System.Linq;
using SF3.Types;
using SF3.MPD.Interfaces;
using SF3.MPD.Extensions;
using SF3.Win.OpenGL.GLResources.Shared;

namespace SF3.Win.OpenGL.GLResources.MPD {
    public class MPD_ModelResources : ModelResources, IMPD_Resources {
        public const float ModelOffsetX = MPD_SurfaceModelResources.WidthInTiles / -2f;
        public const float ModelOffsetZ = MPD_SurfaceModelResources.HeightInTiles / -2f;

        public MPD_ModelResources(bool applyShadowTags, bool applyHideTags)
        : base(applyShadowTags, applyHideTags) {
        }

        private void InitDictsForType(MPD_CollectionType collection)
            => InitDictsForType((int) collection);

        public void Update(IMPD mpdFile) {
            Reset();

            if (mpdFile?.ModelCollections == null)
                return;

            var isForcedSemiTransparentAlpha = mpdFile.GetForceSemiTransparencyAlpha();

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

                var texturesById = mpdFile.GetAnimatableTexturesByModelCollectionID(mc.Collection);

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
                        CreateAndAddQuadModels(
                            (int) mc.Collection, sglModel, texturesById,
                            isForcedSemiTransparent ? isForcedSemiTransparentAlpha : null,
                            isHideMesh, forceLighting: null
                        );
                    }
                }
            }

            ModelInstances = modelInstanceList.ToArray();
        }
    }
}
