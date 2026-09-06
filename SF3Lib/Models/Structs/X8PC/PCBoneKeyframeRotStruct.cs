using CommonLib.Attributes;
using CommonLib.SGL;
using SF3.ByteData;

namespace SF3.Models.Structs.X8PC {
    public class PCBoneKeyframeRotStruct : IStruct {
        private readonly int _frameAddr;
        private readonly int _xAddr;
        private readonly int _yAddr;
        private readonly int _zAddr;
        private readonly int _wAddr;

        public PCBoneKeyframeRotStruct(IByteData data, int boneId, int keyframeId, string name, int frameAddr, int xAddr, int yAddr, int zAddr, int wAddr) {
            Data        = data;
            Name        = name;

            BoneID     = boneId;
            KeyframeID = keyframeId;

            _frameAddr = frameAddr;
            _xAddr     = xAddr;
            _yAddr     = yAddr;
            _zAddr     = zAddr;
            _wAddr     = wAddr;
        }

        public IByteData Data { get; }
        public int ID => BoneID * 1000 + KeyframeID;
        public int Address => 0; // N/A
        public int Size => 0; // N/A

        [TableViewModelColumn(displayOrder: -3)]
        [BulkCopy]
        public int BoneID { get; }

        [TableViewModelColumn(displayOrder: -2)]
        [BulkCopy]
        public int KeyframeID { get; }

        [TableViewModelColumn(displayOrder: -1, minWidth: 150)]
        [BulkCopy]
        public string Name { get; }

        [TableViewModelColumn(addressField: nameof(_frameAddr), displayOrder: 0)]
        [BulkCopy]
        public ushort Frame {
            get => Data.GetUInt16(_frameAddr);
            set => Data.SetUInt16(_frameAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_xAddr), displayOrder: 1, minWidth: 100)]
        [BulkCopy]
        public float X {
            get => Data.GetCompressedFIXED(_xAddr, 14).Float;
            set => Data.SetCompressedFIXED(_xAddr, new CompressedFIXED(value, 14, 0));
        }

        [TableViewModelColumn(addressField: nameof(_yAddr), displayOrder: 2, minWidth: 100)]
        [BulkCopy]
        public float Y {
            get => Data.GetCompressedFIXED(_yAddr, 14).Float;
            set => Data.SetCompressedFIXED(_yAddr, new CompressedFIXED(value, 14, 0));
        }

        [TableViewModelColumn(addressField: nameof(_zAddr), displayOrder: 3, minWidth: 100)]
        [BulkCopy]
        public float Z {
            get => Data.GetCompressedFIXED(_zAddr, 14).Float;
            set => Data.SetCompressedFIXED(_zAddr, new CompressedFIXED(value, 14, 0));
        }

        [TableViewModelColumn(addressField: nameof(_wAddr), displayOrder: 4, minWidth: 100)]
        [BulkCopy]
        public float W {
            get => Data.GetCompressedFIXED(_wAddr, 14).Float;
            set => Data.SetCompressedFIXED(_wAddr, new CompressedFIXED(value, 14, 0));
        }

        public QUATERNION CreateQuaternion() => new QUATERNION(X, Y, Z, W);
    }
}
