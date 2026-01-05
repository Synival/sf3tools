namespace SF3.Types {
    public enum MPD_CollectionType {
        Primary     = 0,
        Chest       = 1,
        LockedChest = 2,
        Barrel      = 3,
        ExtraModels = 4, // Seen in Scn1 Z_AS.MPD; Scn2 Kraken, failed ice spell against Galm
    }

    public static class CollectionTypeExtensions {
        public static bool IsHeaderModelCollection(this MPD_CollectionType collection)
            => collection == MPD_CollectionType.Chest || collection == MPD_CollectionType.LockedChest || collection == MPD_CollectionType.Barrel;
    }
}
