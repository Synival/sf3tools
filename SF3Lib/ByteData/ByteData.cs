using System;
using System.Runtime.InteropServices;
using System.Text;
using CommonLib;
using CommonLib.Arrays;
using CommonLib.SGL;

namespace SF3.ByteData {
    /// <summary>
    /// Used for modifying any set of bytes.
    /// </summary>
    public class ByteData : IByteData {
        public ByteData(IByteArray byteArray) {
            if (byteArray == null)
                throw new NullReferenceException(nameof(byteArray));

            Data = byteArray;
            Data.RangeModified += OnDataRangeModified;

            InitData();
        }

        private void OnDataRangeModified(object sender, ByteArrayRangeModifiedArgs args) {
            // Ignore moving data, which would be a modification for a *parent* byte array,
            // but not the one the ByteData about.
            IsModified = args.Modified || args.Resized;
        }

        public IByteArray Data { get; private set; }

        public int Length => Data.Length;

        private int _isModifiedGuard = 0;
        private bool _isModified = false;

        public virtual bool IsModified {
            get => _isModified;
            set {
                if (_isModifiedGuard == 0 && _isModified != value) {
                    _isModified = value;
                    IsModifiedChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public ScopeGuard IsModifiedChangeBlocker()
            => new ScopeGuard(() => _isModifiedGuard++, () => _isModifiedGuard--);

        public event EventHandler IsModifiedChanged;

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int memcmp(byte[] lhs, byte[] rhs, long count);

        public virtual bool SetDataTo(byte[] data) {
            if (data == null)
                throw new NullReferenceException(nameof(data));
            Data.SetDataTo(data);
            return true;
        }

        protected virtual void InitData() {
        }

        public byte[] GetDataCopy() => Data.GetDataCopy();
        public byte[] GetDataCopyAt(int offset, int length) => Data.GetDataCopyAt(offset, length);

        public uint GetData(int location, int bytes) {
            if (location < 0)
                throw new ArgumentOutOfRangeException(nameof(location));
            if (bytes < 1 || bytes > 4)
                throw new ArgumentOutOfRangeException(nameof(bytes));
            if (location + bytes > Data.Length)
                throw new ArgumentOutOfRangeException(nameof(location) + " + " + nameof(bytes));

            uint value = 0;
            for (var i = 0; i < bytes; i++)
                value += (uint) Data[location + i] << (bytes - i - 1) * 8;
            return value;
        }

        public int GetByte(int location) => (int) GetData(location, 1);
        public int GetWord(int location) => (int) GetData(location, 2);
        public int GetDouble(int location) => (int) GetData(location, 4);

        public CompressedFIXED GetCompressedFIXED(int location) => new CompressedFIXED((short) GetWord(location));

        public CompressedFIXED GetWeirdCompressedFIXED(int location) {
            // These are *weird* -- the *lowest* bit is the signed bit.
            var uWord = (ushort) GetWord(location);
            var sWord = (short) ((uWord & 0x01) << 15 | (uWord >> 1));
            return new CompressedFIXED(sWord);
        }

        public FIXED GetFIXED(int location) => new FIXED(GetDouble(location), true);

        public string GetString(int location, int length) {
            var value = new byte[length];
            for (var i = 0; i < length; i++) {
                if (Data[location + i] == 0x0)
                    break;
                value[i] = Data[location + i];
            }
            var InputText = Encoding.GetEncoding("shift-jis");
            return InputText.GetString(value);
        }

        public void SetData(int location, uint value, int bytes) {
            if (location < 0)
                throw new ArgumentOutOfRangeException(nameof(location));
            if (bytes < 1 || bytes > 4)
                throw new ArgumentOutOfRangeException(nameof(bytes));
            if (location + bytes > Data.Length)
                throw new ArgumentOutOfRangeException(nameof(location) + " + " + nameof(bytes));

            var converted = BitConverter.GetBytes(value);

            for (var i = 0; i < bytes; i++) {
                var b = converted[bytes - i - 1];
                Data[location + i] = b;
            }
        }

        public void SetByte(int location, byte value) => SetData(location, value, 1);
        public void SetWord(int location, int value) => SetData(location, (uint) value, 2);
        public void SetDouble(int location, int value) => SetData(location, (uint) value, 4);

        public void SetCompressedFIXED(int location, CompressedFIXED value) => SetWord(location, value.RawShort);

        public void SetWeirdCompressedFIXED(int location, CompressedFIXED value) {
            // These are *weird* -- the *lowest* bit is the signed bit.
            var uWord = (ushort) value.RawShort;
            SetWord(location, ((value.RawShort & 0x7FFF) << 1) + ((value.RawShort & 0x8000) >> 15));
        }

        public void SetFIXED(int location, FIXED value) => SetDouble(location, value.RawInt);

        public void SetString(int location, int length, string value) {
            var encoding = Encoding.GetEncoding("shift-jis");
            var bytes = encoding.GetBytes(value);

            for (var i = 0; i < bytes.Length; i++)
                Data[location + i] = bytes[i];

            if (bytes.Length < length)
                for (var i = bytes.Length; i < length; i++)
                    Data[location + i] = 0x00;
        }

        public bool GetBit(int location, int bit) {
            if (location < 0)
                throw new ArgumentOutOfRangeException(nameof(location));
            if (bit < 1 || bit > 8)
                throw new ArgumentOutOfRangeException(nameof(bit));
            if (location >= Data.Length)
                throw new ArgumentOutOfRangeException(nameof(location));

            return (Data[location] >> bit - 1 & 0x01) == 1 ? true : false;
        }

        public void SetBit(int location, int bit, bool value) {
            if (location < 0)
                throw new ArgumentOutOfRangeException(nameof(location));
            if (bit < 1 || bit > 8)
                throw new ArgumentOutOfRangeException(nameof(bit));
            if (location >= Data.Length)
                throw new ArgumentOutOfRangeException(nameof(location));

            var bitmask = (byte)(1 << bit - 1);

            if (value)
                Data[location] |= bitmask;
            else
                Data[location] &= (byte) ~bitmask;
        }

        public virtual void Dispose() {
            Data.RangeModified -= OnDataRangeModified;
        }

        public virtual bool OnFinish() => true;

        public bool Finish() {
            if (!OnFinish())
                return false;
            Finished?.Invoke(this, EventArgs.Empty);
            return true;
        }

        public event EventHandler Finished;
    }
}
