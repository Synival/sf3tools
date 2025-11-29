using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CommonLib.Win.Controls;
using SF3.MPD;
using SF3.Types;

namespace SF3.Win.Controls {
    public partial class MPD_FlagEditor : DarkModeUserControl {
        private class ComboBoxValue<T> {
            public ComboBoxValue(T value, string name) {
                Value = value;
                Name = name;
            }

            public override string ToString()
                => Name;

            public T Value { get; }
            public string Name { get; }
        }

        public MPD_FlagEditor() {
            SuspendLayout();
            InitializeComponent();

            cbUnknownMapFlag0x0001.CheckedChanged += (s, e)
                => TrySetProperty(cbUnknownMapFlag0x0001,
                    nameof(IMPD_AllFlags.Bit_0x0001_Unknown),
                    nameof(IMPD_AllFlags.CanSet_0x0001_Unknown));
            cbUnknownMapFlag0x0002.CheckedChanged += (s, e)
                => TrySetProperty(cbUnknownMapFlag0x0002,
                    nameof(IMPD_AllFlags.Bit_0x0002_Unknown),
                    nameof(IMPD_AllFlags.CanSet_0x0002_Unknown));
            cbHasSurfaceTextureRotation.CheckedChanged += (s, e)
                => TrySetProperty(cbHasSurfaceTextureRotation,
                    nameof(IMPD_AllFlags.Bit_0x0002_HasSurfaceTextureRotation),
                    nameof(IMPD_AllFlags.CanSet_0x0002_HasSurfaceTextureRotation));
            cbAddDotProductBasedNoiseToStandardLightmap.CheckedChanged += (s, e)
                => TrySetProperty(cbAddDotProductBasedNoiseToStandardLightmap,
                    nameof(IMPD_AllFlags.Bit_0x0004_AddDotProductBasedNoiseToStandardLightmap),
                    nameof(IMPD_AllFlags.CanSet_0x0004_AddDotProductBasedNoiseToStandardLightmap));
            cbKeepTexturelessFlatTiles.CheckedChanged += (s, e)
                => TrySetProperty(cbKeepTexturelessFlatTiles,
                    nameof(IMPD_AllFlags.Bit_0x0008_KeepTexturelessFlatTiles),
                    nameof(IMPD_AllFlags.CanSet_0x0008_KeepTexturelessFlatTiles));
            cbUnknownMapFlag0x0020.CheckedChanged += (s, e)
                => TrySetProperty(cbUnknownMapFlag0x0020,
                    nameof(IMPD_AllFlags.Bit_0x0020_Unknown),
                    nameof(IMPD_AllFlags.CanSet_0x0020_Unknown));
            cbHasChunk19Model.CheckedChanged += (s, e)
                => TrySetProperty(cbHasChunk19Model,
                    nameof(IMPD_AllFlags.Bit_0x0080_HasChunk19ModelWithChunk10Textures),
                    nameof(IMPD_AllFlags.CanSet_0x0080_HasChunk19ModelWithChunk10Textures));
            cbModifyPalette1ForGradient.CheckedChanged += (s, e)
                => TrySetProperty(cbModifyPalette1ForGradient,
                    nameof(IMPD_AllFlags.Bit_0x0080_SetMSBForPalette1),
                    nameof(IMPD_AllFlags.CanSet_0x0080_SetMSBForPalette1));
            cbHasModels.CheckedChanged += (s, e)
                => TrySetProperty(cbHasModels,
                    nameof(IMPD_AllFlags.Bit_0x0100_HasModels),
                    nameof(IMPD_AllFlags.CanSet_0x0100_HasModels));
            cbHasSurfaceModel.CheckedChanged += (s, e)
                => TrySetProperty(cbHasSurfaceModel,
                    nameof(IMPD_AllFlags.Bit_0x0200_HasSurfaceModel),
                    nameof(IMPD_AllFlags.CanSet_0x0200_HasSurfaceModel));
            cbHasCutsceneSkyBox.CheckedChanged += (s, e)
                => TrySetProperty(cbHasCutsceneSkyBox,
                    nameof(IMPD_AllFlags.Bit_0x0800_HasCutsceneSkyBox),
                    nameof(IMPD_AllFlags.CanSet_0x0800_HasCutsceneSkyBox));
            cbHasBattleSkyBox.CheckedChanged += (s, e)
                => TrySetProperty(cbHasBattleSkyBox,
                    nameof(IMPD_AllFlags.Bit_0x2000_HasBattleSkyBox),
                    nameof(IMPD_AllFlags.CanSet_0x2000_HasBattleSkyBox));
            cbNarrowAngleBasedLightmap.CheckedChanged += (s, e)
                => TrySetProperty(cbNarrowAngleBasedLightmap,
                    nameof(IMPD_AllFlags.Bit_0x2000_NarrowAngleBasedLightmap),
                    nameof(IMPD_AllFlags.CanSet_0x2000_NarrowAngleBasedLightmap));
            cbHasExtraChunk1ModelWithChunk21Textures.CheckedChanged += (s, e)
                => TrySetProperty(cbHasExtraChunk1ModelWithChunk21Textures,
                    nameof(IMPD_AllFlags.Bit_0x4000_HasExtraChunk1ModelWithChunk21Textures),
                    nameof(IMPD_AllFlags.CanSet_0x4000_HasExtraChunk1ModelWithChunk21Textures));
            cbChunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists.CheckedChanged += (s, e)
                => TrySetProperty(cbChunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists,
                    nameof(IMPD_AllFlags.Bit_0x8000_Chunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists),
                    nameof(IMPD_AllFlags.CanSet_0x8000_Chunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists));
            cbChunk20IsSurfaceModelIfExists.CheckedChanged += (s, e)
                => TrySetProperty(cbChunk20IsSurfaceModelIfExists,
                    nameof(IMPD_AllFlags.Bit_0x8000_Chunk20IsSurfaceModelIfExists),
                    nameof(IMPD_AllFlags.CanSet_0x8000_Chunk20IsSurfaceModelIfExists));

            // Derived values
            cbChunk1IsLoadedFromLowMemory.CheckedChanged += (s, e)
                => TrySetProperty(cbChunk1IsLoadedFromLowMemory,
                    nameof(IMPD_AllFlags.Chunk1IsLoadedFromLowMemory),
                    null);
            cbChunk1IsLoadedFromHighMemory.CheckedChanged += (s, e)
                => TrySetProperty(cbChunk1IsLoadedFromHighMemory,
                    nameof(IMPD_AllFlags.Chunk1IsLoadedFromHighMemory),
                    null);
            cbChunk1IsModels.CheckedChanged += (s, e)
                => TrySetProperty(cbChunk1IsModels,
                    nameof(IMPD_AllFlags.Chunk1IsModels),
                    null);
            cbChunk2IsSurfaceModel.CheckedChanged += (s, e)
                => TrySetProperty(cbChunk2IsSurfaceModel,
                    nameof(IMPD_AllFlags.Chunk2IsSurfaceModel),
                    null);
            cbChunk20IsSurfaceModel.CheckedChanged += (s, e)
                => TrySetProperty(cbChunk20IsSurfaceModel,
                    nameof(IMPD_AllFlags.Chunk20IsSurfaceModel),
                    null);
            cbChunk20IsModels.CheckedChanged += (s, e)
                => TrySetProperty(cbChunk20IsModels,
                    nameof(IMPD_AllFlags.Chunk20IsModels),
                    null);
            cbHighMemoryHasModels.CheckedChanged += (s, e)
                => TrySetProperty(cbHighMemoryHasModels,
                    nameof(IMPD_AllFlags.HighMemoryHasModels),
                    null);
            cbHighMemoryHasSurfaceModel.CheckedChanged += (s, e)
                => TrySetProperty(cbHighMemoryHasSurfaceModel,
                    nameof(IMPD_AllFlags.HighMemoryHasSurfaceModel),
                    null);
            cbHasAnySkyBox.CheckedChanged += (s, e)
                => TrySetProperty(cbHasAnySkyBox,
                    nameof(IMPD_AllFlags.HasAnySkyBox),
                    null);

            ResumeLayout();
        }

