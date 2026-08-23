using System;
using System.Collections.Generic;
using CommonLib.SGL;

namespace SF3.Win.OpenGL.Renderers.Shared {
    public class RendererOptions {
        public bool DrawModels;
        public bool DrawExtraModels;
        public bool DrawGradients;
        public bool DrawNormals;
        public bool DrawWireframe;

        public bool ApplyLighting;

        public bool HideModelsNotFacingCamera;
        public float ModelsYRotation = 0.0f;
        public float ModelsViewAngleMax = 108.0f;
        public float ModelsViewAngleMin = -108.0f;

        public bool RotateSpritesUp;
        public bool ForceTwoSidedTextures;
        public bool SmoothLighting;

        public HashSet<int> ModelsToHide;

        public Func<ISGL_ModelInstance, bool[] /*modelDirectionsFacingCamera*/, bool> ModelInstanceFilter;
    }
}