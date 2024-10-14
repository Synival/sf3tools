using System;
using System.Text;
using System.IO;
using SF3.Exceptions;

namespace SF3
{
    /// <summary>
    /// Used for loading, saving, reading, and modifying .BIN files.
    /// </summary>
    public class FileEditor : IFileEditor
    {
        private byte[] data = null;

        public bool IsLoaded => data != null;

        private bool _isModified = false;

        public bool IsModified
        {
            get => _isModified;
            private set
            {
                if (_isModified != value)
                {
                    _isModified = value;
                    ModifiedChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public string Filename { get; private set; }

        public virtual string Title => IsLoaded ? Filename : "(no file)";

        /// <summary>
        /// Loads a file's binary data for editing.
        /// </summary>
        /// <param name="filename">The file to load.</param>
        /// <returns>'true' on success, 'false' on failure.</returns>
        public bool LoadFile(string filename)
        {
            FileStream stream = null;
            try
            {
                PreLoaded?.Invoke(this, EventArgs.Empty);

                stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                data = new byte[stream.Length];
                stream.Read(data, 0, (int)stream.Length);
                Filename = filename;
                stream.Close();

                Loaded?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

            return true;
        }

        /// <summary>
        /// Saves a file's binary data for editing.
        /// </summary>
        /// <param name="filename">The file to load.</param>
        /// <returns>'true' on success, 'false' on failure.</returns>
        public bool SaveFile(string filename)
        {
            if (data == null)
            {
                throw new FileEditorNotLoadedException();
            }

            FileStream stream = null;
            try
            {
                PreSaved?.Invoke(this, EventArgs.Empty);

                stream = new FileStream(filename, FileMode.Create);
                stream.Write(data, 0, data.Length);
                Filename = filename;

                IsModified = false;
                Saved?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

            return true;
        }

        /// <summary>
        /// Closes a file if opened. Invokes event 'Closed' if a file was closed.
        /// </summary>
        /// <returns>'true' on success, even if no file is open.</returns>
        public bool CloseFile()
        {
            if (!IsLoaded)
            {
                return true;
            }

            PreClosed?.Invoke(this, EventArgs.Empty);

            data = null;
            Filename = null;
            IsModified = false;

            Closed?.Invoke(this, EventArgs.Empty);
            return true;
        }

        /// <summary>
        /// Gets the value of a byte at a location.
        /// </summary>
        /// <param name="location">The address of the byte.</param>
        public int GetByte(int location)
        {
            if (data == null)
            {
                throw new FileEditorNotLoadedException();
            }

            try
            {
                return data[location];
            }
            catch (IndexOutOfRangeException)
            {
                //wrong kind of file was selected to load
                throw new FileEditorReadException();
            }
        }

        /// <summary>
        /// Gets the value of a 16-bit integer at a location.
        /// </summary>
        /// <param name="location">The address of the 16-bit integer.</param>
        public int GetWord(int location)
        {
            if (data == null)
            {
                throw new FileEditorNotLoadedException();
            }

            try
            {
                return data[location] * 256 + data[location + 1];
            }
            catch (IndexOutOfRangeException)
            {
                //wrong kind of file was selected to load
                throw new FileEditorReadException();
            }
        }

        /// <summary>
        /// Gets the value of a 32-bit integer at a location.
        /// </summary>
        /// <param name="location">The address of the 32-bit integer.</param>
        public int GetDouble(int location)
        {
            if (data == null)
            {
                throw new FileEditorNotLoadedException();
            }

            try
            {
                return (data[location] * 256 * 256 * 256) + (data[location + 1] * 256 * 256)
                            + (data[location + 2] * 256) + data[location + 3];
            }
            catch (IndexOutOfRangeException)
            {
                //wrong kind of file was selected to load
                throw new FileEditorReadException();
            }
        }

        /// <summary>
        /// Returns the value of string data of a specific size at a location.
        /// </summary>
        /// <param name="location">The address of the string.</param>
        /// <param name="length">The length of the string space.</param>
        public string GetString(int location, int length)
        {
            if (data == null)
            {
                throw new FileEditorNotLoadedException();
            }

            byte[] value = new byte[length];
            for (int i = 0; i < length; i++)
            {
                try
                {
                    if (data[location + i] == 0x0)
                    {
                        break;
                    }
                    value[i] = data[location + i];
                }
                catch (IndexOutOfRangeException)
                {
                    //wrong kind of file was selected to load
                    throw new FileEditorReadException();
                }
            }
            Encoding InputText = Encoding.GetEncoding("shift-jis");
            return InputText.GetString(value);
        }

        /// <summary>
        /// Sets the value of a byte at a location.
        /// </summary>
        /// <param name="location">The address of the byte.</param>
        /// <param name="value">The new value of the byte.</param>
        public void SetByte(int location, byte value)
        {
            if (data == null)
            {
                throw new FileEditorNotLoadedException();
            }

            if (data[location] != value)
            {
                data[location] = value;
                IsModified = true;
            }
        }

        /// <summary>
        /// Sets the value of a 16-bit integer at a location.
        /// </summary>
        /// <param name="location">The address of the 16-bit integer.</param>
        /// <param name="value">The new value of the 16-bit integer.</param>
        public void SetWord(int location, int value)
        {
            if (data == null)
            {
                throw new FileEditorNotLoadedException();
            }

            byte highByte = (byte)(value >> 8);
            byte lowByte = (byte)(value % 0x100);

            if (data[location + 0] != highByte || data[location + 1] != lowByte)
            {
                data[location + 0] = highByte;
                data[location + 1] = lowByte;
                IsModified = true;
            }
        }

        /// <summary>
        /// Sets the value of a 32-bit integer at a location.
        /// </summary>
        /// <param name="location">The address of the 32-bit integer.</param>
        /// <param name="value">The new value of the 32-bit integer.</param>
        public void SetDouble(int location, int value)
        {
            byte[] converted = BitConverter.GetBytes(value);

            if (data[location + 0] != converted[3] ||
                data[location + 1] != converted[2] ||
                data[location + 2] != converted[1] ||
                data[location + 3] != converted[0])
            {
                data[location + 0] = converted[3];
                data[location + 1] = converted[2];
                data[location + 2] = converted[1];
                data[location + 3] = converted[0];
                IsModified = true;
            }
        }

        /// <summary>
        /// Sets the value of string data of a specific size at a location.
        /// Data set will not exceed the length provided, and remaining bytes are automatically filled with zeros.
        /// </summary>
        /// <param name="location">The address of the string.</param>
        /// <param name="length">The length of the string space.</param>
        /// <param name="value">The new value of the string.</param>
        public void SetString(int location, int length, string value)
        {
            if (data == null)
            {
                throw new FileEditorNotLoadedException();
            }

            byte[] name = new byte[12];
            Encoding OutputText = Encoding.GetEncoding("shift-jis");
            name = OutputText.GetBytes(value);
            for (int i = 0; i < name.Length; i++)
            {
                if (data[location + i] != name[i])
                {
                    data[location + i] = name[i];
                    IsModified = true;
                }
            }
            if (name.Length < length)
            {
                for (int i = name.Length; i < length; i++)
                {
                    if (data[location + i] != 0x00)
                    {
                        data[location + i] = 0x00;
                        IsModified = true;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the value of a single bit of a byte at a location.
        /// </summary>
        /// <param name="location">The address of the byte containing the bit.</param>
        /// <param name="bit">The position of the bit, in range (0, 7).</param>
        /// <returns>True if the bit is set, false if the bit is unset.</returns>
        public bool GetBit(int location, int bit)
        {
            if (data == null)
            {
                throw new FileEditorNotLoadedException();
            }

            return ((data[location] >> (bit - 1) & 0x01) == 1) ? true : false;
        }

        /// <summary>
        /// Sets the value of a single bit of a byte at a location.
        /// </summary>
        /// <param name="location">The address of the byte containing the bit.</param>
        /// <param name="bit">The position of the bit, in range (0, 7).</param>
        /// <param name="value">The new value of the bit.</param>
        public void SetBit(int location, int bit, bool value)
        {
            if (data == null)
            {
                throw new FileEditorNotLoadedException();
            }

            byte bitmask = (byte)(1 << (bit - 1));

            if (value)
            {
                if ((data[location] & bitmask) == 0x00)
                {
                    data[location] |= bitmask;
                    IsModified = true;
                }
            }
            else
            {
                if ((data[location] & bitmask) != 0x00)
                {
                    data[location] &= (byte)~bitmask;
                    IsModified = true;
                }
            }
        }

        public event EventHandler PreLoaded;
        public event EventHandler Loaded;
        public event EventHandler PreSaved;
        public event EventHandler Saved;
        public event EventHandler PreClosed;
        public event EventHandler Closed;
        public event EventHandler ModifiedChanged;
    }
}