        private IMPD_AllFlags _flagsSource = null;
        public IMPD_AllFlags FlagsSource {
            get => _flagsSource;
            set {
                if (_flagsSource != value) {
                    _flagsSource = value;
                    UpdateFlagsFromSource();
                }
            }
        }

        private bool _inUpdateFlagsFromSource = false;
        public void UpdateFlagsFromSource() {
            if (_inUpdateFlagsFromSource)
                return;

            _inUpdateFlagsFromSource = true;

            SetControlState(cbUnknownMapFlag0x0001,
                nameof(IMPD_AllFlags.Bit_0x0001_Unknown),
                nameof(IMPD_AllFlags.CanSet_0x0001_Unknown));
            SetControlState(cbUnknownMapFlag0x0002,
                nameof(IMPD_AllFlags.Bit_0x0002_Unknown),
                nameof(IMPD_AllFlags.CanSet_0x0002_Unknown));
            SetControlState(cbHasSurfaceTextureRotation,
                nameof(IMPD_AllFlags.Bit_0x0002_HasSurfaceTextureRotation),
                nameof(IMPD_AllFlags.CanSet_0x0002_HasSurfaceTextureRotation));
            SetControlState(cbAddDotProductBasedNoiseToStandardLightmap,
                nameof(IMPD_AllFlags.Bit_0x0004_AddDotProductBasedNoiseToStandardLightmap),
                nameof(IMPD_AllFlags.CanSet_0x0004_AddDotProductBasedNoiseToStandardLightmap));
            SetControlState(cbKeepTexturelessFlatTiles,
                nameof(IMPD_AllFlags.Bit_0x0008_KeepTexturelessFlatTiles),
                nameof(IMPD_AllFlags.CanSet_0x0008_KeepTexturelessFlatTiles));
            SetControlState(cbUnknownMapFlag0x0020,
                nameof(IMPD_AllFlags.Bit_0x0020_Unknown),
                nameof(IMPD_AllFlags.CanSet_0x0020_Unknown));
            SetControlState(cbHasChunk19Model,
                nameof(IMPD_AllFlags.Bit_0x0080_HasChunk19ModelWithChunk10Textures),
                nameof(IMPD_AllFlags.CanSet_0x0080_HasChunk19ModelWithChunk10Textures));
            SetControlState(cbModifyPalette1ForGradient,
                nameof(IMPD_AllFlags.Bit_0x0080_SetMSBForPalette1),
                nameof(IMPD_AllFlags.CanSet_0x0080_SetMSBForPalette1));
            SetControlState(cbHasModels,
                nameof(IMPD_AllFlags.Bit_0x0100_HasModels),
                nameof(IMPD_AllFlags.CanSet_0x0100_HasModels));
            SetControlState(cbHasSurfaceModel,
                nameof(IMPD_AllFlags.Bit_0x0200_HasSurfaceModel),
                nameof(IMPD_AllFlags.CanSet_0x0200_HasSurfaceModel));
            SetControlState(cbHasCutsceneSkyBox,
                nameof(IMPD_AllFlags.Bit_0x0800_HasCutsceneSkyBox),
                nameof(IMPD_AllFlags.CanSet_0x0800_HasCutsceneSkyBox));
            SetControlState(cbHasBattleSkyBox,
                nameof(IMPD_AllFlags.Bit_0x2000_HasBattleSkyBox),
                nameof(IMPD_AllFlags.CanSet_0x2000_HasBattleSkyBox));
            SetControlState(cbNarrowAngleBasedLightmap,
                nameof(IMPD_AllFlags.Bit_0x2000_NarrowAngleBasedLightmap),
                nameof(IMPD_AllFlags.CanSet_0x2000_NarrowAngleBasedLightmap));
            SetControlState(cbHasExtraChunk1ModelWithChunk21Textures,
                nameof(IMPD_AllFlags.Bit_0x4000_HasExtraChunk1ModelWithChunk21Textures),
                nameof(IMPD_AllFlags.CanSet_0x4000_HasExtraChunk1ModelWithChunk21Textures));
            SetControlState(cbChunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists,
                nameof(IMPD_AllFlags.Bit_0x8000_Chunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists),
                nameof(IMPD_AllFlags.CanSet_0x8000_Chunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists));
            SetControlState(cbChunk20IsSurfaceModelIfExists,
                nameof(IMPD_AllFlags.Bit_0x8000_Chunk20IsSurfaceModelIfExists),
                nameof(IMPD_AllFlags.CanSet_0x8000_Chunk20IsSurfaceModelIfExists));

            SetControlState(cbChunk1IsLoadedFromLowMemory,
                nameof(IMPD_AllFlags.Chunk1IsLoadedFromLowMemory),
                null);
            SetControlState(cbChunk1IsLoadedFromHighMemory,
                nameof(IMPD_AllFlags.Chunk1IsLoadedFromHighMemory),
                null);
            SetControlState(cbChunk1IsModels,
                nameof(IMPD_AllFlags.Chunk1IsModels),
                null);
            SetControlState(cbChunk2IsSurfaceModel,
                nameof(IMPD_AllFlags.Chunk2IsSurfaceModel),
                null);
            SetControlState(cbChunk20IsSurfaceModel,
                nameof(IMPD_AllFlags.Chunk20IsSurfaceModel),
                null);
            SetControlState(cbChunk20IsModels,
                nameof(IMPD_AllFlags.Chunk20IsModels),
                null);
            SetControlState(cbHighMemoryHasModels,
                nameof(IMPD_AllFlags.HighMemoryHasModels),
                null);
            SetControlState(cbHighMemoryHasSurfaceModel,
                nameof(IMPD_AllFlags.HighMemoryHasSurfaceModel),
                null);
            SetControlState(cbHasAnySkyBox,
                nameof(IMPD_AllFlags.HasAnySkyBox),
                null);

            _inUpdateFlagsFromSource = false;
        }

