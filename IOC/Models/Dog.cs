using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Dog : IAnimal
    {
        public void Eat()
        {
            Console.WriteLine("Dog eat.");
        }

        public void Run()
        {
            Console.WriteLine("Dog run.");
        }
    }
}
