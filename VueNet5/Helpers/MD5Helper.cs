using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VueNet5.Helpers
{
    public static class MD5Extensions
    {
        public static string ToMD5(this string str)
        {
            using(var cryptoMD5 = System.Security.Cryptography.MD5.Create())
            {
                //將字串編碼成 UTF8 位元組陣列
                var bytes = Encoding.UTF8.GetBytes(str);

                //取得雜湊值位元組陣列
                var hash = cryptoMD5.ComputeHash(bytes);

                //取得 MD5
                var md5 = BitConverter.ToString(hash)
                  .Replace("-",String.Empty)
                  .ToUpper();

                return md5;
            }
        }
    }
}