        private void SetControlState(CheckBox control, string propertyName, string canSetPropertyName) {
            // Can't do anything if there's no associated IMPD_Flags source.
            if (FlagsSource == null) {
                control.Enabled = false;
                control.Checked = false;
                return;
            }

            // Get the property to check.
            var propertyInfo = FlagsSource.GetType().GetProperty(propertyName);
            if (propertyInfo == null) {
                control.Enabled = false;
                control.Checked = false;
                return;
            }

            // Get the current value. Bail if this property cannot be fetched.
            var getter = propertyInfo.GetGetMethod();
            if (getter == null) {
                control.Enabled = false;
                control.Checked = false;
                return;
            }

            // We can get the value. We can now set the state.
            control.Enabled = CanSetIsTrue(canSetPropertyName) && propertyInfo.GetSetMethod() != null;
            control.Checked = (bool) getter.Invoke(FlagsSource, null);
        }

        private void SetControlState<T>(ComboBox control, string propertyName, string canSetPropertyName) where T : struct {
            // Can't do anything if there's no associated IMPD_Flags source.
            if (FlagsSource == null) {
                control.Enabled = false;
                control.SelectedItem = null;
                return;
            }

            // Get the property to check.
            var propertyInfo = FlagsSource.GetType().GetProperty(propertyName);
            if (propertyInfo == null) {
                control.Enabled = false;
                control.SelectedItem = null;
                return;
            }

            // Get the current value. Bail if this property cannot be fetched.
            var getter = propertyInfo.GetGetMethod();
            if (getter == null) {
                control.Enabled = false;
                control.SelectedItem = null;
                return;
            }

            // We can get the value. We can now set the state.
            control.Enabled = CanSetIsTrue(canSetPropertyName) && propertyInfo.GetSetMethod() != null;
            var comboBoxItems = control.Items.Cast<ComboBoxValue<T>>().ToArray();
            control.SelectedItem = comboBoxItems.FirstOrDefault(x => x.Value.Equals((T) getter.Invoke(FlagsSource, null)));
        }

