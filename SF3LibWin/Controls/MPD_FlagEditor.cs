using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Win.Controls;
using SF3.Types;

namespace SF3.Win.Controls {
    public partial class MPD_FlagEditor : DarkModeUserControl {
        private class ComboBoxValue<T>
        {
            public ComboBoxValue(T value, string name) {
                Value = value;
                Name = name;
            }

            public override string ToString()
                => Name;

            public T Value { get; }
            public string Name { get; }
        }

        private class BackgroundImageTypeComboBoxValue : ComboBoxValue<BackgroundImageType> {
            public BackgroundImageTypeComboBoxValue(BackgroundImageType value, string name) : base(value, name) {}
        }

        private class GroundImageTypeComboBoxValue : ComboBoxValue<GroundImageType> {
            public GroundImageTypeComboBoxValue(GroundImageType value, string name) : base(value, name) {}
        }

        private static Dictionary<BackgroundImageType, BackgroundImageTypeComboBoxValue> c_backgroundImageComboBoxValues = new Dictionary<BackgroundImageType, BackgroundImageTypeComboBoxValue>() {
            { BackgroundImageType.None, new BackgroundImageTypeComboBoxValue(BackgroundImageType.None,  "None") },
            { BackgroundImageType.Still, new BackgroundImageTypeComboBoxValue(BackgroundImageType.Still, "Still") },
            { BackgroundImageType.Tiled, new BackgroundImageTypeComboBoxValue(BackgroundImageType.Tiled, "Tiled") },
            { BackgroundImageType.StillAndTiled, new BackgroundImageTypeComboBoxValue(BackgroundImageType.StillAndTiled, "Still + Tiled") },
        };

        private static Dictionary<GroundImageType, GroundImageTypeComboBoxValue> c_groundImageComboBoxValues = new Dictionary<GroundImageType, GroundImageTypeComboBoxValue>() {
            { GroundImageType.None, new GroundImageTypeComboBoxValue(GroundImageType.None, "None") },
            { GroundImageType.Repeated, new GroundImageTypeComboBoxValue(GroundImageType.Repeated, "Repeated") },
            { GroundImageType.Tiled, new GroundImageTypeComboBoxValue(GroundImageType.Tiled, "Tiled") },
            { GroundImageType.Invalid, new GroundImageTypeComboBoxValue(GroundImageType.Invalid, "0x1400 (Invalid)") },
        };

        public MPD_FlagEditor() {
            InitializeComponent();
            cbBackgroundImageType.Items.AddRange(c_backgroundImageComboBoxValues.Values.ToArray());
            cbGroundImageType.Items.AddRange(c_groundImageComboBoxValues.Values.ToArray());
        }

        public bool UnknownMapFlag0x0001 {
            get => cbUnknownMapFlag0x0001.Checked;
            set => cbUnknownMapFlag0x0001.Checked = value;
        }

        public bool UnknownMapFlag0x0002 {
            get => cbUnknownMapFlag0x0002.Checked;
            set => cbUnknownMapFlag0x0002.Checked = value;
        }

        public bool HasSurfaceTextureRotation {
            get => cbHasSurfaceTextureRotation.Checked;
            set => cbHasSurfaceTextureRotation.Checked = value;
        }

        public bool AddDotProductBasedNoiseToStandardLightmap {
            get => cbAddDotProductBasedNoiseToStandardLightmap.Checked;
            set => cbAddDotProductBasedNoiseToStandardLightmap.Checked = value;
        }

        public bool UnknownFlatTileFlag0x0008 {
            get => cbUnknownFlatTileFlag0x0008.Checked;
            set => cbUnknownFlatTileFlag0x0008.Checked = value;
        }

        public BackgroundImageType BackgroundImageType {
            get => (cbBackgroundImageType.SelectedItem as BackgroundImageTypeComboBoxValue)?.Value ?? (BackgroundImageType) -1;
            set => cbBackgroundImageType.SelectedItem = c_backgroundImageComboBoxValues.TryGetValue(value, out var valueOut) ? valueOut : null;
        }

