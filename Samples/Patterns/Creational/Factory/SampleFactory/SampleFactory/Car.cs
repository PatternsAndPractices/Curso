using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleFactory
{
    public class Car : Vehicule
    {
        public Car()
        {
            base.Doors = 5;
            base.Wheells = 4;
            base.Capacity = 5;
        }

        public override void Run()
        {
            Console.Write("Soy un Carro");
            base.Run();
        }
    }
}
