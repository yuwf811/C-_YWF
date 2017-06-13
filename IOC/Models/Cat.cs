using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Cat : IAnimal
    {
        public void Eat()
        {
            Console.WriteLine("Cat eat.");
        }

        public void Run()
        {
            Console.WriteLine("Cat run.");
        }
    }
}
