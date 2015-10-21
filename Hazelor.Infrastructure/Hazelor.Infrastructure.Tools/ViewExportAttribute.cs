using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hazelor.Infrastructure.Tools
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    [MetadataAttribute]
    public sealed class ViewExportAttribute : ExportAttribute, IViewRegionRegistration
    {
        public ViewExportAttribute()
            : base(typeof(object))
        { }

        public ViewExportAttribute(string viewName)
            : base(viewName, typeof(object))
        { }

        public string ViewName { get { return base.ContractName; } }

        public string RegionName { get; set; }
    }
}
