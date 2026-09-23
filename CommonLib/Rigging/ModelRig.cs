namespace CommonLib.Rigging {
    public class ModelRig : IModelRig {
        public ModelRig(IBone rootBone) {
            RootBone = rootBone;
        }

        public ModelRig(ModelRig rig) {
            RootBone = new Bone(rig.RootBone);
        }

        public IBone RootBone { get; }
    }
}
