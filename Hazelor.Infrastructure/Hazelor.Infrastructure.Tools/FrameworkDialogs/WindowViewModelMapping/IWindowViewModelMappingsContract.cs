using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hazelor.Infrastructure.FrameworkDialogs.WindowViewModelMapping
{
    [ContractClassFor(typeof(IWindowViewModelMappings))]
    abstract class IWindowViewModelMappingsContract : IWindowViewModelMappings
    {
        /// <summary>
        /// Gets the window type based on registered ViewModel type.
        /// </summary>
        /// <param name="viewModelType">The type of the ViewModel.</param>
        /// <returns>
        /// The window type based on registered ViewModel type.
        /// </returns>
        public Type GetWindowTypeFromViewModelType(Type viewModelType)
        {
            Contract.Ensures(Contract.Result<Type>().IsSubclassOf(typeof(Window)));

            return default(Type);
        }
    }
}
