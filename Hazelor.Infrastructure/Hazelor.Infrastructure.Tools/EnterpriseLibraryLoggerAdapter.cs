using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.Prism.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hazelor.Infrastructure.Tools
{
    public class EnterpriseLibraryLoggerAdapter : ILoggerFacade
    {
        public EnterpriseLibraryLoggerAdapter()
        {
            Logger.SetLogWriter(new LogWriterFactory().Create());
        }

        #region ILoggerFacade Members

        public void Log(string message, Category category, Priority priority)
        {
            Logger.Write(message, category.ToString(), (int)priority);
        }

        #endregion
    }
}
