using CommonLib.SGL;
using OpenTK.Mathematics;
using SF3.Types;
using SF3.Win.OpenGL.GLResources.MPD;
using SF3.Win.OpenGL.Renderers.Shared;

namespace SF3.Win.OpenGL.Renderers.MPD {
    public class MPD_ModelRenderer : ModelRenderer {
        public MPD_ModelRenderer() {
            ModelPositionOffset = new Vector3(-MPD_ModelResources.ModelOffsetX, 0, -MPD_ModelResources.ModelOffsetZ);
        }

        protected override Vector4 ModelSelectionColor(ISGL_ModelInstance model) {
            var r = model.ModelInstanceID % 64 / 64.0f;
            var g = model.ModelInstanceID / 64 / 64.0f;
            return new Vector4(r, g, model.ModelCollectionID == (int) MPD_CollectionType.Primary ? MPD_RendererSelectionConstants.PrimaryModelsB : MPD_RendererSelectionConstants.ExtraModelsB, 1.0f);
        }
    }
}
