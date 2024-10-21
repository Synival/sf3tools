using System.Xml;
using System.IO;
using SF3.FileEditors;
using SF3.Extensions;

namespace SF3.Models.X002.Warps
{
    public class WarpList : ModelArray<Warp>
    {
        public int MaxSize { get; } = 1000;

        public WarpList(IX002_FileEditor fileEditor) : base(fileEditor)
        {
            _fileEditor = fileEditor;
        }

        private IX002_FileEditor _fileEditor;
        private Warp[] models;

        public override string ResourceFile => "Resources/X002Warp.xml";

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load()
        {
            _models = new Warp[0];
            models = new Warp[MaxSize];
            FileStream stream = null;
            try
            {
                stream = new FileStream(ResourceFile, FileMode.Open);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreWhitespace = true;
                XmlReader xml = XmlTextReader.Create(stream, settings);
                xml.Read();
                Warp[] old;
                //int stop = 0;
                //int numberTest = 0;
                //while (!xml.EOF)
                int myCount = 0;
                string myName = "WarpIndex " + myCount;
                //Globals.treasureDebug = true;
                //while (!xml.EOF && (_models.Length == 0 || _models[_models.Length - 1].Searched != 0xffff))

                while (!xml.EOF && (_models.Length == 0 || (_models[_models.Length - 1].WarpType != 0x01 && _models[_models.Length - 1].WarpType != 0xff)))
                //while (!xml.EOF && (_models.Length == 0 || (_models[_models.Length - 1].Searched != 0xffff || _models[_models.Length - 1].EventNumber != 0xffff)))
                //while (!xml.EOF && (_models.Length == 0 || myCount <= 2))
                {
                    {
                        xml.Read();
                        if (xml.HasAttributes)
                        {
                            _models = _models.ExpandedWith(new Warp(_fileEditor, myCount, myName));

                            myCount++;
                            myName = "WarpIndex ";
                            myName = myName + myCount;

                            models[_models[_models.Length - 1].WarpID] = _models[_models.Length - 1];
                        }
                    }
                }

            }
            catch (FileLoadException)
            {
                return false;
            }
            catch (FileNotFoundException)
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
    }
}
