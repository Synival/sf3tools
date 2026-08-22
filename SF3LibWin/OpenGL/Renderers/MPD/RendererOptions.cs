using System.Collections.Generic;
using CommonLib.Imaging;

namespace SF3.Win.OpenGL.Renderers.MPD {
    public class RendererOptions {
        public bool DrawModels;
        public bool DrawExtraModels;
        public bool DrawSurfaceModel;
        public bool DrawGround;
        public bool DrawSky;
        public bool DrawGradients;
        public bool DrawActors;

        public bool DrawNormals;
        public bool DrawWireframe;
        public bool DrawOutlines;

        public bool DrawTerrainTypes;
        public bool DrawEventIDs;
        public bool DrawBoundaries;
        public bool DrawBattleZones;
        public bool DrawCollisionLines;

        public bool ApplyLighting;

        public bool HideModelsNotFacingCamera;
        public float ModelsYRotation = 0.0f;
        public float ModelsViewAngleMax = 108.0f;
        public float ModelsViewAngleMin = -108.0f;

        public bool UseOutsideLighting;
        public bool RotateSpritesUp;
        public bool ForceTwoSidedTextures;
        public bool SmoothLighting;

        public float BackgroundX = 0.00f;
        public float BackgroundY = 0.00f;

        public IColorAdjustRGB555 GroundAdj;

        public HashSet<int> ModelsToHide;

        public bool WillDrawSurfaceModel
            => DrawSurfaceModel || DrawTerrainTypes || DrawEventIDs;
        public bool WillDrawAnyModels
            => DrawModels || DrawExtraModels;
        public bool WillDrawAnyObjects
            => WillDrawAnyModels || WillDrawSurfaceModel;
        public bool WillDrawSurfaceModelWireframe
            => DrawWireframe && !DrawNormals && (DrawSurfaceModel || DrawTerrainTypes || DrawEventIDs);
    }
}