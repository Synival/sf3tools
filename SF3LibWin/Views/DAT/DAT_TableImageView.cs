using System;
using System.Drawing;
using CommonLib.NamedValues;
using SF3.Images;
using SF3.Models.Structs.Shared;
using SF3.Models.Tables;

namespace SF3.Win.Views.DAT {
    public class DAT_TableImageView : TableTextureView<FixedSizeTextureStructBase, Table<FixedSizeTextureStructBase>> {
        public DAT_TableImageView(string name, Table<FixedSizeTextureStructBase> table, INameGetterContext nameGetterContext, float? imageScale = null)
        : base(name, table, nameGetterContext, imageScale) {}

        protected override ITextureData GetTextureFromModel(FixedSizeTextureStructBase frame)
            => frame;
    }
}