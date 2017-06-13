using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisTool
{
    public static class Extensions
    {
        public static string[] ToStringArray(this byte[][] bytes)
        {
            if (bytes == null)
                return null;
            string[] result = new string[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
            {
                result[i] = bytes[i] == null ? null : Encoding.UTF8.GetString(bytes[i]);
            }
            return result;
        }

        public static byte[][] To2DByteArray(this IEnumerable<string> strArray)
        {
            if (strArray == null)
                return null;
            byte[][] result = new byte[strArray.Count()][];

            int index = 0;
            foreach (var item in strArray)
            {
                result[index++] = item == null ? null : Encoding.UTF8.GetBytes(item);
            }
            return result;
        }
    }
}
