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
    public class ByteData : IByteData, IDisposable {
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

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
            if (!_disposedValue) {
                if (disposing)
                    Data.RangeModified -= OnDataRangeModified;
                _disposedValue = true;
            }
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
        public byte[] GetDataCopyOrReference() => Data.GetDataCopyOrReference();
        public byte[] GetDataCopyAt(int offset, int length) => Data.GetDataCopyAt(offset, length);

        public uint GetData(int offset, int bytes) {
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (bytes < 1 || bytes > 4)
                throw new ArgumentOutOfRangeException(nameof(bytes));
            if (offset + bytes > Data.Length)
                throw new ArgumentOutOfRangeException(nameof(offset) + " + " + nameof(bytes));

            uint value = 0;
            for (var i = 0; i < bytes; i++)
                value += (uint) Data[offset + i] << (bytes - i - 1) * 8;
            return value;
        }

        public int GetByte(int offset) => (int) GetData(offset, 1);
        public int GetWord(int offset) => (int) GetData(offset, 2);
        public int GetDouble(int offset) => (int) GetData(offset, 4);

        public CompressedFIXED GetCompressedFIXED(int offset) => new CompressedFIXED((short) GetWord(offset));
        public CompressedFIXED GetWeirdCompressedFIXED(int offset) => new CompressedFIXED((ushort) GetWord(offset), isWeird: true);

        public FIXED GetFIXED(int offset) => new FIXED(GetDouble(offset), true);

        public string GetString(int offset, int length) {
            var value = new byte[length];
            for (var i = 0; i < length; i++) {
                if (Data[offset + i] == 0x0)
                    break;
                value[i] = Data[offset + i];
            }
            var InputText = Encoding.GetEncoding("shift-jis");
            return InputText.GetString(value);
        }

        public void SetData(int offset, uint value, int bytes) {
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (bytes < 1 || bytes > 4)
                throw new ArgumentOutOfRangeException(nameof(bytes));
            if (offset + bytes > Data.Length)
                throw new ArgumentOutOfRangeException(nameof(offset) + " + " + nameof(bytes));

            var converted = BitConverter.GetBytes(value);

            for (var i = 0; i < bytes; i++) {
                var b = converted[bytes - i - 1];
                Data[offset + i] = b;
            }
        }

        public void SetByte(int offset, byte value) => SetData(offset, value, 1);
        public void SetWord(int offset, int value) => SetData(offset, (uint) value, 2);
        public void SetDouble(int offset, int value) => SetData(offset, (uint) value, 4);

        public void SetCompressedFIXED(int offset, CompressedFIXED value) => SetWord(offset, value.RawShort);
        public void SetWeirdCompressedFIXED(int offset, CompressedFIXED value) => SetWord(offset, value.WeirdRawShort);

        public void SetFIXED(int offset, FIXED value) => SetDouble(offset, value.RawInt);

        public void SetString(int offset, int length, string value) {
            var encoding = Encoding.GetEncoding("shift-jis");
            var bytes = encoding.GetBytes(value);

            for (var i = 0; i < bytes.Length; i++)
                Data[offset + i] = bytes[i];

            if (bytes.Length < length)
                for (var i = bytes.Length; i < length; i++)
                    Data[offset + i] = 0x00;
        }

        public bool GetBit(int offset, int bit) {
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (bit < 1 || bit > 8)
                throw new ArgumentOutOfRangeException(nameof(bit));
            if (offset >= Data.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));

            return (Data[offset] >> bit - 1 & 0x01) == 1 ? true : false;
        }

        public void SetBit(int offset, int bit, bool value) {
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (bit < 1 || bit > 8)
                throw new ArgumentOutOfRangeException(nameof(bit));
            if (offset >= Data.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));

            var bitmask = (byte)(1 << bit - 1);

            if (value)
                Data[offset] |= bitmask;
            else
                Data[offset] &= (byte) ~bitmask;
        }

        public virtual bool OnFinish() => true;

        public bool Finish() {
            if (!OnFinish())
                return false;
            Finished?.Invoke(this, EventArgs.Empty);
            return true;
        }

        private bool _disposedValue;

        public event EventHandler Finished;
    }
}
