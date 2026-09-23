namespace CommonLib.Rigging {
    public class ModelRig : IModelRig {
        public ModelRig(Bone rootBone) {
            RootBone = rootBone;
        }

        public Bone RootBone { get; }
    }
}
