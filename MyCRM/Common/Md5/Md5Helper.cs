using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common.Md5
{
    
    public class Md5Helper
    {
        private static StringBuilder sb = null;

        public static string GetMD5Str(string str)
        {
            sb = new StringBuilder(32);
            var bytes = Encoding.UTF8.GetBytes(str);
            MD5 md5 = MD5.Create();
            var result = md5.ComputeHash(bytes);
            foreach ( var item in result )
            {
                sb.Append(item.ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
