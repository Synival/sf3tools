using System;
using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.KAO {
    public class FaceHeader : Struct {
        private readonly int _widthAddr;
        private readonly int _heightAddr;

        private readonly int _layer1Offset1Addr;
        private readonly int _layer1Offset2Addr;
        private readonly int _layer1Offset3Addr;

        private readonly int _layer2Offset1Addr;
        private readonly int _layer2Offset2Addr;
        private readonly int _layer2Offset3Addr;
        private readonly int _layer2Offset4Addr;
        private readonly int _layer2Offset5Addr;
        private readonly int _layer2Offset6Addr;

        private readonly int _layer1WidthAddr;
        private readonly int _layer1HeightAddr;
        private readonly int _layer2WidthAddr;
        private readonly int _layer2HeightAddr;

        private readonly int _layer1RelativeXAddr;
        private readonly int _layer1RelativeYAddr;
        private readonly int _layer2RelativeXAddr;
        private readonly int _layer2RelativeYAddr;

        public FaceHeader(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x22) {
            _widthAddr         = Address + 0x00; // 2 bytes
            _heightAddr        = Address + 0x02; // 2 bytes

            _layer1Offset1Addr = Address + 0x04; // 2 bytes
            _layer1Offset2Addr = Address + 0x06; // 2 bytes
            _layer1Offset3Addr = Address + 0x08; // 2 bytes

            _layer2Offset1Addr = Address + 0x0A; // 2 bytes
            _layer2Offset2Addr = Address + 0x0C; // 2 bytes
            _layer2Offset3Addr = Address + 0x0E; // 2 bytes
            _layer2Offset4Addr = Address + 0x10; // 2 bytes
            _layer2Offset5Addr = Address + 0x12; // 2 bytes
            _layer2Offset6Addr = Address + 0x14; // 2 bytes

            _layer1WidthAddr   = Address + 0x16; // 2 bytes
            _layer1HeightAddr  = Address + 0x18; // 2 bytes
            _layer2WidthAddr   = Address + 0x1A; // 2 bytes
            _layer2HeightAddr  = Address + 0x1C; // 2 bytes

            _layer1RelativeXAddr = Address + 0x1E; // 1 byte
            _layer1RelativeYAddr = Address + 0x1F; // 1 byte
            _layer2RelativeXAddr = Address + 0x20; // 1 byte
            _layer2RelativeYAddr = Address + 0x21; // 1 byte
        }

        public short GetLayerOffset(int layer, int index) {
            if (layer < 0 || layer > 3)
                throw new ArgumentOutOfRangeException(nameof(layer));
            if (index < 0 || (layer == 0 && index != 0) || (layer == 1 && index > 3) || (layer == 2 && index > 6))
                throw new ArgumentOutOfRangeException(nameof(index));
            return (layer == 0) ? (short) 0 : Data.GetInt16((layer - 1) * 0x06 + index * 0x02 + _layer1Offset1Addr);
        }

        public void SetLayerOffset(int layer, int index, short value) {
            if (layer < 1 || layer > 3)
                throw new ArgumentOutOfRangeException(nameof(layer));
            if (index < 1 || (layer == 1 && index > 3) || (layer == 2 && index > 6))
                throw new ArgumentOutOfRangeException(nameof(index));
            Data.SetInt16((layer - 1) * 0x06 + index * 0x02 + _layer1Offset1Addr, value);
        }

        public int GetLayerWidth(int layer)
            => (layer == 0) ? Width : (layer == 1) ? Layer1Width : (layer == 2) ? Layer2Width : throw new ArgumentOutOfRangeException(nameof(layer));

        public void SetLayerWidth(int layer, ushort value) {
            if (layer == 0)
                Width = value;
            else if (layer == 1)
                Layer1Width = value;
            else if (layer == 2)
                Layer2Width = value;
            else
                throw new ArgumentOutOfRangeException(nameof(layer));
        }

        public int GetLayerHeight(int layer)
            => (layer == 0) ? Height : (layer == 1) ? Layer1Height : (layer == 2) ? Layer2Height : throw new ArgumentOutOfRangeException(nameof(layer));

        public void SetLayerHeight(int layer, ushort value) {
            if (layer == 0)
                Height = value;
            else if (layer == 1)
                Layer1Height = value;
            else if (layer == 2)
                Layer2Height = value;
            else
                throw new ArgumentOutOfRangeException(nameof(layer));
        }

        public int GetLayerRelativeX(int layer) 
            => (layer == 0) ? 0 : (layer == 1) ? Layer1RelativeX : (layer == 2) ? Layer2RelativeX : throw new ArgumentOutOfRangeException(nameof(layer));

        public void SetLayerRelativeX(int layer, int value) {
            if (layer == 1)
                Layer1RelativeX = (sbyte) value;
            else if (layer == 2)
                Layer2RelativeX = (sbyte) value;
            else
                throw new ArgumentOutOfRangeException(nameof(layer));
        }

        public int GetLayerRelativeY(int layer) 
            => (layer == 0) ? 0 : (layer == 1) ? Layer1RelativeY : (layer == 2) ? Layer2RelativeY : throw new ArgumentOutOfRangeException(nameof(layer));

        public void SetLayerRelativeY(int layer, int value) {
            if (layer == 1)
                Layer1RelativeY = (sbyte) value;
            else if (layer == 2)
                Layer2RelativeY = (sbyte) value;
            else
                throw new ArgumentOutOfRangeException(nameof(layer));
        }

        public int GetLayerX(int layer) 
            => (layer == 0) ? 0 : (layer == 1) ? Layer1X : (layer == 2) ? Layer2X : throw new ArgumentOutOfRangeException(nameof(layer));

        public void SetLayerX(int layer, int value) {
            if (layer == 1)
                Layer1X = (sbyte) value;
            else if (layer == 2)
                Layer2X = (sbyte) value;
            else
                throw new ArgumentOutOfRangeException(nameof(layer));
        }

        public int GetLayerY(int layer) 
            => (layer == 0) ? 0 : (layer == 1) ? Layer1Y : (layer == 2) ? Layer2Y : throw new ArgumentOutOfRangeException(nameof(layer));

        public void SetLayerY(int layer, int value) {
            if (layer == 1)
                Layer1Y = (sbyte) value;
            else if (layer == 2)
                Layer2Y = (sbyte) value;
            else
                throw new ArgumentOutOfRangeException(nameof(layer));
        }

        [TableViewModelColumn(addressField: nameof(_widthAddr), displayOrder: 1)]
        [BulkCopy]
        public ushort Width {
            get => Data.GetUInt16(_widthAddr);
            set {
                Data.SetUInt16(_widthAddr, value);
                if (Data.GetUInt16(_widthAddr) != value) {
                    Data.SetUInt16(_widthAddr, value);
                    OnDimensionsChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        [TableViewModelColumn(addressField: nameof(_heightAddr), displayOrder: 2)]
        [BulkCopy]
        public ushort Height {
            get => Data.GetUInt16(_heightAddr);
            set {
                if (Data.GetUInt16(_heightAddr) != value) {
                    Data.SetUInt16(_heightAddr, value);
                    OnDimensionsChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        [TableViewModelColumn(addressField: nameof(_layer1Offset1Addr), displayOrder: 3, displayFormat: "-X4", displayName: "L1_Off1")]
        [BulkCopy]
        public short Layer1Offset1 {
            get => Data.GetInt16(_layer1Offset1Addr);
            set => Data.SetInt16(_layer1Offset1Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_layer1Offset2Addr), displayOrder: 4, displayFormat: "-X4", displayName: "L1_Off2")]
        [BulkCopy]
        public short Layer1Offset2 {
            get => Data.GetInt16(_layer1Offset2Addr);
            set => Data.SetInt16(_layer1Offset2Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_layer1Offset3Addr), displayOrder: 5, displayFormat: "-X4", displayName: "L1_Off3")]
        [BulkCopy]
        public short Layer1Offset3 {
            get => Data.GetInt16(_layer1Offset3Addr);
            set => Data.SetInt16(_layer1Offset3Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_layer2Offset1Addr), displayOrder: 6, displayFormat: "-X4", displayName: "L2_Off1")]
        [BulkCopy]
        public short Layer2Offset1 {
            get => Data.GetInt16(_layer2Offset1Addr);
            set => Data.SetInt16(_layer2Offset1Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_layer2Offset2Addr), displayOrder: 7, displayFormat: "-X4", displayName: "L2_Off2")]
        [BulkCopy]
        public short Layer2Offset2 {
            get => Data.GetInt16(_layer2Offset2Addr);
            set => Data.SetInt16(_layer2Offset2Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_layer2Offset3Addr), displayOrder: 8, displayFormat: "-X4", displayName: "L2_Off3")]
        [BulkCopy]
        public short Layer2Offset3 {
            get => Data.GetInt16(_layer2Offset3Addr);
            set => Data.SetInt16(_layer2Offset3Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_layer2Offset4Addr), displayOrder: 9, displayFormat: "-X4", displayName: "L2_Off4")]
        [BulkCopy]
        public short Layer2Offset4 {
            get => Data.GetInt16(_layer2Offset4Addr);
            set => Data.SetInt16(_layer2Offset4Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_layer2Offset5Addr), displayOrder: 10, displayFormat: "-X4", displayName: "L2_Off5")]
        [BulkCopy]
        public short Layer2Offset5 {
            get => Data.GetInt16(_layer2Offset5Addr);
            set => Data.SetInt16(_layer2Offset5Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_layer2Offset6Addr), displayOrder: 11, displayFormat: "-X4", displayName: "L2_Off6")]
        [BulkCopy]
        public short Layer2Offset6 {
            get => Data.GetInt16(_layer2Offset6Addr);
            set => Data.SetInt16(_layer2Offset6Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_layer1WidthAddr), displayOrder: 12, displayName: "L1_Width")]
        [BulkCopy]
        public ushort Layer1Width {
            get => Data.GetUInt16(_layer1WidthAddr);
            set => Data.SetUInt16(_layer1WidthAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_layer1HeightAddr), displayOrder: 13, displayName: "L1_Height")]
        [BulkCopy]
        public ushort Layer1Height {
            get => Data.GetUInt16(_layer1HeightAddr);
            set => Data.SetUInt16(_layer1HeightAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_layer2WidthAddr), displayOrder: 14, displayName: "L2_Width")]
        [BulkCopy]
        public ushort Layer2Width {
            get => Data.GetUInt16(_layer2WidthAddr);
            set => Data.SetUInt16(_layer2WidthAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_layer2HeightAddr), displayOrder: 15, displayName: "L2_Height")]
        [BulkCopy]
        public ushort Layer2Height {
            get => Data.GetUInt16(_layer2HeightAddr);
            set => Data.SetUInt16(_layer2HeightAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_layer1RelativeXAddr), displayOrder: 16, displayName: "L1_RelX")]
        [BulkCopy]
        public sbyte Layer1RelativeX {
            get => Data.GetInt8(_layer1RelativeXAddr);
            set => Data.SetInt8(_layer1RelativeXAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_layer1RelativeYAddr), displayOrder: 17, displayName: "L1_RelY")]
        [BulkCopy]
        public sbyte Layer1RelativeY {
            get => Data.GetInt8(_layer1RelativeYAddr);
            set => Data.SetInt8(_layer1RelativeYAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_layer2RelativeXAddr), displayOrder: 18, displayName: "L2_RelX")]
        [BulkCopy]
        public sbyte Layer2RelativeX {
            get => Data.GetInt8(_layer2RelativeXAddr);
            set => Data.SetInt8(_layer2RelativeXAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_layer2RelativeYAddr), displayOrder: 19, displayName: "L2_RelY")]
        [BulkCopy]
        public sbyte Layer2RelativeY {
            get => Data.GetInt8(_layer2RelativeYAddr);
            set => Data.SetInt8(_layer2RelativeYAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_layer2RelativeYAddr), displayOrder: 20, displayName: "L1_X")]
        public sbyte Layer1X {
            get => (sbyte) ((Width - Layer1Width) / 2 + Layer1RelativeX);
            set => Layer1RelativeX = (sbyte) (value - (Width - Layer1Width) / 2);
        }

        [TableViewModelColumn(addressField: nameof(_layer2RelativeYAddr), displayOrder: 21, displayName: "L1_Y")]
        public sbyte Layer1Y {
            get => (sbyte) ((Height - Layer1Height) / 2 + Layer1RelativeY);
            set => Layer1RelativeY = (sbyte) (value - (Height - Layer1Height) / 2);
        }

        [TableViewModelColumn(addressField: nameof(_layer2RelativeYAddr), displayOrder: 22, displayName: "L2_X")]
        public sbyte Layer2X {
            get => (sbyte) ((Width - Layer2Width) / 2 + Layer2RelativeX);
            set => Layer2RelativeX = (sbyte) (value - (Width - Layer2Width) / 2);
        }

        [TableViewModelColumn(addressField: nameof(_layer2RelativeYAddr), displayOrder: 23, displayName: "L2_Y")]
        public sbyte Layer2Y {
            get => (sbyte) ((Height - Layer2Height) / 2 + Layer2RelativeY);
            set => Layer2RelativeY = (sbyte) (value - (Height - Layer2Height) / 2);
        }

        public event EventHandler OnDimensionsChanged;
    }
}
