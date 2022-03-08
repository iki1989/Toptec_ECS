using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Core.Util
{
    public static class ObjectExtension
    {
        public static T Clone<T>(this T src)
        {
            if (src is ICloneable)
            {
                return (T)(src as ICloneable).Clone();
            }

            using (var stream = new MemoryStream())
            {
                DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(T));

                dataContractSerializer.WriteObject(stream, src);
                stream.Position = 0;
                return (T)dataContractSerializer.ReadObject(stream);
            }
        }
    }
}
