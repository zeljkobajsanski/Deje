using System.Security.Cryptography;
using System.Text;

namespace Deje.CustomerWebApp.Utils
{
    public static class HashFunction
    {
        public static byte[] ComputeHash(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.Unicode.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            return hash;
        } 
    }
}