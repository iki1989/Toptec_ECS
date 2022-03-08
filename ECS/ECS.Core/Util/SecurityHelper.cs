using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Core.Util
{
    public class SecurityHelper
    {
        public static string MD5_Encrypt(string inputStr)
        {
            MD5 mD5 = MD5.Create();
            byte[] numArray = mD5.ComputeHash(Encoding.UTF8.GetBytes(inputStr));
            return Convert.ToBase64String(numArray).Replace("\r\n", "");
        }
    }
}