        private void TrySetProperty(CheckBox control, string propertyName, string canSetPropertyName) {
            // Can't do anything if there's no associated IMPD_Flags source.
            if (FlagsSource == null) {
                control.Enabled = false;
                return;
            }

            // Get the property to modify.
            var propertyInfo = FlagsSource.GetType().GetProperty(propertyName);
            if (propertyInfo == null) {
                control.Enabled = false;
                return;
            }

            // Get the old value. Bail if this property cannot be fetched.
            var getter = propertyInfo.GetGetMethod();
            if (getter == null) {
                control.Enabled = false;
                return;
            }
            var oldValue = (bool) getter.Invoke(FlagsSource, null);

            // Don't do anything if this property can't be modified. Update the 'checked' value to undo any changes.
            var setter = propertyInfo.GetSetMethod();
            if (setter == null) {
                control.Enabled = false;
                control.Checked = oldValue;
                return;
            }

            // Getter and setter are available - let's assume we can modify this control.
            control.Enabled = CanSetIsTrue(canSetPropertyName);

            // Don't do anything if there will be no change.
            var newValue = control.Checked;
            if (oldValue == newValue)
                return;

            // Change the property.
            setter.Invoke(FlagsSource, [newValue]);

            // Check to see if the value really changed. If not, reset the 'Checked' value and abort.
            newValue = (bool) getter.Invoke(FlagsSource, null);
            if (oldValue == newValue) {
                control.Checked = oldValue;
                return;
            }

            // Changes were made; update all checkboxes.
            UpdateFlagsFromSource();
        }

