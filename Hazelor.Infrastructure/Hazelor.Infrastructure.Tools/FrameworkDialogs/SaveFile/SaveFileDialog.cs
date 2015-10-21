using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsSaveFileDialog = System.Windows.Forms.SaveFileDialog;

namespace Hazelor.Infrastructure.FrameworkDialogs.SaveFile
{
    public class SaveFileDialog : IDisposable
    {
        private readonly ISaveFileDialog saveFileDialog;
        private WinFormsSaveFileDialog concreteSaveFileDialog;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenFileDialog"/> class.
        /// </summary>
        /// <param name="openFileDialog">The interface of a open file dialog.</param>
        public SaveFileDialog(ISaveFileDialog saveFileDialog)
        {
            Contract.Requires(saveFileDialog != null);

            this.saveFileDialog = saveFileDialog;

            // Create concrete OpenFileDialog
            concreteSaveFileDialog = new WinFormsSaveFileDialog
            {
                AddExtension = saveFileDialog.AddExtension,
                CheckFileExists = saveFileDialog.CheckFileExists,
                CheckPathExists = saveFileDialog.CheckPathExists,
                DefaultExt = saveFileDialog.DefaultExt,
                FileName = saveFileDialog.FileName,
                Filter = saveFileDialog.Filter,
                InitialDirectory = saveFileDialog.InitialDirectory,
                Title = saveFileDialog.Title,
            };
        }


        /// <summary>
        /// Runs a common dialog box with the specified owner.
        /// </summary>
        /// <param name="owner">
        /// Any object that implements System.Windows.Forms.IWin32Window that represents the top-level
        /// window that will own the modal dialog box.
        /// </param>
        /// <returns>
        /// System.Windows.Forms.DialogResult.OK if the user clicks OK in the dialog box; otherwise,
        /// System.Windows.Forms.DialogResult.Cancel.
        /// </returns>
        public DialogResult ShowDialog(IWin32Window owner)
        {
            Contract.Requires(owner != null);

            DialogResult result = concreteSaveFileDialog.ShowDialog(owner);

            // Update ViewModel
            saveFileDialog.FileName = concreteSaveFileDialog.FileName;
            saveFileDialog.FileNames = concreteSaveFileDialog.FileNames;

            return result;
        }


        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        ~SaveFileDialog()
        {
            Dispose(false);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (concreteSaveFileDialog != null)
                {
                    concreteSaveFileDialog.Dispose();
                    concreteSaveFileDialog = null;
                }
            }
        }

        #endregion
    }
}
