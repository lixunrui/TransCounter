using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace LXR.Counter
{
    internal static class Encryption
    {
        internal static String Encrypt(String source)
        {
            MD5 md5Hasher = MD5.Create();

            byte[] data = md5Hasher.ComputeHash(Encoding.ASCII.GetBytes(source));

            return data.ToString();
        }
    }
}
