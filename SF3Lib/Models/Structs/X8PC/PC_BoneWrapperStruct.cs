using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Attributes;
using CommonLib.SGL;
using SF3.ByteData;
using SF3.X8PC;

namespace SF3.Models.Structs.X8PC {
    // There is no "bone" struct in X8PC files; this is just a wrapper.
    public class PC_BoneWrapperStruct : IStruct, IBone, ISGL_ModelCollection {
        public PC_BoneWrapperStruct(int id, string name, IBone bone, PolyChar polyChar) {
            ID       = id;
            Name     = name;

            _actualBone = bone;
            PolyChar = polyChar;

            Path = _actualBone.GetBonePath();

            var outline = _actualBone.ToOutline();

            var flattened = this.Flatten();
            var modelIds    = flattened.Where(x => x.ModelID.HasValue).Select(x => x.ModelID.Value).ToArray();

            // TODO: Should be instances, not models.
            var allModels = ((polyChar.XPDataTables?.Length ?? 0) == 0)
                ? new List<ISGL_Model>()
                : polyChar.XPDataTables[0].Join(modelIds, x => x.ModelID, y => y, (x, y) => x).Cast<ISGL_Model>().ToList();

            var weaponBones = flattened.Where(x => x.Tag.HasValue && (x.Tag.Value == 0x30 || x.Tag.Value == 0x81)).ToArray();
            if (weaponBones.Length > 0) {
                // TODO: Add multiple times, as instances, with coordinates.
                var weaponModel = polyChar.XPDataTables.Length >= 2 ? (polyChar.XPDataTables[1].Count > 0 ? polyChar.XPDataTables[1][0] : null) : null;
                allModels.Add(weaponModel);
            }

            _models = allModels.ToArray();

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

        [TableViewModelColumn(addressField: null, displayOrder: -3, displayFormat: "X2", minWidth: 45, displayGroup: "Metadata")]
        public int ID { get; }
        public PolyChar PolyChar { get; }
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
        public int? BoneID          => _actualBone.BoneID;
        public int? ModelID         => _actualBone.ModelID;
        public VECTOR? Position     => _actualBone.Position;
        public QUATERNION? Rotation => _actualBone.Rotation;
        public VECTOR? Scale        => _actualBone.Scale;
        public IBone[] Children     => _actualBone.Children;

        private readonly IBone _actualBone;
        private readonly ISGL_Model[] _models;

        public ISGL_Model GetModel(int id, int lod) => null;
        public ISGL_Model[] GetAllModels() => _models;
    }
}
