using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.ModelLoaders;
using SF3.Models.Structs.X033_X031;
using SF3.NamedValues;
using SF3.Types;
using SF3.Win;
using SF3.Win.Views;
using static SF3.Utils.FileUtils;

namespace SF3.Editor.Forms {
    public partial class SF3EditorForm : Form {
        public static readonly string Version = "0.1.1 (DEV 2025-04-11)";

        private readonly Dictionary<ScenarioType, INameGetterContext> c_nameGetterContexts = Enum.GetValues<ScenarioType>()
            .ToDictionary(x => x, x => (INameGetterContext) new NameGetterContext(x));

        private readonly string OpenDialogFilter =
            "All Supported Files|" + string.Join(';', Enum.GetValues<SF3FileType>().Select(x => GetFileFilterForFileType(x)).Distinct())
            + "|" + string.Join('|', Enum.GetValues<SF3FileType>().Select(x => GetFileDialogFilterForFileType(x)).Distinct())
            + "|All Files (*.*)|*.*"
            ;

        public SF3EditorForm() {
            SuspendLayout();
            InitializeComponent();

            // The application state should never change for the app's lifetime.
            _appState = AppState.RetrieveAppState();

            // Create a container for all files.
            _fileContainerView = new TabView("File Container", lazyLoad: false);
            _ = _fileContainerView.Create();
            var fileContainerControl = _fileContainerView.TabControl;
            Controls.Add(fileContainerControl);
            fileContainerControl.Dock = DockStyle.Fill;
            fileContainerControl.BringToFront(); // If this isn't in the front, the menu is placed behind it (eep)
            fileContainerControl.Selected += (s, e) => FocusFileTab(e.TabPage);

            // Store the original title in a few different forms. It's going to be changing around.
            _baseTitle = Text;
            _versionTitle = _baseTitle + " v" + Version;
            Text = _versionTitle;

            // Remember the last Scenario type to open.
            UpdateLocalOpenScenario();
            _appState.OpenScenarioChanged += (s, e) => {
                UpdateLocalOpenScenario();
                _appState.Serialize();
            };

            tsmiFile_SwapToPrev.ShortcutKeyDisplayString = "Ctrl+Alt+,";
            tsmiFile_SwapToPrev.ShowShortcutKeys = true;
            tsmiFile_SwapToNext.ShortcutKeyDisplayString = "Ctrl+Alt+.";
            tsmiFile_SwapToNext.ShowShortcutKeys = true;

            // Link some dropdowns/values to the app state.
            tsmiEdit_UseDropdowns.Checked = _appState.UseDropdownsForNamedValues;
            _appState.UseDropdownsForNamedValuesChanged += (s, e) => {
                tsmiEdit_UseDropdowns.Checked = _appState.UseDropdownsForNamedValues;
                _appState.Serialize();
            };

            tsmiMPD_EnableBlankFieldV2Controls.Checked = _appState.EnableExperimentalBlankFieldV2Brushes;
            _appState.EnableExperimentalBlankFieldV2BrushesChanged += (s, e) => {
                tsmiMPD_EnableBlankFieldV2Controls.Checked = _appState.EnableExperimentalBlankFieldV2Brushes;
                _appState.Serialize();
            };

            tsmiEdit_EnableDebugSettings.Checked = _appState.EnableDebugSettings;
            Stats.DebugGrowthValues = _appState.EnableDebugSettings;

            _appState.EnableDebugSettingsChanged += (s, e) => {
                tsmiEdit_EnableDebugSettings.Checked = _appState.EnableDebugSettings;
                Stats.DebugGrowthValues = _appState.EnableDebugSettings;
                _appState.Serialize();
            };

            UpdateRecentFilesMenu();
            _appState.RecentFilesChanged += (s, e) => UpdateRecentFilesMenu();

            tsmiMPD_View_DrawSurfaceModel.Checked        = _appState.ViewerDrawSurfaceModel;
            tsmiMPD_View_DrawModels.Checked              = _appState.ViewerDrawModels;
            tsmiMPD_View_DrawGround.Checked              = _appState.ViewerDrawGround;
            tsmiMPD_View_DrawSkyBox.Checked              = _appState.ViewerDrawSkyBox;
            tsmiMPD_View_RunAnimations.Checked           = _appState.ViewerRunAnimations;
            tsmiMPD_View_ApplyLighting.Checked           = _appState.ViewerApplyLighting;
            tsmiMPD_View_DrawGradients.Checked           = _appState.ViewerDrawGradients;
            tsmiMPD_View_DrawWireframes.Checked          = _appState.ViewerDrawWireframe;
            tsmiMPD_View_DrawBoundaries.Checked          = _appState.ViewerDrawBoundaries;
            tsmiMPD_View_DrawTerrainTypes.Checked        = _appState.ViewerDrawTerrainTypes;
            tsmiMPD_View_DrawEventIDs.Checked            = _appState.ViewerDrawEventIDs;
            tsmiMPD_View_DrawCollisionLines.Checked      = _appState.ViewerDrawCollisionLines;
            tsmiMPD_View_DrawNormalMap.Checked           = _appState.ViewerDrawNormals;
            tsmiMPD_View_RotateSpritesUpToCamera.Checked = _appState.ViewerRotateSpritesUp;

            _appState.ViewerDrawSurfaceModelChanged   += (s, e) => { tsmiMPD_View_DrawSurfaceModel.Checked        = _appState.ViewerDrawSurfaceModel;   _appState.Serialize(); };
            _appState.ViewerDrawModelsChanged         += (s, e) => { tsmiMPD_View_DrawModels.Checked              = _appState.ViewerDrawModels;         _appState.Serialize(); };
            _appState.ViewerDrawGroundChanged         += (s, e) => { tsmiMPD_View_DrawGround.Checked              = _appState.ViewerDrawGround;         _appState.Serialize(); };
            _appState.ViewerDrawSkyBoxChanged         += (s, e) => { tsmiMPD_View_DrawSkyBox.Checked              = _appState.ViewerDrawSkyBox;         _appState.Serialize(); };
            _appState.ViewerRunAnimationsChanged      += (s, e) => { tsmiMPD_View_RunAnimations.Checked           = _appState.ViewerRunAnimations;      _appState.Serialize(); };
            _appState.ViewerApplyLightingChanged      += (s, e) => { tsmiMPD_View_ApplyLighting.Checked           = _appState.ViewerApplyLighting;      _appState.Serialize(); };
            _appState.ViewerDrawGradientsChanged      += (s, e) => { tsmiMPD_View_DrawGradients.Checked           = _appState.ViewerDrawGradients;      _appState.Serialize(); };
            _appState.ViewerDrawWireframeChanged      += (s, e) => { tsmiMPD_View_DrawWireframes.Checked          = _appState.ViewerDrawWireframe;      _appState.Serialize(); };
            _appState.ViewerDrawBoundariesChanged     += (s, e) => { tsmiMPD_View_DrawBoundaries.Checked          = _appState.ViewerDrawBoundaries;     _appState.Serialize(); };
            _appState.ViewerDrawTerrainTypesChanged   += (s, e) => { tsmiMPD_View_DrawTerrainTypes.Checked        = _appState.ViewerDrawTerrainTypes;   _appState.Serialize(); };
            _appState.ViewerDrawEventIDsChanged       += (s, e) => { tsmiMPD_View_DrawEventIDs.Checked            = _appState.ViewerDrawEventIDs;       _appState.Serialize(); };
            _appState.ViewerDrawCollisionLinesChanged += (s, e) => { tsmiMPD_View_DrawCollisionLines.Checked      = _appState.ViewerDrawCollisionLines; _appState.Serialize(); };
            _appState.ViewerDrawNormalsChanged        += (s, e) => { tsmiMPD_View_DrawNormalMap.Checked           = _appState.ViewerDrawNormals;        _appState.Serialize(); };
            _appState.ViewerRotateSpritesUpChanged    += (s, e) => { tsmiMPD_View_RotateSpritesUpToCamera.Checked = _appState.ViewerRotateSpritesUp;    _appState.Serialize(); };

            ResumeLayout();
        }

