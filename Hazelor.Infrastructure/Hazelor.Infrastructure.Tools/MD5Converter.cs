using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hazelor.Infrastructure.Tools
{
    public static class MD5Converter
    {
        public static string GetMD5(string originStr)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.Unicode.GetBytes(originStr);
            byte[] targetData = md5.ComputeHash(fromData);
            string resStr = "";
            for (int i = 0; i < targetData.Length; i++)
            {
                resStr += targetData[i].ToString("X");
            }
            return resStr;
        }

    }
}
