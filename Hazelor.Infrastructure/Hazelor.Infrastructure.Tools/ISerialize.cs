using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hazelor.Infrastructure.Tools
{
    public interface ISerialize
    {
        void Serialize(ISerializeModel model, Type t);
        ISerializeModel DeSerialize(string filePath, Type t);
    }
}
