using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YIOC;

namespace IOCTest
{
    class Program
    {
        static void Main(string[] args)
        {
            YResolver resolver = new YResolver("autofac.json");
            IAnimal animal = resolver.Resolve<IAnimal>();
            animal.Eat();
            Console.ReadLine();
        }
    }
}
