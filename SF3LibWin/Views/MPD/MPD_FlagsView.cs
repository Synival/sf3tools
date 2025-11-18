using System.Windows.Forms;
using SF3.MPD;
using SF3.Win.Controls;

namespace SF3.Win.Views.MPD {
    public class MPD_FlagsView : ControlView<MPD_FlagEditor> {
        public MPD_FlagsView(string name, IMPD_Flags model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            var rval = base.Create();
            if (rval == null)
                return null;

            UpdateAllFlags();
            return rval;
        }

        public override void RefreshContent()
            => UpdateAllFlags();

        private void UpdateAllFlags() {
            if (Control == null)
                return;

            Control.UnknownMapFlag0x0001 = Model.UnknownMapFlag0x0001;
            Control.UnknownMapFlag0x0002 = Model.UnknownMapFlag0x0002;
            Control.HasSurfaceTextureRotation = Model.HasSurfaceTextureRotation;
            Control.AddDotProductBasedNoiseToStandardLightmap = Model.AddDotProductBasedNoiseToStandardLightmap;
            Control.UnknownFlatTileFlag0x0008 = Model.UnknownFlatTileFlag0x0008;
            Control.BackgroundImageType = Model.BackgroundImageType;
            Control.UnknownMapFlag0x0020 = Model.UnknownMapFlag0x0020;
            Control.HasChunk19Model = Model.HasChunk19Model;
            Control.ModifyPalette1ForGradient = Model.ModifyPalette1ForGradient;
            Control.HasModels = Model.HasModels;
            Control.HasSurfaceModel = Model.HasSurfaceModel;
            Control.GroundImageType = Model.GroundImageType;
            Control.HasCutsceneSkyBox = Model.HasCutsceneSkyBox;
            Control.HasBattleSkyBox = Model.HasBattleSkyBox;
            Control.NarrowAngleBasedLightmap = Model.NarrowAngleBasedLightmap;
            Control.HasExtraChunk1ModelWithChunk21Textures = Model.HasExtraChunk1ModelWithChunk21Textures;
            Control.Chunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists = Model.Chunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists;

            UpdateDerivedFlags();
        }

        private void UpdateDerivedFlags() {
            if (Control == null)
                return;

            Control.Chunk1IsLoadedFromLowMemory = Model.Chunk1IsLoadedFromLowMemory;
            Control.Chunk1IsLoadedFromHighMemory = Model.Chunk1IsLoadedFromHighMemory;
            Control.Chunk20IsSurfaceModelIfExists = Model.Chunk20IsSurfaceModelIfExists;
            Control.Chunk1IsModels = Model.Chunk1IsModels;
            Control.Chunk2IsSurfaceModel = Model.Chunk2IsSurfaceModel;
            Control.Chunk20IsSurfaceModel = Model.Chunk20IsSurfaceModel;
            Control.Chunk20IsModels = Model.Chunk20IsModels;
            Control.HighMemoryHasModels = Model.HighMemoryHasModels;
            Control.HighMemoryHasSurfaceModel = Model.HighMemoryHasSurfaceModel;
            Control.HasAnySkyBox = Model.HasAnySkyBox;
        }

        private IMPD_Flags _model;
        public IMPD_Flags Model {
            get => _model;
            set {
                if (_model != value) {
                    _model = value;
                    RefreshContent();
                }
            }
        }
    }
}
