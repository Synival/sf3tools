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
        public FileEditor()
        {
            _title = BaseTitle;
        }

        private byte[] _data = null;

        public bool IsLoaded => _data != null;

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
                    UpdateTitle();
                }
            }
        }

        /// <summary>
        /// File opened, with full path.
        /// </summary>
        public string Filename { get; private set; }

        /// <summary>
        /// Filename without the path.
        /// </summary>
        public string ShortFilename
        {
            get
            {
                if (Filename == null)
                {
                    return null;
                }

                var words = Filename.Split('\\');
                return words[Math.Max(0, words.Length - 1)];
            }
        }

        private string _title;

        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    TitleChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

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
                stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                var newData = new byte[stream.Length];
                stream.Read(newData, 0, (int)stream.Length);
                stream.Close();

                if (IsLoaded)
                {
                    CloseFile();
                }

                PreLoaded?.Invoke(this, EventArgs.Empty);

                Filename = filename;
                _data = newData;

                UpdateTitle();
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
            if (_data == null)
            {
                throw new FileEditorNotLoadedException();
            }

            FileStream stream = null;
            try
            {
                PreSaved?.Invoke(this, EventArgs.Empty);

                stream = new FileStream(filename, FileMode.Create);
                stream.Write(_data, 0, _data.Length);
                Filename = filename;

                IsModified = false;
                UpdateTitle();
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

            _data = null;
            Filename = null;
            IsModified = false;

            UpdateTitle();
            Closed?.Invoke(this, EventArgs.Empty);
            return true;
        }

        /// <summary>
        /// Gets the value of a byte at a location.
        /// </summary>
        /// <param name="location">The address of the byte.</param>
        public int GetByte(int location)
        {
            if (_data == null)
            {
                throw new FileEditorNotLoadedException();
            }

            try
            {
                return _data[location];
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
            if (_data == null)
            {
                throw new FileEditorNotLoadedException();
            }

            try
            {
                return _data[location] * 256 + _data[location + 1];
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
            if (_data == null)
            {
                throw new FileEditorNotLoadedException();
            }

            try
            {
                return (_data[location] * 256 * 256 * 256) + (_data[location + 1] * 256 * 256)
                            + (_data[location + 2] * 256) + _data[location + 3];
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
            if (_data == null)
            {
                throw new FileEditorNotLoadedException();
            }

            byte[] value = new byte[length];
            for (int i = 0; i < length; i++)
            {
                try
                {
                    if (_data[location + i] == 0x0)
                    {
                        break;
                    }
                    value[i] = _data[location + i];
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
            if (_data == null)
            {
                throw new FileEditorNotLoadedException();
            }

            if (_data[location] != value)
            {
                _data[location] = value;
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
            if (_data == null)
            {
                throw new FileEditorNotLoadedException();
            }

            byte highByte = (byte)(value >> 8);
            byte lowByte = (byte)(value % 0x100);

            if (_data[location + 0] != highByte || _data[location + 1] != lowByte)
            {
                _data[location + 0] = highByte;
                _data[location + 1] = lowByte;
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

            if (_data[location + 0] != converted[3] ||
                _data[location + 1] != converted[2] ||
                _data[location + 2] != converted[1] ||
                _data[location + 3] != converted[0])
            {
                _data[location + 0] = converted[3];
                _data[location + 1] = converted[2];
                _data[location + 2] = converted[1];
                _data[location + 3] = converted[0];
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
            if (_data == null)
            {
                throw new FileEditorNotLoadedException();
            }

            byte[] name = new byte[12];
            Encoding OutputText = Encoding.GetEncoding("shift-jis");
            name = OutputText.GetBytes(value);
            for (int i = 0; i < name.Length; i++)
            {
                if (_data[location + i] != name[i])
                {
                    _data[location + i] = name[i];
                    IsModified = true;
                }
            }
            if (name.Length < length)
            {
                for (int i = name.Length; i < length; i++)
                {
                    if (_data[location + i] != 0x00)
                    {
                        _data[location + i] = 0x00;
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
            if (_data == null)
            {
                throw new FileEditorNotLoadedException();
            }

            return ((_data[location] >> (bit - 1) & 0x01) == 1) ? true : false;
        }

        /// <summary>
        /// Sets the value of a single bit of a byte at a location.
        /// </summary>
        /// <param name="location">The address of the byte containing the bit.</param>
        /// <param name="bit">The position of the bit, in range (0, 7).</param>
        /// <param name="value">The new value of the bit.</param>
        public void SetBit(int location, int bit, bool value)
        {
            if (_data == null)
            {
                throw new FileEditorNotLoadedException();
            }

            byte bitmask = (byte)(1 << (bit - 1));

            if (value)
            {
                if ((_data[location] & bitmask) == 0x00)
                {
                    _data[location] |= bitmask;
                    IsModified = true;
                }
            }
            else
            {
                if ((_data[location] & bitmask) != 0x00)
                {
                    _data[location] &= (byte)~bitmask;
                    IsModified = true;
                }
            }
        }

        public string EditorTitle(string formTitle) => formTitle + (IsLoaded ? " - " + Title : "");

        public event EventHandler PreLoaded;
        public event EventHandler Loaded;
        public event EventHandler PreSaved;
        public event EventHandler Saved;
        public event EventHandler PreClosed;
        public event EventHandler Closed;
        public event EventHandler ModifiedChanged;
        public event EventHandler TitleChanged;

        /// <summary>
        /// Creates a new title to be used with UpdateTitle().
        /// This isn't intended to be used directly, but just overridden.
        /// Call UpdateTitle() whenever it looks like the title should be modified.
        /// </summary>
        protected virtual string BaseTitle => IsLoaded ? ShortFilename : "(no file)";

        /// <summary>
        /// Updates 'Title' and invokes a 'TitleChanged' event if it changed.
        /// Call this whenever it looks like the title should be modified.
        /// </summary>
        protected void UpdateTitle() => Title = BaseTitle + (IsModified ? "*" : "");
    }
}
