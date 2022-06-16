using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SystemPlus.Extensions;

namespace SystemPlus.ClassSupport
{
    public abstract class SaveSupport
    {
        protected abstract void save(string path);
        protected abstract void load(ref object loadedObject, string path);

        protected abstract string getSuportedExtensions();

        public bool Save(string location, string fileName)
        {
            if (!Directory.Exists(location))
                return false;
            //FileSave fs = new FileSave();
            save(location + fileName);//ref fs, Path.GetExtension(location + fileName));
            //File.WriteAllBytes(location + fileName, fs.Bytes);
            return true;
        }

        public object Load(string location)
        {
            if (!File.Exists(location))
                return null;
            else
            {
                object ob = new object();
                load(ref ob, location);
                return ob;
            }
        }

        public string GetSuportedExtensions() => getSuportedExtensions();
    }

    public class FileSave
    {
        private List<byte> bytes;

        public byte[] Bytes { get { return (byte[])bytes.ToArray(); } }

        public FileSave()
        {
            bytes = new List<byte>();
        }

        public void WriteByte(byte b)
        {
            bytes.Add(b);
        }

        public void WriteShort(short s)
        {
            bytes.AddAll(BitConverter.GetBytes(s).ToList());
        }

        public void WriteInt(int i)
        {
            bytes.AddAll(BitConverter.GetBytes(i).ToList());
        }

        public void WriteLong(long l)
        {
            bytes.AddAll(BitConverter.GetBytes(l).ToList());
        }

        public void WriteDouble(double d)
        {
            bytes.AddAll(BitConverter.GetBytes(d).ToList());
        }

        public void WriteChar(char ch)
        {
            bytes.AddAll(BitConverter.GetBytes(ch).ToList());
        }

        public void WriteString(string s)
        {
            List<byte> _bytes = new List<byte>();
            for (int i = 0; i < s.Length; i++)
                _bytes.AddAll(BitConverter.GetBytes(s[i]).ToList());
            bytes.AddAll(_bytes);
        }

        public void WriteBool(bool b)
        {
            bytes.AddAll(BitConverter.GetBytes(b).ToList());
        }

        public void WriteFS(FileSave fs)
        {
            bytes.AddAll(fs.bytes);
        }
    }
}
