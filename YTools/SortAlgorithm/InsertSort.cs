using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortAlgorithm
{
    public class InsertSort : ISort
    {
        public void Sort<T>(T[] array) where T : IComparable
        {
            for (int i = 1; i < array.Length; i++)
            {
                T temp = array[i];
                int j;
                for (j = i - 1; j >= 0; j--)
                {
                    if (array[j].CompareTo(temp) <= 0)
                    {
                        break;
                    }
                    else
                    {
                        array[j + 1] = array[j];
                    }
                }
                array[j + 1] = temp;
            }
        }
    }
}
