using System.Linq;
using System.Windows.Forms;
using CommonLib.Win.Controls;
using SF3.MPD;

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

            cb0x0001_Unknown.CheckedChanged += (s, e)
                => TrySetProperty(cb0x0001_Unknown,
                    nameof(IMPD_AllFlags.Bit_0x0001_Unknown),
                    nameof(IMPD_AllFlags.CanSet_0x0001_Unknown));
            cb0x0002_Unknown.CheckedChanged += (s, e)
                => TrySetProperty(cb0x0002_Unknown,
                    nameof(IMPD_AllFlags.Bit_0x0002_Unknown),
                    nameof(IMPD_AllFlags.CanSet_0x0002_Unknown));
            cb0x0002_HasSurfaceTextureRotation.CheckedChanged += (s, e)
                => TrySetProperty(cb0x0002_HasSurfaceTextureRotation,
                    nameof(IMPD_AllFlags.Bit_0x0002_HasSurfaceTextureRotation),
                    nameof(IMPD_AllFlags.CanSet_0x0002_HasSurfaceTextureRotation));
            cb0x0004_AddDotProductBasedNoiseToStandardLightmap.CheckedChanged += (s, e)
                => TrySetProperty(cb0x0004_AddDotProductBasedNoiseToStandardLightmap,
                    nameof(IMPD_AllFlags.Bit_0x0004_AddDotProductBasedNoiseToStandardLightmap),
                    nameof(IMPD_AllFlags.CanSet_0x0004_AddDotProductBasedNoiseToStandardLightmap));
            cb0x0008_KeepTexturelessFlatTiles.CheckedChanged += (s, e)
                => TrySetProperty(cb0x0008_KeepTexturelessFlatTiles,
                    nameof(IMPD_AllFlags.Bit_0x0008_KeepTexturelessFlatTiles),
                    nameof(IMPD_AllFlags.CanSet_0x0008_KeepTexturelessFlatTiles));
            cb0x0020_Unknown.CheckedChanged += (s, e)
                => TrySetProperty(cb0x0020_Unknown,
                    nameof(IMPD_AllFlags.Bit_0x0020_Unknown),
                    nameof(IMPD_AllFlags.CanSet_0x0020_Unknown));
            cb0x0040_HasBackgroundImage.CheckedChanged += (s, e)
                => TrySetProperty(cb0x0040_HasBackgroundImage,
                    nameof(IMPD_AllFlags.Bit_0x0040_HasBackgroundImage),
                    nameof(IMPD_AllFlags.CanSet_0x0040_HasBackgroundImage));
            cb0x0080_HasChunk19ModelWithChunk21Textures.CheckedChanged += (s, e)
                => TrySetProperty(cb0x0080_HasChunk19ModelWithChunk21Textures,
                    nameof(IMPD_AllFlags.Bit_0x0080_HasChunk19ModelWithChunk10Textures),
                    nameof(IMPD_AllFlags.CanSet_0x0080_HasChunk19ModelWithChunk10Textures));
            cb0x0080_SetMSBForPalette1.CheckedChanged += (s, e)
                => TrySetProperty(cb0x0080_SetMSBForPalette1,
                    nameof(IMPD_AllFlags.Bit_0x0080_SetMSBForPalette1),
                    nameof(IMPD_AllFlags.CanSet_0x0080_SetMSBForPalette1));
            cb0x0100_HasModels.CheckedChanged += (s, e)
                => TrySetProperty(cb0x0100_HasModels,
                    nameof(IMPD_AllFlags.Bit_0x0100_HasModels),
                    nameof(IMPD_AllFlags.CanSet_0x0100_HasModels));
            cb0x0200_HasSurfaceModel.CheckedChanged += (s, e)
                => TrySetProperty(cb0x0200_HasSurfaceModel,
                    nameof(IMPD_AllFlags.Bit_0x0200_HasSurfaceModel),
                    nameof(IMPD_AllFlags.CanSet_0x0200_HasSurfaceModel));
            cb0x0400_HasGroundImage.CheckedChanged += (s, e)
                => TrySetProperty(cb0x0400_HasGroundImage,
                    nameof(IMPD_AllFlags.Bit_0x0400_HasGroundImage),
                    nameof(IMPD_AllFlags.CanSet_0x0400_HasGroundImage));
            cb0x0800_Unused.CheckedChanged += (s, e)
                => TrySetProperty(cb0x0800_Unused,
                    nameof(IMPD_AllFlags.Bit_0x0800_Unused),
                    nameof(IMPD_AllFlags.CanSet_0x0800_Unused));
            cb0x0800_HasCutsceneSkyBox.CheckedChanged += (s, e)
                => TrySetProperty(cb0x0800_HasCutsceneSkyBox,
                    nameof(IMPD_AllFlags.Bit_0x0800_HasCutsceneSkyBox),
                    nameof(IMPD_AllFlags.CanSet_0x0800_HasCutsceneSkyBox));
            cb0x1000_HasTileBasedBackgroundImage.CheckedChanged += (s, e)
                => TrySetProperty(cb0x1000_HasTileBasedBackgroundImage,
                    nameof(IMPD_AllFlags.Bit_0x1000_HasTileBasedGroundImage),
                    nameof(IMPD_AllFlags.CanSet_0x0800_HasCutsceneSkyBox));
            cb0x2000_HasBattleSkyBox.CheckedChanged += (s, e)
                => TrySetProperty(cb0x2000_HasBattleSkyBox,
                    nameof(IMPD_AllFlags.Bit_0x2000_HasBattleSkyBox),
                    nameof(IMPD_AllFlags.CanSet_0x2000_HasBattleSkyBox));
            cb0x2000_NarrowAngleBasedLightmap.CheckedChanged += (s, e)
                => TrySetProperty(cb0x2000_NarrowAngleBasedLightmap,
                    nameof(IMPD_AllFlags.Bit_0x2000_NarrowAngleBasedLightmap),
                    nameof(IMPD_AllFlags.CanSet_0x2000_NarrowAngleBasedLightmap));
            cb0x4000_Unused.CheckedChanged += (s, e)
                => TrySetProperty(cb0x4000_Unused,
                    nameof(IMPD_AllFlags.Bit_0x4000_Unused),
                    nameof(IMPD_AllFlags.CanSet_0x4000_Unused));
            cb0x4000_HasExtraChunk1ModelWithChunk21Textures.CheckedChanged += (s, e)
                => TrySetProperty(cb0x4000_HasExtraChunk1ModelWithChunk21Textures,
                    nameof(IMPD_AllFlags.Bit_0x4000_HasExtraChunk1ModelWithChunk21Textures),
                    nameof(IMPD_AllFlags.CanSet_0x4000_HasExtraChunk1ModelWithChunk21Textures));
            cb0x8000_ModelsAreStillLowMemoryWithSurfaceModel.CheckedChanged += (s, e)
                => TrySetProperty(cb0x8000_ModelsAreStillLowMemoryWithSurfaceModel,
                    nameof(IMPD_AllFlags.Bit_0x8000_ModelsAreStillLowMemoryWithSurfaceModel),
                    nameof(IMPD_AllFlags.CanSet_0x8000_ModelsAreStillLowMemoryWithSurfaceModel));

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

            SetControlState(cb0x0001_Unknown,
                nameof(IMPD_AllFlags.Bit_0x0001_Unknown),
                nameof(IMPD_AllFlags.CanSet_0x0001_Unknown));
            SetControlState(cb0x0002_Unknown,
                nameof(IMPD_AllFlags.Bit_0x0002_Unknown),
                nameof(IMPD_AllFlags.CanSet_0x0002_Unknown));
            SetControlState(cb0x0002_HasSurfaceTextureRotation,
                nameof(IMPD_AllFlags.Bit_0x0002_HasSurfaceTextureRotation),
                nameof(IMPD_AllFlags.CanSet_0x0002_HasSurfaceTextureRotation));
            SetControlState(cb0x0004_AddDotProductBasedNoiseToStandardLightmap,
                nameof(IMPD_AllFlags.Bit_0x0004_AddDotProductBasedNoiseToStandardLightmap),
                nameof(IMPD_AllFlags.CanSet_0x0004_AddDotProductBasedNoiseToStandardLightmap));
            SetControlState(cb0x0008_KeepTexturelessFlatTiles,
                nameof(IMPD_AllFlags.Bit_0x0008_KeepTexturelessFlatTiles),
                nameof(IMPD_AllFlags.CanSet_0x0008_KeepTexturelessFlatTiles));
            SetControlState(cb0x0010_HasTileBasedForegroundImage,
                nameof(IMPD_AllFlags.Bit_0x0010_HasTileBasedForegroundImage),
                nameof(IMPD_AllFlags.CanSet_0x0010_HasTileBasedForegroundImage));
            SetControlState(cb0x0020_Unknown,
                nameof(IMPD_AllFlags.Bit_0x0020_Unknown),
                nameof(IMPD_AllFlags.CanSet_0x0020_Unknown));
            SetControlState(cb0x0040_HasBackgroundImage,
                nameof(IMPD_AllFlags.Bit_0x0040_HasBackgroundImage),
                nameof(IMPD_AllFlags.CanSet_0x0040_HasBackgroundImage));
            SetControlState(cb0x0080_HasChunk19ModelWithChunk21Textures,
                nameof(IMPD_AllFlags.Bit_0x0080_HasChunk19ModelWithChunk10Textures),
                nameof(IMPD_AllFlags.CanSet_0x0080_HasChunk19ModelWithChunk10Textures));
            SetControlState(cb0x0080_SetMSBForPalette1,
                nameof(IMPD_AllFlags.Bit_0x0080_SetMSBForPalette1),
                nameof(IMPD_AllFlags.CanSet_0x0080_SetMSBForPalette1));
            SetControlState(cb0x0100_HasModels,
                nameof(IMPD_AllFlags.Bit_0x0100_HasModels),
                nameof(IMPD_AllFlags.CanSet_0x0100_HasModels));
            SetControlState(cb0x0200_HasSurfaceModel,
                nameof(IMPD_AllFlags.Bit_0x0200_HasSurfaceModel),
                nameof(IMPD_AllFlags.CanSet_0x0200_HasSurfaceModel));
            SetControlState(cb0x0400_HasGroundImage,
                nameof(IMPD_AllFlags.Bit_0x0400_HasGroundImage),
                nameof(IMPD_AllFlags.CanSet_0x0400_HasGroundImage));
            SetControlState(cb0x0800_Unused,
                nameof(IMPD_AllFlags.Bit_0x0800_Unused),
                nameof(IMPD_AllFlags.CanSet_0x0800_Unused));
            SetControlState(cb0x0800_HasCutsceneSkyBox,
                nameof(IMPD_AllFlags.Bit_0x0800_HasCutsceneSkyBox),
                nameof(IMPD_AllFlags.CanSet_0x0800_HasCutsceneSkyBox));
            SetControlState(cb0x1000_HasTileBasedBackgroundImage,
                nameof(IMPD_AllFlags.Bit_0x1000_HasTileBasedGroundImage),
                nameof(IMPD_AllFlags.CanSet_0x1000_HasTileBasedGroundImage));
            SetControlState(cb0x2000_HasBattleSkyBox,
                nameof(IMPD_AllFlags.Bit_0x2000_HasBattleSkyBox),
                nameof(IMPD_AllFlags.CanSet_0x2000_HasBattleSkyBox));
            SetControlState(cb0x2000_NarrowAngleBasedLightmap,
                nameof(IMPD_AllFlags.Bit_0x2000_NarrowAngleBasedLightmap),
                nameof(IMPD_AllFlags.CanSet_0x2000_NarrowAngleBasedLightmap));
            SetControlState(cb0x4000_Unused,
                nameof(IMPD_AllFlags.Bit_0x4000_Unused),
                nameof(IMPD_AllFlags.CanSet_0x4000_Unused));
            SetControlState(cb0x4000_HasExtraChunk1ModelWithChunk21Textures,
                nameof(IMPD_AllFlags.Bit_0x4000_HasExtraChunk1ModelWithChunk21Textures),
                nameof(IMPD_AllFlags.CanSet_0x4000_HasExtraChunk1ModelWithChunk21Textures));
            SetControlState(cb0x8000_ModelsAreStillLowMemoryWithSurfaceModel,
                nameof(IMPD_AllFlags.Bit_0x8000_ModelsAreStillLowMemoryWithSurfaceModel),
                nameof(IMPD_AllFlags.CanSet_0x8000_ModelsAreStillLowMemoryWithSurfaceModel));

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
