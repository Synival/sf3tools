﻿using System;
using System.Collections;
using System.Collections.Generic;
using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.CHR {
    public class MultipleFrameDataOffsetsRow : Struct, IEnumerable<uint> {
        public MultipleFrameDataOffsetsRow(IByteData data, int id, string name, int address, uint dataOffset, int spriteId) : base(data, id, name, address, 0x00) {
            DataOffset = dataOffset;
            SpriteID = spriteId;

            int length = 0;
            while (Data.GetDouble(address + length * 0x04) != 0x0000)
                length++;

            Length = length;
            Size = length * 0x04;
        }

        public uint DataOffset { get; }

        [TableViewModelColumn(addressField: null, displayOrder: -0.2f, displayFormat: "X2", minWidth: 200)]
        [NameGetter(NamedValueType.Sprite)]
        public int SpriteID { get; }

        [TableViewModelColumn(addressField: null, displayOrder: -0.1f)]
        public int Length { get; }

        uint? GetOffset(int index)
            => (index >= 0 && index < Length) ? (uint) Data.GetDouble(Address + index * 0x04) : (uint?) null;

        void SetOffset(int index, uint? value) {
            if (value.HasValue && index >= 0 && index < Length)
                SetOffset(0, value);
        }

        public IEnumerable<uint> GetOffsets() {
            var offsets = new uint[Length];
            for (int i = 0; i < offsets.Length; i++)
                offsets[i] = (uint) Data.GetDouble(Address + i * 0x04);
            return offsets;
        }

        public IEnumerator<uint> GetEnumerator() => GetOffsets().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetOffsets().GetEnumerator();

        public uint this[int index] {
            get => (index >= 0 && index < Length) ? GetOffset(index).Value : throw new ArgumentOutOfRangeException(nameof(index));
            set {
                if (index >= 0 && index < Length)
                    SetOffset(index, value);
                else
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        // Gigantic table of properties so it's visible to the ObjectListView (eeeeew)
        [TableViewModelColumn(displayOrder:  0, isPointer: true)] [BulkCopy] public uint? Offset01 { get => GetOffset( 0); set => SetOffset( 0, value); }
        [TableViewModelColumn(displayOrder:  1, isPointer: true)] [BulkCopy] public uint? Offset02 { get => GetOffset( 1); set => SetOffset( 1, value); }
        [TableViewModelColumn(displayOrder:  2, isPointer: true)] [BulkCopy] public uint? Offset03 { get => GetOffset( 2); set => SetOffset( 2, value); }
        [TableViewModelColumn(displayOrder:  3, isPointer: true)] [BulkCopy] public uint? Offset04 { get => GetOffset( 3); set => SetOffset( 3, value); }
        [TableViewModelColumn(displayOrder:  4, isPointer: true)] [BulkCopy] public uint? Offset05 { get => GetOffset( 4); set => SetOffset( 4, value); }
        [TableViewModelColumn(displayOrder:  5, isPointer: true)] [BulkCopy] public uint? Offset06 { get => GetOffset( 5); set => SetOffset( 5, value); }
        [TableViewModelColumn(displayOrder:  6, isPointer: true)] [BulkCopy] public uint? Offset07 { get => GetOffset( 6); set => SetOffset( 6, value); }
        [TableViewModelColumn(displayOrder:  7, isPointer: true)] [BulkCopy] public uint? Offset08 { get => GetOffset( 7); set => SetOffset( 7, value); }
        [TableViewModelColumn(displayOrder:  8, isPointer: true)] [BulkCopy] public uint? Offset09 { get => GetOffset( 8); set => SetOffset( 8, value); }
        [TableViewModelColumn(displayOrder:  9, isPointer: true)] [BulkCopy] public uint? Offset10 { get => GetOffset( 9); set => SetOffset( 9, value); }
        [TableViewModelColumn(displayOrder: 10, isPointer: true)] [BulkCopy] public uint? Offset11 { get => GetOffset(10); set => SetOffset(10, value); }
        [TableViewModelColumn(displayOrder: 11, isPointer: true)] [BulkCopy] public uint? Offset12 { get => GetOffset(11); set => SetOffset(11, value); }
        [TableViewModelColumn(displayOrder: 12, isPointer: true)] [BulkCopy] public uint? Offset13 { get => GetOffset(12); set => SetOffset(12, value); }
        [TableViewModelColumn(displayOrder: 13, isPointer: true)] [BulkCopy] public uint? Offset14 { get => GetOffset(13); set => SetOffset(13, value); }
        [TableViewModelColumn(displayOrder: 14, isPointer: true)] [BulkCopy] public uint? Offset15 { get => GetOffset(14); set => SetOffset(14, value); }
        [TableViewModelColumn(displayOrder: 15, isPointer: true)] [BulkCopy] public uint? Offset16 { get => GetOffset(15); set => SetOffset(15, value); }
        [TableViewModelColumn(displayOrder: 16, isPointer: true)] [BulkCopy] public uint? Offset17 { get => GetOffset(16); set => SetOffset(16, value); }
        [TableViewModelColumn(displayOrder: 17, isPointer: true)] [BulkCopy] public uint? Offset18 { get => GetOffset(17); set => SetOffset(17, value); }
        [TableViewModelColumn(displayOrder: 18, isPointer: true)] [BulkCopy] public uint? Offset19 { get => GetOffset(18); set => SetOffset(18, value); }
        [TableViewModelColumn(displayOrder: 19, isPointer: true)] [BulkCopy] public uint? Offset20 { get => GetOffset(19); set => SetOffset(19, value); }
        [TableViewModelColumn(displayOrder: 20, isPointer: true)] [BulkCopy] public uint? Offset21 { get => GetOffset(20); set => SetOffset(20, value); }
        [TableViewModelColumn(displayOrder: 21, isPointer: true)] [BulkCopy] public uint? Offset22 { get => GetOffset(21); set => SetOffset(21, value); }
        [TableViewModelColumn(displayOrder: 22, isPointer: true)] [BulkCopy] public uint? Offset23 { get => GetOffset(22); set => SetOffset(22, value); }
        [TableViewModelColumn(displayOrder: 23, isPointer: true)] [BulkCopy] public uint? Offset24 { get => GetOffset(23); set => SetOffset(23, value); }
        [TableViewModelColumn(displayOrder: 24, isPointer: true)] [BulkCopy] public uint? Offset25 { get => GetOffset(24); set => SetOffset(24, value); }
        [TableViewModelColumn(displayOrder: 25, isPointer: true)] [BulkCopy] public uint? Offset26 { get => GetOffset(25); set => SetOffset(25, value); }
        [TableViewModelColumn(displayOrder: 26, isPointer: true)] [BulkCopy] public uint? Offset27 { get => GetOffset(26); set => SetOffset(26, value); }
        [TableViewModelColumn(displayOrder: 27, isPointer: true)] [BulkCopy] public uint? Offset28 { get => GetOffset(27); set => SetOffset(27, value); }
        [TableViewModelColumn(displayOrder: 28, isPointer: true)] [BulkCopy] public uint? Offset29 { get => GetOffset(28); set => SetOffset(28, value); }
        [TableViewModelColumn(displayOrder: 29, isPointer: true)] [BulkCopy] public uint? Offset30 { get => GetOffset(29); set => SetOffset(29, value); }
        [TableViewModelColumn(displayOrder: 30, isPointer: true)] [BulkCopy] public uint? Offset31 { get => GetOffset(30); set => SetOffset(30, value); }
        [TableViewModelColumn(displayOrder: 31, isPointer: true)] [BulkCopy] public uint? Offset32 { get => GetOffset(31); set => SetOffset(31, value); }
        [TableViewModelColumn(displayOrder: 32, isPointer: true)] [BulkCopy] public uint? Offset33 { get => GetOffset(32); set => SetOffset(32, value); }
        [TableViewModelColumn(displayOrder: 33, isPointer: true)] [BulkCopy] public uint? Offset34 { get => GetOffset(33); set => SetOffset(33, value); }
        [TableViewModelColumn(displayOrder: 34, isPointer: true)] [BulkCopy] public uint? Offset35 { get => GetOffset(34); set => SetOffset(34, value); }
        [TableViewModelColumn(displayOrder: 35, isPointer: true)] [BulkCopy] public uint? Offset36 { get => GetOffset(35); set => SetOffset(35, value); }
        [TableViewModelColumn(displayOrder: 36, isPointer: true)] [BulkCopy] public uint? Offset37 { get => GetOffset(36); set => SetOffset(36, value); }
        [TableViewModelColumn(displayOrder: 37, isPointer: true)] [BulkCopy] public uint? Offset38 { get => GetOffset(37); set => SetOffset(37, value); }
        [TableViewModelColumn(displayOrder: 38, isPointer: true)] [BulkCopy] public uint? Offset39 { get => GetOffset(38); set => SetOffset(38, value); }
        [TableViewModelColumn(displayOrder: 39, isPointer: true)] [BulkCopy] public uint? Offset40 { get => GetOffset(39); set => SetOffset(39, value); }
        [TableViewModelColumn(displayOrder: 40, isPointer: true)] [BulkCopy] public uint? Offset41 { get => GetOffset(40); set => SetOffset(40, value); }
        [TableViewModelColumn(displayOrder: 41, isPointer: true)] [BulkCopy] public uint? Offset42 { get => GetOffset(41); set => SetOffset(41, value); }
        [TableViewModelColumn(displayOrder: 42, isPointer: true)] [BulkCopy] public uint? Offset43 { get => GetOffset(42); set => SetOffset(42, value); }
        [TableViewModelColumn(displayOrder: 43, isPointer: true)] [BulkCopy] public uint? Offset44 { get => GetOffset(43); set => SetOffset(43, value); }
        [TableViewModelColumn(displayOrder: 44, isPointer: true)] [BulkCopy] public uint? Offset45 { get => GetOffset(44); set => SetOffset(44, value); }
        [TableViewModelColumn(displayOrder: 45, isPointer: true)] [BulkCopy] public uint? Offset46 { get => GetOffset(45); set => SetOffset(45, value); }
        [TableViewModelColumn(displayOrder: 46, isPointer: true)] [BulkCopy] public uint? Offset47 { get => GetOffset(46); set => SetOffset(46, value); }
        [TableViewModelColumn(displayOrder: 47, isPointer: true)] [BulkCopy] public uint? Offset48 { get => GetOffset(47); set => SetOffset(47, value); }
        [TableViewModelColumn(displayOrder: 48, isPointer: true)] [BulkCopy] public uint? Offset49 { get => GetOffset(48); set => SetOffset(48, value); }
        [TableViewModelColumn(displayOrder: 49, isPointer: true)] [BulkCopy] public uint? Offset50 { get => GetOffset(49); set => SetOffset(49, value); }
        [TableViewModelColumn(displayOrder: 50, isPointer: true)] [BulkCopy] public uint? Offset51 { get => GetOffset(50); set => SetOffset(50, value); }
        [TableViewModelColumn(displayOrder: 51, isPointer: true)] [BulkCopy] public uint? Offset52 { get => GetOffset(51); set => SetOffset(51, value); }
        [TableViewModelColumn(displayOrder: 52, isPointer: true)] [BulkCopy] public uint? Offset53 { get => GetOffset(52); set => SetOffset(52, value); }
        [TableViewModelColumn(displayOrder: 53, isPointer: true)] [BulkCopy] public uint? Offset54 { get => GetOffset(53); set => SetOffset(53, value); }
        [TableViewModelColumn(displayOrder: 54, isPointer: true)] [BulkCopy] public uint? Offset55 { get => GetOffset(54); set => SetOffset(54, value); }
        [TableViewModelColumn(displayOrder: 55, isPointer: true)] [BulkCopy] public uint? Offset56 { get => GetOffset(55); set => SetOffset(55, value); }
        [TableViewModelColumn(displayOrder: 56, isPointer: true)] [BulkCopy] public uint? Offset57 { get => GetOffset(56); set => SetOffset(56, value); }
        [TableViewModelColumn(displayOrder: 57, isPointer: true)] [BulkCopy] public uint? Offset58 { get => GetOffset(57); set => SetOffset(57, value); }
        [TableViewModelColumn(displayOrder: 58, isPointer: true)] [BulkCopy] public uint? Offset59 { get => GetOffset(58); set => SetOffset(58, value); }
        [TableViewModelColumn(displayOrder: 59, isPointer: true)] [BulkCopy] public uint? Offset60 { get => GetOffset(59); set => SetOffset(59, value); }
        [TableViewModelColumn(displayOrder: 60, isPointer: true)] [BulkCopy] public uint? Offset61 { get => GetOffset(60); set => SetOffset(60, value); }
        [TableViewModelColumn(displayOrder: 61, isPointer: true)] [BulkCopy] public uint? Offset62 { get => GetOffset(61); set => SetOffset(61, value); }
        [TableViewModelColumn(displayOrder: 62, isPointer: true)] [BulkCopy] public uint? Offset63 { get => GetOffset(62); set => SetOffset(62, value); }
        [TableViewModelColumn(displayOrder: 63, isPointer: true)] [BulkCopy] public uint? Offset64 { get => GetOffset(63); set => SetOffset(63, value); }
        [TableViewModelColumn(displayOrder: 64, isPointer: true)] [BulkCopy] public uint? Offset65 { get => GetOffset(64); set => SetOffset(64, value); }
        [TableViewModelColumn(displayOrder: 65, isPointer: true)] [BulkCopy] public uint? Offset66 { get => GetOffset(65); set => SetOffset(65, value); }
        [TableViewModelColumn(displayOrder: 66, isPointer: true)] [BulkCopy] public uint? Offset67 { get => GetOffset(66); set => SetOffset(66, value); }
        [TableViewModelColumn(displayOrder: 67, isPointer: true)] [BulkCopy] public uint? Offset68 { get => GetOffset(67); set => SetOffset(67, value); }
        [TableViewModelColumn(displayOrder: 68, isPointer: true)] [BulkCopy] public uint? Offset69 { get => GetOffset(68); set => SetOffset(68, value); }
        [TableViewModelColumn(displayOrder: 69, isPointer: true)] [BulkCopy] public uint? Offset70 { get => GetOffset(69); set => SetOffset(69, value); }
        [TableViewModelColumn(displayOrder: 70, isPointer: true)] [BulkCopy] public uint? Offset71 { get => GetOffset(70); set => SetOffset(70, value); }
        [TableViewModelColumn(displayOrder: 71, isPointer: true)] [BulkCopy] public uint? Offset72 { get => GetOffset(71); set => SetOffset(71, value); }
        [TableViewModelColumn(displayOrder: 72, isPointer: true)] [BulkCopy] public uint? Offset73 { get => GetOffset(72); set => SetOffset(72, value); }
        [TableViewModelColumn(displayOrder: 73, isPointer: true)] [BulkCopy] public uint? Offset74 { get => GetOffset(73); set => SetOffset(73, value); }
        [TableViewModelColumn(displayOrder: 74, isPointer: true)] [BulkCopy] public uint? Offset75 { get => GetOffset(74); set => SetOffset(74, value); }
        [TableViewModelColumn(displayOrder: 75, isPointer: true)] [BulkCopy] public uint? Offset76 { get => GetOffset(75); set => SetOffset(75, value); }
        [TableViewModelColumn(displayOrder: 76, isPointer: true)] [BulkCopy] public uint? Offset77 { get => GetOffset(76); set => SetOffset(76, value); }
        [TableViewModelColumn(displayOrder: 77, isPointer: true)] [BulkCopy] public uint? Offset78 { get => GetOffset(77); set => SetOffset(77, value); }
        [TableViewModelColumn(displayOrder: 78, isPointer: true)] [BulkCopy] public uint? Offset79 { get => GetOffset(78); set => SetOffset(78, value); }
        [TableViewModelColumn(displayOrder: 79, isPointer: true)] [BulkCopy] public uint? Offset80 { get => GetOffset(79); set => SetOffset(79, value); }
    }
}
