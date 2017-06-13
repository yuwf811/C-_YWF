using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashAlgorithm
{
    public class SHAHash : IHash
    {
        public ulong Hash(string input)
        {
            using (var hash = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                var a = BitConverter.ToUInt64(bytes, 0);
                var b = BitConverter.ToUInt64(bytes, 8);
                ulong hashCode = a ^ b;
                return hashCode;
            }
        }
    }
}
