using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Structs.DAT;
using SF3.Models.Tables;
using SF3.Types;

namespace SF3.Models.Files.DAT {
    public abstract class DAT_FileBase : GameTableFile, IDAT_File {
        protected DAT_FileBase(IByteData data, INameGetterContext nameGetterContext, ScenarioType? scenario, DAT_FileType fileType)
        : base(data, nameGetterContext, scenario) {
            FileType = fileType;
        }

        public DAT_FileType FileType { get; }
        public Table<DAT_FileTextureBase> TextureTable { get; protected set; }
        public int TextureViewerScale { get; set; } = 0;
        public TexturesAsSpritesheet Spritesheet { get; protected set; }
    }
}
