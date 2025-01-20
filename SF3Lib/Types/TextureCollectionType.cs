namespace SF3.Types {
    public enum TextureCollectionType {
        PrimaryTextures = 0,
        SpecialChunk10Textures = 1, // Seen in Scn1 Z_AS.MPD, where textures restart at 0 for some reason.
        MovableObjects1 = 2,
        MovableObjects2 = 3,
        MovableObjects3 = 4
    }
}
