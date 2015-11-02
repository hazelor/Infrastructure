using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hazelor.Infrastructure.FrameworkDialogs.OpenFile
{
    /// <summary>
    /// ViewModel of the OpenFileDialog.
    /// </summary>
    public class OpenFileDialogViewModel : FileDialogViewModel, IOpenFileDialog
    {
        /// <summary>
        /// Gets or sets a value indicating whether the dialog box allows multiple files to be selected.
        /// </summary>
        public bool Multiselect { get; set; }
    }
}
