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

        private readonly int _layer1XAddr;
        private readonly int _layer1YAddr;
        private readonly int _layer2XAddr;
        private readonly int _layer2YAddr;

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

            _layer1XAddr       = Address + 0x1E; // 1 byte
            _layer1YAddr       = Address + 0x1F; // 1 byte
            _layer2XAddr       = Address + 0x20; // 1 byte
            _layer2YAddr       = Address + 0x21; // 1 byte
        }

        public short GetLayerOffset(int layer, int index) {
            if (layer < 0 || layer > 3)
                throw new ArgumentOutOfRangeException(nameof(layer));
            if (index < 0 || (layer == 0 && index != 0) || (layer == 1 && index > 3) || (layer == 2 && index > 6))
                throw new ArgumentOutOfRangeException(nameof(index));
            return (layer == 0) ? (short) 0 : (short) Data.GetWord((layer - 1) * 0x06 + index * 0x02 + _layer1Offset1Addr);
        }

        public void SetLayerOffset(int layer, int index, short value) {
            if (layer < 1 || layer > 3)
                throw new ArgumentOutOfRangeException(nameof(layer));
            if (index < 1 || (layer == 1 && index > 3) || (layer == 2 && index > 6))
                throw new ArgumentOutOfRangeException(nameof(index));
            Data.SetWord((layer - 1) * 0x06 + index * 0x02 + _layer1Offset1Addr, value);
        }

        public int GetLayerWidth(int layer)
            => (layer == 0) ? Width : (layer == 1) ? Layer1Width : (layer == 2) ? Layer2Width : throw new ArgumentOutOfRangeException(nameof(layer));

        public void SetLayerWidth(int layer, int value) {
            if (layer == 0)
                Width = value;
            else if (layer == 1)
                Layer1Width = (ushort) value;
            else if (layer == 2)
                Layer2Width = (ushort) value;
            else
                throw new ArgumentOutOfRangeException(nameof(layer));
        }

        public int GetLayerHeight(int layer)
            => (layer == 0) ? Height : (layer == 1) ? Layer1Height : (layer == 2) ? Layer2Height : throw new ArgumentOutOfRangeException(nameof(layer));

        public void SetLayerHeight(int layer, int value) {
            if (layer == 0)
                Height = value;
            else if (layer == 1)
                Layer1Height = (ushort) value;
            else if (layer == 2)
                Layer2Height = (ushort) value;
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
        public int Width {
            get => (ushort) Data.GetWord(_widthAddr);
            set {
                Data.SetWord(_widthAddr, value);
                if (Data.GetWord(_widthAddr) != value) {
                    Data.SetWord(_widthAddr, value);
                    OnDimensionsChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        [TableViewModelColumn(addressField: nameof(_heightAddr), displayOrder: 2)]
        [BulkCopy]
        public int Height {
            get => (ushort) Data.GetWord(_heightAddr);
            set {
                if (Data.GetWord(_heightAddr) != value) {
                    Data.SetWord(_heightAddr, value);
                    OnDimensionsChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        [TableViewModelColumn(addressField: nameof(_layer1Offset1Addr), displayOrder: 3, displayFormat: "-X4", displayName: "L1_Off1")]
        [BulkCopy]
        public short Layer1Offset1 {
            get => (short) Data.GetWord(_layer1Offset1Addr);
            set => Data.SetWord(_layer1Offset1Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_layer1Offset2Addr), displayOrder: 4, displayFormat: "-X4", displayName: "L1_Off2")]
        [BulkCopy]
        public short Layer1Offset2 {
            get => (short) Data.GetWord(_layer1Offset2Addr);
            set => Data.SetWord(_layer1Offset2Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_layer1Offset3Addr), displayOrder: 5, displayFormat: "-X4", displayName: "L1_Off3")]
        [BulkCopy]
        public short Layer1Offset3 {
            get => (short) Data.GetWord(_layer1Offset3Addr);
            set => Data.SetWord(_layer1Offset3Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_layer2Offset1Addr), displayOrder: 6, displayFormat: "-X4", displayName: "L2_Off1")]
        [BulkCopy]
        public short Layer2Offset1 {
            get => (short) Data.GetWord(_layer2Offset1Addr);
            set => Data.SetWord(_layer2Offset1Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_layer2Offset2Addr), displayOrder: 7, displayFormat: "-X4", displayName: "L2_Off2")]
        [BulkCopy]
        public short Layer2Offset2 {
            get => (short) Data.GetWord(_layer2Offset2Addr);
            set => Data.SetWord(_layer2Offset2Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_layer2Offset3Addr), displayOrder: 8, displayFormat: "-X4", displayName: "L2_Off3")]
        [BulkCopy]
        public short Layer2Offset3 {
            get => (short) Data.GetWord(_layer2Offset3Addr);
            set => Data.SetWord(_layer2Offset3Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_layer2Offset4Addr), displayOrder: 9, displayFormat: "-X4", displayName: "L2_Off4")]
        [BulkCopy]
        public short Layer2Offset4 {
            get => (short) Data.GetWord(_layer2Offset4Addr);
            set => Data.SetWord(_layer2Offset4Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_layer2Offset5Addr), displayOrder: 10, displayFormat: "-X4", displayName: "L2_Off5")]
        [BulkCopy]
        public short Layer2Offset5 {
            get => (short) Data.GetWord(_layer2Offset5Addr);
            set => Data.SetWord(_layer2Offset5Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_layer2Offset6Addr), displayOrder: 11, displayFormat: "-X4", displayName: "L2_Off6")]
        [BulkCopy]
        public short Layer2Offset6 {
            get => (short) Data.GetWord(_layer2Offset6Addr);
            set => Data.SetWord(_layer2Offset6Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_layer1WidthAddr), displayOrder: 12, displayName: "L1_Width")]
        [BulkCopy]
        public ushort Layer1Width {
            get => (ushort) Data.GetWord(_layer1WidthAddr);
            set => Data.SetWord(_layer1WidthAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_layer1HeightAddr), displayOrder: 13, displayName: "L1_Height")]
        [BulkCopy]
        public ushort Layer1Height {
            get => (ushort) Data.GetWord(_layer1HeightAddr);
            set => Data.SetWord(_layer1HeightAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_layer2WidthAddr), displayOrder: 14, displayName: "L2_Width")]
        [BulkCopy]
        public ushort Layer2Width {
            get => (ushort) Data.GetWord(_layer2WidthAddr);
            set => Data.SetWord(_layer2WidthAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_layer2HeightAddr), displayOrder: 15, displayName: "L2_Height")]
        [BulkCopy]
        public ushort Layer2Height {
            get => (ushort) Data.GetWord(_layer2HeightAddr);
            set => Data.SetWord(_layer2HeightAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_layer1XAddr), displayOrder: 16, displayName: "L1_X")]
        [BulkCopy]
        public sbyte Layer1X {
            get => (sbyte) Data.GetByte(_layer1XAddr);
            set => Data.SetByte(_layer1XAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_layer1YAddr), displayOrder: 17, displayName: "L1_Y")]
        [BulkCopy]
        public sbyte Layer1Y {
            get => (sbyte) Data.GetByte(_layer1YAddr);
            set => Data.SetByte(_layer1YAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_layer2XAddr), displayOrder: 18, displayName: "L2_X")]
        [BulkCopy]
        public sbyte Layer2X {
            get => (sbyte) Data.GetByte(_layer2XAddr);
            set => Data.SetByte(_layer2XAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_layer2YAddr), displayOrder: 19, displayName: "L2_Y")]
        [BulkCopy]
        public sbyte Layer2Y {
            get => (sbyte) Data.GetByte(_layer2YAddr);
            set => Data.SetByte(_layer2YAddr, (byte) value);
        }

        public EventHandler OnDimensionsChanged;
    }
}
