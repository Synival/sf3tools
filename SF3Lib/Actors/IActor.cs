namespace SF3.Actors {
    public interface IActor {
        bool HasActorY { get; }

        float ActorX { get; set; }
        float ActorY { get; set; }
        float ActorZ { get; set; }
    }
}
