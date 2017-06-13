using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashAlgorithm
{
    public interface IHash
    {
        ulong Hash(string input);
    }
}