        protected override void OnFormClosing(FormClosingEventArgs e) {
            if (!CloseAllFiles())
                e.Cancel = true;
            base.OnFormClosing(e);
        }

        /// <summary>
        /// Information about a file loaded into the SF3Editor, including UI object references.
        /// </summary>
        public class LoadedFile {
            public LoadedFile(ModelFileLoader loader, ScenarioType scenario, SF3FileType fileType, TabPage tabPage, FileView view) {
                Loader   = loader;
                Scenario = scenario;
                FileType = fileType;
                TabPage  = tabPage;
                View     = view;
            }

            public readonly ModelFileLoader Loader;
            public readonly ScenarioType Scenario;
            public readonly SF3FileType FileType;
            public readonly TabPage TabPage;
            public readonly FileView View;
        };

        /// <summary>
        /// The file/tab currently selected.
        /// </summary>
        public LoadedFile? SelectedFile {
            get => _selectedFile;
            private set {
                if (_selectedFile != value) {
                    _selectedFile = value;
                    if (_selectedFile != null)
                        _selectedFile.TabPage.Select();
                }
            }
        }
        private LoadedFile? _selectedFile = null;

        private List<LoadedFile> _loadedFiles = [];

        private readonly string _baseTitle;
        private readonly string _versionTitle;
        private readonly TabView _fileContainerView;
        private readonly AppState _appState;
    }
}
