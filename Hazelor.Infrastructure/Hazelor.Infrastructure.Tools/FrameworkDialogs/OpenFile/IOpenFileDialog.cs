﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hazelor.Infrastructure.FrameworkDialogs.OpenFile
{
    /// <summary>
    /// Interface describing the OpenFileDialog.
    /// </summary>
    public interface IOpenFileDialog : IFileDialog
    {
        /// <summary>
        /// Gets or sets a value indicating whether the dialog box allows multiple files to be
        /// selected.
        /// </summary>
        bool Multiselect { get; set; }
    }
}
