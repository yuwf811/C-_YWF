using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace YOrm.Hash
{
    public class MD5Computer : IHashComputer
    {
        public MD5 JavaScriptSerializer { get; private set; }

        public string ComputeHash<T>(T obj)
        {
            MD5 calculator = MD5.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));
            return Encoding.UTF8.GetString(calculator.ComputeHash(bytes));
        }
    }
}
