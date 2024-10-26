using System;
using System.IO;
using SF3.Extensions;
using SF3.FileEditors;
using static SF3.Utils.Resources;

namespace SF3.Models.X013.WeaponSpellRank {
    public class WeaponSpellRankList : ModelArray<WeaponSpellRank> {
        public int MaxSize { get; } = 4;

        public WeaponSpellRankList(IX013_FileEditor fileEditor) : base(fileEditor) {
            _fileEditor = fileEditor;
        }

        private IX013_FileEditor _fileEditor;

        public override string ResourceFile => "Resources/WeaponSpellRankList.xml";

        /// <summary>
        /// Loads data from the file editor provided in the constructor.
        /// </summary>
        /// <returns>'true' if ResourceFile was loaded successfully, otherwise 'false'.</returns>
        public override bool Load() {
            _models = new WeaponSpellRank[0];
            FileStream stream = null;
            try {
                stream = new FileStream(ResourceFile, FileMode.Open, FileAccess.Read);

                var xml = MakeXmlReader(stream);
                xml.Read();
                while (!xml.EOF) {
                    xml.Read();
                    if (xml.HasAttributes) {
                        var newModel = new WeaponSpellRank(_fileEditor, Convert.ToInt32(xml.GetAttribute(0), 16), xml.GetAttribute(1));
                        _models = _models.ExpandedWith(newModel);
                        if (newModel.WeaponSpellRankID < 0 || newModel.WeaponSpellRankID >= MaxSize) {
                            throw new IndexOutOfRangeException();
                        }
                    }
                }
            }
            catch (FileLoadException) {
                return false;
            }
            catch (FileNotFoundException) {
                return false;
            }
            finally {
                if (stream != null) {
                    stream.Close();
                }
            }
            return true;
        }
    }
}
