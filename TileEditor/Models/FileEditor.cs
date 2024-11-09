﻿using System;
using System.Text;
using System.IO;

namespace STHAEditor.Models
{
    static class FileEditor
    {
        private static byte[] data;
        public static int scenario = 0;
        public static string Filename;

        public static bool loadFile(string filename)
        {
            try {
                FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                data = new byte[stream.Length];
                stream.Read(data, 0, (int)stream.Length);
                Filename = filename;
                stream.Close();
            } catch (Exception) {
                return false;
            }
            return true;
        }

        public static bool saveFile(string filename)
        {
            FileStream stream = new FileStream(filename, FileMode.Create);
            stream.Write(data, 0, data.Length);
            Filename = filename;
            stream.Close();
            return true;
        }

        public static int getByte(int location)
        {
            return data[location];
        }

        public static int getWord(int location)
        {
            return data[location] * 256 + data[location + 1];
        }
        
        public static int getDouble(int location)
        {
            return (data[location] * 256 * 256 * 256) + (data[location + 1] * 256 * 256)
                        + (data[location + 2] * 256) + data[location + 3];
        }

        //Returns a string from values of location to location+length bytes
        public static string getString(int location, int length)
        {
            byte[] value = new byte[length];
            for (int i = 0; i < length; i++) {
                if (data[location + i] == 0x0) {
                    break;
                }
                value[i] = data[location + i];
            }
            Encoding InputText = Encoding.GetEncoding("shift-jis");
            return InputText.GetString(value);
        }

        public static void setByte(int location, byte value)
        {
            data[location] = value;
        }
        public static void setWord(int location, int value)
        {
            data[location] = (byte)(value >> 8);
            data[location + 1] = (byte)(value % 256);
        }
        public static void setDouble(int location, int value)
        {
            data[location] = (byte)(value >> 24);
            data[location + 1] = (byte)(value >> 16);
            data[location + 2] = (byte)(value >> 8);
            data[location + 3] = (byte)(value % 256);
        }

        public static void setString(int location, int length, string value)
        {
            byte[] name = new byte[12];
            Encoding OutputText = Encoding.GetEncoding("shift-jis");
            name = OutputText.GetBytes(value);
            for (int i = 0; i < name.Length; i++) {
                data[location + i] = name[i];
            }
            if (name.Length < length) {
                for (int i = name.Length; i < length; i++) {
                    data[location + i] = 0x0;
                }
            }
        }
        public static bool getBit(int location, int bit)
        {
            return ((data[location] >> (bit - 1) & 0x01) == 1) ? true : false;
        }
        public static void setBit(int location, int bit, bool value)
        {
            if (value) {
                data[location] |= (byte)(1 << (bit - 1));
            } else {
                data[location] &= (byte)~(1 << (bit - 1));
            }
        }
    }
}