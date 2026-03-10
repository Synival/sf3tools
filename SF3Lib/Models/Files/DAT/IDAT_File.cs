using CommonLib.Imaging;
using SF3.Models.Structs.DAT;
using SF3.Models.Tables;
using SF3.Types;

namespace SF3.Models.Files.DAT {
    public interface IDAT_File : IGameTableFile {
        void ReplaceImages8Bit(byte[][,] images, Palette palette, bool minimalChanges);
        void ReplaceImages16Bit(ushort[][,] images, bool minimalChanges);

        DAT_FileType FileType { get; }
        Table<DAT_FileTextureBase> TextureTable { get; }
        int TextureViewerScale { get; }
        TexturesAsSpritesheet Spritesheet { get; }

        bool CanReplaceImages8Bit { get; }
        bool CanReplaceImages16Bit { get; }
    }
}
