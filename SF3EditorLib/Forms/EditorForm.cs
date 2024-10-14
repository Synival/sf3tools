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
        private IFileEditor _fileEditor;

        public IFileEditor FileEditor
        {
            get => _fileEditor;
            set => _fileEditor = value;
        }

        public string BaseTitle { get; protected set; }

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
        /// The title to set when using UpdateTitle().
        /// </summary>
        /// <returns></returns>
        protected virtual string MakeTitle() => _fileEditor?.EditorTitle(BaseTitle) ?? BaseTitle;

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
            FileEditor.CloseFile();
            FileEditor = null;
        }
    }
}
