namespace SF3.Actors {
    /// <summary>
    /// Abstract interface for any kind of actor, from either an NPC table or battle.
    /// </summary>
    public interface IActor {
        /// <summary>
        /// ID of the sprite used for display purposes.
        /// </summary>
        int SpriteID { get; set; }

        /// <summary>
        /// When set, the Y coordinate is set explicitly. When unset -- as it the case for battles -- the Y coordinate
        /// is calculated as the position on the surface heightmap.
        /// </summary>
        bool HasActorY { get; }

        /// <summary>
        /// X component of the actor position in game coordinates.
        /// </summary>
        float ActorX { get; set; }

        /// <summary>
        /// Y component of the actor position in game coordinates.
        /// If 'HasActorY' is false, this value should not be used.
        /// </summary>
        float ActorY { get; set; }

        /// <summary>
        /// Z component of the actor position in game coordinates.
        /// </summary>
        float ActorZ { get; set; }

        /// <summary>
        /// The direction the actor is facing in degrees in range [-180, 180).
        /// </summary>
        float ActorDirection { get; set; }
    }
}
