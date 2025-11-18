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
            SuspendLayout();
            InitializeComponent();

            cbBackgroundImageType.Items.AddRange(c_backgroundImageComboBoxValues.Values.ToArray());
            cbGroundImageType.Items.AddRange(c_groundImageComboBoxValues.Values.ToArray());

            cbUnknownMapFlag0x0001.CheckedChanged += (s, e)
                => TrySetProperty(cbUnknownMapFlag0x0001, nameof(IMPD_Flags.UnknownMapFlag0x0001), null);
            cbUnknownMapFlag0x0002.CheckedChanged += (s, e)
                => TrySetProperty(cbUnknownMapFlag0x0002, nameof(IMPD_Flags.UnknownMapFlag0x0002), null);
            cbHasSurfaceTextureRotation.CheckedChanged += (s, e)
                => TrySetProperty(cbHasSurfaceTextureRotation, nameof(IMPD_Flags.HasSurfaceTextureRotation), null);
            cbAddDotProductBasedNoiseToStandardLightmap.CheckedChanged += (s, e)
                => TrySetProperty(cbAddDotProductBasedNoiseToStandardLightmap, nameof(IMPD_Flags.AddDotProductBasedNoiseToStandardLightmap), null);
            cbUnknownFlatTileFlag0x0008.CheckedChanged += (s, e)
                => TrySetProperty(cbUnknownFlatTileFlag0x0008, nameof(IMPD_Flags.UnknownFlatTileFlag0x0008), null);
            cbBackgroundImageType.SelectedIndexChanged += (s, e)
                => TrySetProperty<BackgroundImageType>(cbBackgroundImageType, nameof(IMPD_Flags.BackgroundImageType), null);
            cbUnknownMapFlag0x0020.CheckedChanged += (s, e)
                => TrySetProperty(cbUnknownMapFlag0x0020, nameof(IMPD_Flags.UnknownMapFlag0x0020), null);
            cbHasChunk19Model.CheckedChanged += (s, e)
                => TrySetProperty(cbHasChunk19Model, nameof(IMPD_Flags.HasChunk19Model), null);
            cbModifyPalette1ForGradient.CheckedChanged += (s, e)
                => TrySetProperty(cbModifyPalette1ForGradient, nameof(IMPD_Flags.ModifyPalette1ForGradient), null);
            cbHasModels.CheckedChanged += (s, e)
                => TrySetProperty(cbHasModels, nameof(IMPD_Flags.HasModels), null);
            cbHasSurfaceModel.CheckedChanged += (s, e)
                => TrySetProperty(cbHasSurfaceModel, nameof(IMPD_Flags.HasSurfaceModel), null);
            cbGroundImageType.SelectedIndexChanged += (s, e)
                => TrySetProperty<GroundImageType>(cbGroundImageType, nameof(IMPD_Flags.GroundImageType), null);
            cbHasCutsceneSkyBox.CheckedChanged += (s, e)
                => TrySetProperty(cbHasCutsceneSkyBox, nameof(IMPD_Flags.HasCutsceneSkyBox), null);
            cbHasBattleSkyBox.CheckedChanged += (s, e)
                => TrySetProperty(cbHasBattleSkyBox, nameof(IMPD_Flags.HasBattleSkyBox), null);
            cbNarrowAngleBasedLightmap.CheckedChanged += (s, e)
                => TrySetProperty(cbNarrowAngleBasedLightmap, nameof(IMPD_Flags.NarrowAngleBasedLightmap), null);
            cbHasExtraChunk1ModelWithChunk21Textures.CheckedChanged += (s, e)
                => TrySetProperty(cbHasExtraChunk1ModelWithChunk21Textures, nameof(IMPD_Flags.HasExtraChunk1ModelWithChunk21Textures), null);
            cbChunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists.CheckedChanged += (s, e)
                => TrySetProperty(cbChunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists, nameof(IMPD_Flags.Chunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists), null);
            cbChunk20IsSurfaceModelIfExists.CheckedChanged += (s, e)
                => TrySetProperty(cbChunk20IsSurfaceModelIfExists, nameof(IMPD_Flags.Chunk20IsSurfaceModelIfExists), null);

            // Derived values
            cbChunk1IsLoadedFromLowMemory.CheckedChanged += (s, e)
                => TrySetProperty(cbChunk1IsLoadedFromLowMemory, nameof(IMPD_Flags.Chunk1IsLoadedFromLowMemory), null);
            cbChunk1IsLoadedFromHighMemory.CheckedChanged += (s, e)
                => TrySetProperty(cbChunk1IsLoadedFromHighMemory, nameof(IMPD_Flags.Chunk1IsLoadedFromHighMemory), null);
            cbChunk1IsModels.CheckedChanged += (s, e)
                => TrySetProperty(cbChunk1IsModels, nameof(IMPD_Flags.Chunk1IsModels), null);
            cbChunk2IsSurfaceModel.CheckedChanged += (s, e)
                => TrySetProperty(cbChunk2IsSurfaceModel, nameof(IMPD_Flags.Chunk2IsSurfaceModel), null);
            cbChunk20IsSurfaceModel.CheckedChanged += (s, e)
                => TrySetProperty(cbChunk20IsSurfaceModel, nameof(IMPD_Flags.Chunk20IsSurfaceModel), null);
            cbChunk20IsModels.CheckedChanged += (s, e)
                => TrySetProperty(cbChunk20IsModels, nameof(IMPD_Flags.Chunk20IsModels), null);
            cbHighMemoryHasModels.CheckedChanged += (s, e)
                => TrySetProperty(cbHighMemoryHasModels, nameof(IMPD_Flags.HighMemoryHasModels), null);
            cbHighMemoryHasSurfaceModel.CheckedChanged += (s, e)
                => TrySetProperty(cbHighMemoryHasSurfaceModel, nameof(IMPD_Flags.HighMemoryHasSurfaceModel), null);
            cbHasAnySkyBox.CheckedChanged += (s, e)
                => TrySetProperty(cbHasAnySkyBox, nameof(IMPD_Flags.HasAnySkyBox), null);

            ResumeLayout();
        }

        private IMPD_Flags _flagsSource = null;
        public IMPD_Flags FlagsSource {
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

            SetControlState(cbUnknownMapFlag0x0001,                                 nameof(IMPD_Flags.UnknownMapFlag0x0001), null);
            SetControlState(cbUnknownMapFlag0x0002,                                 nameof(IMPD_Flags.UnknownMapFlag0x0002), null);
            SetControlState(cbHasSurfaceTextureRotation,                            nameof(IMPD_Flags.HasSurfaceTextureRotation), null);
            SetControlState(cbAddDotProductBasedNoiseToStandardLightmap,            nameof(IMPD_Flags.AddDotProductBasedNoiseToStandardLightmap), null);
            SetControlState(cbUnknownFlatTileFlag0x0008,                            nameof(IMPD_Flags.UnknownFlatTileFlag0x0008), null);
            SetControlState<BackgroundImageType>(cbBackgroundImageType,             nameof(IMPD_Flags.BackgroundImageType), null);
            SetControlState(cbUnknownMapFlag0x0020,                                 nameof(IMPD_Flags.UnknownMapFlag0x0020), null);
            SetControlState(cbHasChunk19Model,                                      nameof(IMPD_Flags.HasChunk19Model), null);
            SetControlState(cbModifyPalette1ForGradient,                            nameof(IMPD_Flags.ModifyPalette1ForGradient), null);
            SetControlState(cbHasModels,                                            nameof(IMPD_Flags.HasModels), null);
            SetControlState(cbHasSurfaceModel,                                      nameof(IMPD_Flags.HasSurfaceModel), null);
            SetControlState<GroundImageType>(cbGroundImageType,                     nameof(IMPD_Flags.GroundImageType), null);
            SetControlState(cbHasCutsceneSkyBox,                                    nameof(IMPD_Flags.HasCutsceneSkyBox), null);
            SetControlState(cbHasBattleSkyBox,                                      nameof(IMPD_Flags.HasBattleSkyBox), null);
            SetControlState(cbNarrowAngleBasedLightmap,                             nameof(IMPD_Flags.NarrowAngleBasedLightmap), null);
            SetControlState(cbHasExtraChunk1ModelWithChunk21Textures,               nameof(IMPD_Flags.HasExtraChunk1ModelWithChunk21Textures), null);
            SetControlState(cbChunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists, nameof(IMPD_Flags.Chunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists), null);
            SetControlState(cbChunk20IsSurfaceModelIfExists,                        nameof(IMPD_Flags.Chunk20IsSurfaceModelIfExists), null);

            SetControlState(cbChunk1IsLoadedFromLowMemory,                          nameof(IMPD_Flags.Chunk1IsLoadedFromLowMemory), null);
            SetControlState(cbChunk1IsLoadedFromHighMemory,                         nameof(IMPD_Flags.Chunk1IsLoadedFromHighMemory), null);
            SetControlState(cbChunk1IsModels,                                       nameof(IMPD_Flags.Chunk1IsModels), null);
            SetControlState(cbChunk2IsSurfaceModel,                                 nameof(IMPD_Flags.Chunk2IsSurfaceModel), null);
            SetControlState(cbChunk20IsSurfaceModel,                                nameof(IMPD_Flags.Chunk20IsSurfaceModel), null);
            SetControlState(cbChunk20IsModels,                                      nameof(IMPD_Flags.Chunk20IsModels), null);
            SetControlState(cbHighMemoryHasModels,                                  nameof(IMPD_Flags.HighMemoryHasModels), null);
            SetControlState(cbHighMemoryHasSurfaceModel,                            nameof(IMPD_Flags.HighMemoryHasSurfaceModel), null);
            SetControlState(cbHasAnySkyBox,                                         nameof(IMPD_Flags.HasAnySkyBox), null);

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
            control.Enabled = propertyInfo.GetSetMethod() != null;
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
            control.Enabled = propertyInfo.GetSetMethod() != null;
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
            control.Enabled = true;

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
            control.Enabled = true;

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
    }
}
