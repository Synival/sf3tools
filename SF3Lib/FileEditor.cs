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
        private static byte[] data = null;
        public static string Filename;

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
                data = new byte[stream.Length];
                stream.Read(data, 0, (int)stream.Length);
                Filename = filename;
                stream.Close();
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
                stream = new FileStream(filename, FileMode.Create);
                stream.Write(data, 0, data.Length);
                Filename = filename;
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

        public static int GetByte(int location)
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

        public static int GetWord(int location)
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

        public static int GetDouble(int location)
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

        //Returns a string from values of location to location+length bytes
        public static string GetString(int location, int length)
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

        public static void SetByte(int location, byte value)
        {
            if (data == null)
            {
                throw new FileEditorNotLoadedException();
            }

            data[location] = value;
        }
        public static void SetWord(int location, int value)
        {
            if (data == null)
            {
                throw new FileEditorNotLoadedException();
            }

            data[location] = (byte)(value >> 8);
            data[location + 1] = (byte)(value % 256);
        }
        public static void SetDouble(int location, int value)
        {
            byte[] converted = BitConverter.GetBytes(value);
            data[location] = converted[3];
            data[location + 1] = converted[2];
            data[location + 2] = converted[1];
            data[location + 3] = converted[0];
        }

        public static void SetString(int location, int length, string value)
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
                data[location + i] = name[i];
            }
            if (name.Length < length)
            {
                for (int i = name.Length; i < length; i++)
                {
                    data[location + i] = 0x0;
                }
            }
        }
        public static bool GetBit(int location, int bit)
        {
            if (data == null)
            {
                throw new FileEditorNotLoadedException();
            }

            return ((data[location] >> (bit - 1) & 0x01) == 1) ? true : false;
        }
        public static void SetBit(int location, int bit, bool value)
        {
            if (data == null)
            {
                throw new FileEditorNotLoadedException();
            }

            if (value)
            {
                data[location] |= (byte)(1 << (bit - 1));
            }
            else
            {
                data[location] &= (byte)~(1 << (bit - 1));
            }
        }
    }
}
