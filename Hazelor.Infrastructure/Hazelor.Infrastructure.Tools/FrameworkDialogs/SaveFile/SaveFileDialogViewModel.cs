using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hazelor.Infrastructure.FrameworkDialogs.SaveFile
{
    public class SaveFileDialogViewModel : FileDialogViewModel, ISaveFileDialog
    {
        /// <summary>
        /// Gets or sets a value indicating whether the dialog box allows multiple files to be selected.
        /// </summary>
        public bool Multiselect { get; set; }
    }
}
