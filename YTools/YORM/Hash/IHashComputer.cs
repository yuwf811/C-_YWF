using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YOrm.Hash
{
    public interface IHashComputer
    {
        string ComputeHash<T>(T obj);
    }
}
