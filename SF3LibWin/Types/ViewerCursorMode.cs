namespace SF3.Win.Types {
    public enum ViewerCursorMode {
        Select   = 0,
        Navigate = 1,

        DrawFirst         = 100,

        DrawGrassland     = 100,
        DrawDirt          = 101,
        DrawDarkGrass     = 102,
        DrawForest        = 103,
        DrawBrownMountain = 104,
        DrawGreyMountain  = 105,
        DrawMountainPeak  = 106,
        DrawDesert        = 107,
        DrawRiver         = 108,
        DrawBridge        = 109,
        DrawWater         = 110,
        DrawNoEntry       = 111,

        DrawLast          = 111,
    }

    public static class ViewerCursorModeExtensions {
        public static bool IsDrawingMode(this ViewerCursorMode mode)
            => mode >= ViewerCursorMode.DrawFirst && mode <= ViewerCursorMode.DrawLast;
    }
}