        public bool UnknownMapFlag0x0020 {
            get => cbUnknownMapFlag0x0020.Checked;
            set => cbUnknownMapFlag0x0020.Checked = value;
        }

        public bool HasChunk19Model {
            get => cbHasChunk19Model.Checked;
            set => cbHasChunk19Model.Checked = value;
        }

        public bool ModifyPalette1ForGradient {
            get => cbModifyPalette1ForGradient.Checked;
            set => cbModifyPalette1ForGradient.Checked = value;
        }

        public bool HasModels {
            get => cbHasModels.Checked;
            set => cbHasModels.Checked = value;
        }

        public bool HasSurfaceModel {
            get => cbHasSurfaceModel.Checked;
            set => cbHasSurfaceModel.Checked = value;
        }

        public GroundImageType GroundImageType {
            get => (cbGroundImageType.SelectedItem as GroundImageTypeComboBoxValue)?.Value ?? (GroundImageType) -1;
            set => cbGroundImageType.SelectedItem = c_groundImageComboBoxValues.TryGetValue(value, out var valueOut) ? valueOut : null;
        }

        public bool HasCutsceneSkyBox {
            get => cbHasCutsceneSkyBox.Checked;
            set => cbHasCutsceneSkyBox.Checked = value;
        }

        public bool HasBattleSkyBox {
            get => cbHasBattleSkyBox.Checked;
            set => cbHasBattleSkyBox.Checked = value;
        }

        public bool NarrowAngleBasedLightmap {
            get => cbNarrowAngleBasedLightmap.Checked;
            set => cbNarrowAngleBasedLightmap.Checked = value;
        }

        public bool HasExtraChunk1ModelWithChunk21Textures {
            get => cbHasExtraChunk1ModelWithChunk21Textures.Checked;
            set => cbHasExtraChunk1ModelWithChunk21Textures.Checked = value;
        }

        public bool Chunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists {
            get => cbChunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists.Checked;
            set => cbChunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists.Checked = value;
        }

        public bool Chunk1IsLoadedFromLowMemory {
            get => cbChunk1IsLoadedFromLowMemory.Checked;
            set => cbChunk1IsLoadedFromLowMemory.Checked = value;
        }

        public bool Chunk1IsLoadedFromHighMemory {
            get => cbChunk1IsLoadedFromHighMemory.Checked;
            set => cbChunk1IsLoadedFromHighMemory.Checked = value;
        }

        public bool Chunk20IsSurfaceModelIfExists {
            get => cbChunk20IsSurfaceModelIfExists.Checked;
            set => cbChunk20IsSurfaceModelIfExists.Checked = value;
        }

        public bool Chunk1IsModels {
            get => cbChunk1IsModels.Checked;
            set => cbChunk1IsModels.Checked = value;
        }

        public bool Chunk2IsSurfaceModel {
            get => cbChunk2IsSurfaceModel.Checked;
            set => cbChunk2IsSurfaceModel.Checked = value;
        }

        public bool Chunk20IsSurfaceModel {
            get => cbChunk20IsSurfaceModel.Checked;
            set => cbChunk20IsSurfaceModel.Checked = value;
        }

        public bool Chunk20IsModels {
            get => cbChunk20IsModels.Checked;
            set => cbChunk20IsModels.Checked = value;
        }

        public bool HighMemoryHasModels {
            get => cbHighMemoryHasModels.Checked;
            set => cbHighMemoryHasModels.Checked = value;
        }

        public bool HighMemoryHasSurfaceModel {
            get => cbHighMemoryHasSurfaceModel.Checked;
            set => cbHighMemoryHasSurfaceModel.Checked = value;
        }

        public bool HasAnySkyBox {
            get => cbHasAnySkyBox.Checked;
            set => cbHasAnySkyBox.Checked = value;
        }
    }
}
