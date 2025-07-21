namespace SF3.CHR {
    public class AnimationGroupDef {
        public override string ToString()
            => (Directions.HasValue ? Directions.ToString() + ": " : "") + ((Animations != null) ? string.Join(", ", Animations) : "[]");

        public int? Directions;
        public string[] Animations;
    }
}
