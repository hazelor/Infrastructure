using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hazelor.Infrastructure.FrameworkDialogs.WindowViewModelMapping
{
    [ContractClass(typeof(IWindowViewModelMappingsContract))]
    public interface IWindowViewModelMappings
    {
        /// <summary>
        /// Gets the window type based on registered ViewModel type.
        /// </summary>
        /// <param name="viewModelType">The type of the ViewModel.</param>
        /// <returns>The window type based on registered ViewModel type.</returns>
        Type GetWindowTypeFromViewModelType(Type viewModelType);
    }
}
