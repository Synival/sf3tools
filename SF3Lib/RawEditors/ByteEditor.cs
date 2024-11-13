using System;
using System.IO;
using System.Text;
using SF3.Exceptions;

namespace SF3.RawEditors {
    /// <summary>
    /// Used for modifying any set of bytes.
    /// </summary>
    public class ByteEditor : IByteEditor {
        public ByteEditor(byte[] data) {
            _ = SetData(data);
        }

        public byte[] Data { get; private set; }

        public int Size => Data.Length;

        private bool _isModified = false;

        public bool IsModified {
            get => _isModified;
            set {
                if (_isModified != value) {
                    _isModified = value;
                    IsModifiedChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public virtual bool SetData(byte[] data) {
            Data = data;
            IsModified = false;
            return true;
        }

        public byte[] GetAllData() {
            var newData = new byte[Data.Length];
            Data.CopyTo(newData, 0);
            return Data;
        }

        public uint GetData(int location, int bytes) {
            if (location < 0)
                throw new ArgumentOutOfRangeException(nameof(location));
            if (bytes < 1 || bytes > 4)
                throw new ArgumentOutOfRangeException(nameof(bytes));
            if (Data == null)
                throw new FileEditorNotLoadedException();
            if (location + bytes > Data.Length)
                throw new FileEditorReadException();

            uint value = 0;
            for (var i = 0; i < bytes; i++)
                value += (uint) Data[location + i] << (bytes - i - 1) * 8;
            return value;
        }

        public int GetByte(int location) => (int) GetData(location, 1);
        public int GetWord(int location) => (int) GetData(location, 2);
        public int GetDouble(int location) => (int) GetData(location, 4);

        public string GetString(int location, int length) {
            if (Data == null)
                throw new FileEditorNotLoadedException();

            var value = new byte[length];
            for (var i = 0; i < length; i++) {
                try {
                    if (Data[location + i] == 0x0)
                        break;
                    value[i] = Data[location + i];
                }
                catch (IndexOutOfRangeException) {
                    //wrong kind of file was selected to load
                    throw new FileEditorReadException();
                }
            }
            var InputText = Encoding.GetEncoding("shift-jis");
            return InputText.GetString(value);
        }

        public void SetData(int location, uint value, int bytes) {
            if (location < 0)
                throw new ArgumentOutOfRangeException(nameof(location));
            if (bytes < 1 || bytes > 4)
                throw new ArgumentOutOfRangeException(nameof(bytes));
            if (Data == null)
                throw new FileEditorNotLoadedException();
            if (location + bytes > Data.Length)
                throw new FileEditorReadException();

            var converted = BitConverter.GetBytes(value);

            for (var i = 0; i < bytes; i++) {
                var b = converted[bytes - i - 1];
                if (Data[location + i] != b) {
                    Data[location + i] = b;
                    IsModified = true;
                }
            }
        }

        public void SetByte(int location, byte value) => SetData(location, value, 1);
        public void SetWord(int location, int value) => SetData(location, (uint) value, 2);
        public void SetDouble(int location, int value) => SetData(location, (uint) value, 4);

        public void SetString(int location, int length, string value) {
            if (Data == null)
                throw new FileEditorNotLoadedException();
            var OutputText = Encoding.GetEncoding("shift-jis");
            var name = OutputText.GetBytes(value);
            for (var i = 0; i < name.Length; i++) {
                if (Data[location + i] != name[i]) {
                    Data[location + i] = name[i];
                    IsModified = true;
                }
            }
            if (name.Length < length) {
                for (var i = name.Length; i < length; i++) {
                    if (Data[location + i] != 0x00) {
                        Data[location + i] = 0x00;
                        IsModified = true;
                    }
                }
            }
        }

        public bool GetBit(int location, int bit) {
            if (location < 0)
                throw new ArgumentOutOfRangeException(nameof(location));
            if (bit < 1 || bit > 8)
                throw new ArgumentOutOfRangeException(nameof(bit));
            if (Data == null)
                throw new FileEditorNotLoadedException();
            if (location >= Data.Length)
                throw new FileEditorReadException();

            return (Data[location] >> bit - 1 & 0x01) == 1 ? true : false;
        }

        public void SetBit(int location, int bit, bool value) {
            if (location < 0)
                throw new ArgumentOutOfRangeException(nameof(location));
            if (bit < 1 || bit > 8)
                throw new ArgumentOutOfRangeException(nameof(bit));
            if (Data == null)
                throw new FileEditorNotLoadedException();
            if (location >= Data.Length)
                throw new FileEditorReadException();

            var bitmask = (byte)(1 << bit - 1);

            if (value) {
                if ((Data[location] & bitmask) == 0x00) {
                    Data[location] |= bitmask;
                    IsModified = true;
                }
            }
            else {
                if ((Data[location] & bitmask) != 0x00) {
                    Data[location] &= (byte) ~bitmask;
                    IsModified = true;
                }
            }
        }

        public void Dispose() {
        }

        public event EventHandler IsModifiedChanged;
    }
}
