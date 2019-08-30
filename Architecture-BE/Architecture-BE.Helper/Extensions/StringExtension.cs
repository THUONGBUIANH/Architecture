using System;
using System.Security.Cryptography;
using System.Text;

namespace Architecture_BE.Helper.Extensions
{
    public static class StringExtension
    {
        public static string MD5Hash(this string input)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.ASCII.GetBytes(input));
                var strResult = BitConverter.ToString(result);
                return strResult.Replace("-", "");
            }
        }
    }
}
