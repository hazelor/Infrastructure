﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hazelor.Infrastructure.FrameworkDialogs.FolderBrowse
{
    public interface IFolderBrowserDialog
    {
        /// <summary>
        /// Gets or sets the descriptive text displayed above the tree view control in the dialog box.
        /// </summary>
        string Description { get; set; }


        /// <summary>
        /// Gets or sets the path selected by the user.
        /// </summary>
        string SelectedPath { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether the New Folder button appears in the folder browser
        /// dialog box.
        /// </summary>
        bool ShowNewFolderButton { get; set; }
    }
}
