using CommonLib.Imaging;
using SF3.Win.OpenGL.Renderers.Shared;

namespace SF3.Win.OpenGL.Renderers.MPD {
    public class MPD_RendererOptions : RendererOptions {
        public bool DrawSurfaceModel;
        public bool DrawGround;
        public bool DrawSky;
        public bool DrawActors;
        public bool DrawOutlines;

        public bool DrawTerrainTypes;
        public bool DrawEventIDs;
        public bool DrawBoundaries;
        public bool DrawBattleZones;
        public bool DrawCollisionLines;

        public bool UseOutsideLighting;

        public float BackgroundX = 0.00f;
        public float BackgroundY = 0.00f;

        public IColorAdjustRGB555 GroundAdj;

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