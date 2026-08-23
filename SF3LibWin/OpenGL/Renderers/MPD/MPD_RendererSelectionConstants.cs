namespace SF3.Win.OpenGL.Renderers.MPD {
    public static class MPD_RendererSelectionConstants {
        public const float SurfaceTile     = 0;
        public const float PrimaryModels   = 1;
        public const float ExtraModels     = 2;
        public const float Actors          = 3;
        public const float CollisionLines  = 4;
        public const float CollisionPoints = 5;

        public const float SurfaceTileB     = SurfaceTile     * (1.0f / 64.0f);
        public const float PrimaryModelsB   = PrimaryModels   * (1.0f / 64.0f);
        public const float ExtraModelsB     = ExtraModels     * (1.0f / 64.0f);
        public const float ActorsB          = Actors          * (1.0f / 64.0f);
        public const float CollisionLinesB  = CollisionLines  * (1.0f / 64.0f);
        public const float CollisionPointsB = CollisionPoints * (1.0f / 64.0f);
    }
}
