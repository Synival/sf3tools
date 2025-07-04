namespace SF3.Types {
    public enum SpriteFrameDirection {
        Unset = 0,

        // 1-2 sprite directions
        First  = 1,
        Second = 2,

        // Four directions (standard)
        SSE = 11,
        ESE = 13,
        ENE = 15,
        NNE = 17,

        // Only if 6 directions
        S = 10,
        N = 18,

        // Only if 8 directions
        NNW = 19,
        WNW = 21,
        WSW = 23,
        SSW = 25,

        // Only if 5 directions (N,SE,E,NE,N)
        SE = 12,
        E  = 14,
        NE = 16
    }
}