        private void TrySetProperty<T>(ComboBox control, string propertyName, string canSetPropertyName) where T : struct {
            // Can't do anything if there's no associated IMPD_Flags source.
            if (FlagsSource == null) {
                control.Enabled = false;
                return;
            }

            // Get the property to modify.
            var propertyInfo = FlagsSource.GetType().GetProperty(propertyName);
            if (propertyInfo == null) {
                control.Enabled = false;
                return;
            }

            // Get the old value. Bail if this property cannot be fetched.
            var getter = propertyInfo.GetGetMethod();
            if (getter == null) {
                control.Enabled = false;
                return;
            }
            var oldValue = (T) getter.Invoke(FlagsSource, null);

            // Some helpers for setting values.
            var comboBoxValues = control.Items.Cast<ComboBoxValue<T>>().ToArray();
            void SetComboBoxValueTo(T value) {
                var comboBoxValue = comboBoxValues.FirstOrDefault(x => x.Value.Equals(value));
                control.SelectedItem = comboBoxValue;
            }

            // Don't do anything if this property can't be modified. Update the 'checked' value to undo any changes.
            var setter = propertyInfo.GetSetMethod();
            if (setter == null) {
                control.Enabled = false;
                SetComboBoxValueTo(oldValue);
                return;
            }

            // Getter and setter are available - let's assume we can modify this control.
            control.Enabled = CanSetIsTrue(canSetPropertyName);

            // Don't do anything if there will be no change.
            var newValue = ((ComboBoxValue<T>) control.SelectedItem)?.Value;
            if (oldValue.Equals(newValue))
                return;

            // Change the property.
            setter.Invoke(FlagsSource, [newValue]);

            // Check to see if the value really changed. If not, reset the 'Checked' value and abort.
            newValue = (T) getter.Invoke(FlagsSource, null);
            if (oldValue.Equals(newValue)) {
                SetComboBoxValueTo(oldValue);
                return;
            }

            // Changes were made; update all checkboxes.
            UpdateFlagsFromSource();
        }

        private bool CanSetIsTrue(string propertyName) {
            if (propertyName == null)
                return true;
            if (FlagsSource == null)
                return false;

            var propertyInfo = FlagsSource.GetType().GetProperty(propertyName);
            if (propertyInfo == null)
                return false;

            var getter = propertyInfo.GetGetMethod();
            if (getter == null)
                return false;

            return (bool) getter.Invoke(FlagsSource, null);
        }
    }
}
