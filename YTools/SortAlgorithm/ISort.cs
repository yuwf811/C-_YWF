using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortAlgorithm
{
    public interface ISort
    {
        void Sort<T>(T[] array) where T : IComparable;
    }
}
