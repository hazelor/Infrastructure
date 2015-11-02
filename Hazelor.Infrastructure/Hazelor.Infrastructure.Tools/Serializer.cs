using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Hazelor.Infrastructure.Tools
{
    public class Serializer : ISerialize
    {
        private FileStream fs;
        public void Serialize(ISerializeModel model, Type t)
        {
            using (fs = new FileStream(model.FilePath, FileMode.Create))
            {
                //BinaryFormatter formate = new BinaryFormatter();
                XmlSerializer formate = new XmlSerializer(t);
                formate.Serialize(fs, model);
            }
        }
        public ISerializeModel DeSerialize(string filePath, Type t)
        {
            using (fs = new FileStream(filePath, FileMode.Open))
            {
                XmlSerializer formate = new XmlSerializer(t);
                return (ISerializeModel)formate.Deserialize(fs);
            }
        }
    }
}
