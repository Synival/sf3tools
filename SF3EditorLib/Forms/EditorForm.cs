using BrightIdeasSoftware;
using SF3.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SF3.Editor.Forms
{
    /// <summary>
    /// Base editor form from which all other editors are derived.
    /// </summary>
    public partial class EditorForm : Form
    {
        /// <summary>
        /// FileEditor open for the current file.
        /// </summary>
        protected IFileEditor FileEditor { get; set; }

        /// <summary>
        /// Title of the form set in the designer. Should be set after derived class' InitializeComponent().
        /// </summary>
        public string BaseTitle { get; protected set; }

        /// <summary>
        /// All ObjectListView's present in the form. Populated automatically.
        /// </summary>
        protected List<ObjectListView> ObjectListViews { get; private set; }

        private ScenarioType _scenario = ScenarioType.Scenario1;

        /// <summary>
        /// The Scenario set for editing.
        /// </summary>
        public ScenarioType Scenario
        {
            get => _scenario;
            set
            {
                if (_scenario != value)
                {
                    _scenario = value;
                    ScenarioChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public EditorForm()
        {
            InitializeComponent();
        }

        public EditorForm(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        /// <summary>
        /// Function to be called after derived class's InitializeComponent() is called.
        /// </summary>
        public void FinalizeForm()
        {
            ObjectListViews = SF3.Utils.GetAllObjectsOfTypeInFields<ObjectListView>(this, false);
            UpdateTitle();
        }

        /// <summary>
        /// The title to set when using UpdateTitle().
        /// </summary>
        /// <returns></returns>
        protected virtual string MakeTitle() => (FileEditor?.IsLoaded == true)
            ? FileEditor.EditorTitle(BaseTitle)
            : BaseTitle;

        /// <summary>
        /// Updates the title of the form.
        /// </summary>
        protected void UpdateTitle()
        {
            this.Text = MakeTitle();
        }

        /// <summary>
        /// Closes a file if open.
        /// </summary>
        public virtual void CloseFile()
        {
            if (FileEditor == null)
            {
                return;
            }

            ObjectListViews.ForEach(x => x.ClearObjects());
            FileEditor.CloseFile();
            FileEditor = null;
        }

        /// <summary>
        /// Triggered when Scenario has a new value.
        /// </summary>
        public event EventHandler ScenarioChanged;
    }
}
