using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Model.Domain
{
    public interface IReaderConvertable
    {
        void Convert(SqlDataReader reader);
    }
    public static class DomainUtil
    {
        static public T? GetValueOrNull<T>(SqlDataReader reader, string column) where T : struct
        {
            return reader[column] != DBNull.Value ? (T?)reader[column] : null;
        }
    }
}
