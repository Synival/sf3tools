namespace CommonLib.Rigging {
    public class ModelRig : IModelRig {
        public ModelRig(Bone rootBone) {
            RootBone = rootBone;
        }

        public ModelRig(ModelRig rig) {
            RootBone = new Bone(rig.RootBone);
        }

        public Bone RootBone { get; }
    }
}
