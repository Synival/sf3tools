using BrightIdeasSoftware;
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
        protected IFileEditor FileEditor { get; set; }

        public string BaseTitle { get; protected set; }

        protected List<ObjectListView> ObjectListViews { get; set; }

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
        protected virtual string MakeTitle() => FileEditor?.EditorTitle(BaseTitle) ?? BaseTitle;

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
    }
}
