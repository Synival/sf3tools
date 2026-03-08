using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Imaging;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.DAT;
using SF3.NamedValues;
using SF3.Types;

namespace SF3.Models.Files.DAT {
    public class ITEM_CG_File : DAT_FileBase {
        // TODO: Where is this loaded?
        public override int RamAddress => 0x00000000;
        // TODO: To where is this loaded?
        public override int RamAddressLimit => 0x00000000;

        protected ITEM_CG_File(IByteData data, INameGetterContext nameGetterContext, ScenarioType? scenario)
        : base(data, nameGetterContext, scenario, DAT_FileType.ITEM_CG) {
            SpellIconIndex = ValueNames.ItemInfo.Info[scenario.Value].Values.Max(x => x.Key) + 1;
        }

        public static ITEM_CG_File Create(IByteData data, INameGetterContext nameGetterContext, ScenarioType? scenario) {
            var newFile = new ITEM_CG_File(data, nameGetterContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize " + newFile.GetType().Name);
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            var tables = new List<ITable> {
                (TextureTable = ItemCG_TextureTable.Create(Data, nameof(TextureTable), 0, NameGetterContext))
            };
            TextureViewerScale = 4;
            Spritesheet = new TexturesAsSpritesheet(this, ItemCG_TextureTable.ItemSpellPalette, true, 24, 24, 32);

            return tables;
        }

        public override void ReplaceImages8Bit(byte[][,] images, Palette palette) {
            // TODO: force images to existing palette
            // TODO: pass along to ReplaceImages(byte[][,])
            throw new ArgumentException("Coming soon!");
        }

        public override void ReplaceImages16Bit(ushort[][,] images) {
            // TODO: force images to existing 8-bit indexed color palette
            // TODO: pass along to ReplaceImages(byte[][,])
            throw new ArgumentException("Coming soon!");
        }

        private void ReplaceImages(byte[][,] images) {
            // TODO: force images to existing palette
            // TODO: rebuild contents of entire file
            // TODO: update addresses of all images
            throw new ArgumentException("Coming soon!");
        }

        public override bool CanReplaceImages8Bit => true;
        public override bool CanReplaceImages16Bit => true;

        public int SpellIconIndex { get; }
    }
}
