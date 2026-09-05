using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Attributes;
using CommonLib.SGL;
using SF3.ByteData;
using SF3.X8PC;

namespace SF3.Models.Structs.X8PC {
    // There is no "bone" struct in X8PC files; this is just a wrapper.
    public class PCBoneWrapperStruct : IStruct, IBone, ISGL_ModelInstanceCollection {
        public PCBoneWrapperStruct(string name, IBone bone, PolyChar polyChar) {
            Name = name;

            _actualBone = bone;
            PolyChar = polyChar;

            Path = _actualBone.GetBonePath();

            var outline = _actualBone.ToOutline();

            var flattened = this.Flatten();
            var flattenedModelIds = flattened.Where(x => x.ModelID.HasValue).Select(x => x.ModelID.Value).ToArray();
            var boneModelIds      = (this.Children ?? new IBone[0]).Where(x => x.ModelID.HasValue).Select(x => x.ModelID.Value).ToArray();

            var allModels = ((polyChar.XPDataTables?.Length ?? 0) == 0)
                ? new List<SGL_ModelInstance>()
                : polyChar.XPDataTables[0]
                    .Join(flattenedModelIds, x => x.ModelID, y => y, (x, y) => x)
                    .Select((x, i) => new SGL_ModelInstance((_1, _2) => x) {
                        ModelInstanceID = i, ModelID = x.ModelID, ModelCollectionID = x.ModelCollectionID,
                        ForceTransparency = boneModelIds.Contains(x.ModelID) ? (float?) null : 0.25f,
                    })
                    .ToList();

            var weaponModel = polyChar.WeaponXPData;
            if (weaponModel != null) {
                var weaponBones = flattened.Where(x => x.Tag.HasValue && (x.Tag.Value == 0x30 || x.Tag.Value == 0x81)).ToArray();
                if (weaponBones.Length > 0) {
                    foreach (var weaponBone in weaponBones) {
                        var weaponModelInstance = new SGL_ModelInstance((_1, _2) => weaponModel) {
                            ModelInstanceID = allModels.Count, ModelID = weaponModel.ModelID, ModelCollectionID = weaponModel.ModelCollectionID,
                            ForceTransparency = (weaponBone.Parent == _actualBone) ? (float?) null : 0.25f,
                            Matrix = weaponBone.CreateMatrix()
                        };
                        if (weaponModel != null)
                            allModels.Add(weaponModelInstance);
                    }
                }
            }

            _sglModelInstancesById = allModels.ToDictionary(x => x.ModelInstanceID, x => (ISGL_ModelInstance) x);

            Depth = GetDepth();
            ChildDepth = GetChildDepth(_actualBone, 0);
        }

        private int GetDepth() {
            var depth = 0;
            for (var parent = _actualBone.Parent; parent != null; parent = parent.Parent)
                depth++;
            return depth;
        }

        private int GetChildDepth(IBone bone, int currentDepth) {
            if (bone.Children == null)
                return currentDepth;
            var subBones = bone.Children.Where(x => x.Children != null).ToArray();
            if (subBones.Length == 0)
                return currentDepth;
            return subBones.Select(x => GetChildDepth(x, currentDepth + 1)).Max();
        }

        public PolyChar PolyChar { get; }

        public int ID => _actualBone.BoneID ?? -1;

        [TableViewModelColumn(addressField: null, displayOrder: -3, minWidth: 45, displayGroup: "Metadata")]
        public int? BoneID => _actualBone.BoneID;

        [TableViewModelColumn(addressField: null, displayOrder: -1, minWidth: 120, displayGroup: "Metadata")]
        public string Name { get; }

        [TableViewModelColumn(addressField: null, displayOrder: 0, minWidth: 300)]
        public string Path { get; }

        [TableViewModelColumn(addressField: null, displayOrder: 1)]
        public int Depth { get; }

        [TableViewModelColumn(addressField: null, displayOrder: 2)]
        public int ChildDepth { get; }

        public IByteData Data => null;
        public int Address => 0;
        public int Size => 0;

        public int? Tag             => _actualBone.Tag;
        public IBone Parent         => _actualBone.Parent;
        public int? ModelID         => _actualBone.ModelID;
        public VECTOR? Position     => _actualBone.Position;
        public QUATERNION? Rotation => _actualBone.Rotation;
        public VECTOR? Scale        => _actualBone.Scale;
        public IBone[] Children     => _actualBone.Children;

        private readonly IBone _actualBone;
        private readonly Dictionary<int, ISGL_ModelInstance> _sglModelInstancesById;

        public ISGL_ModelInstance GetModelInstance(int id) => _sglModelInstancesById.TryGetValue(id, out var instance) ? instance : null;

        public IEnumerator<ISGL_ModelInstance> GetEnumerator() => _sglModelInstancesById.Values.AsEnumerable().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
