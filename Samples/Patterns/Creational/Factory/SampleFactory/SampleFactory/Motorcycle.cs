using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleFactory
{
    public class Motorcycle: Vehicule
    {
        public Motorcycle()
        {
            base.Doors = 0;
            base.Wheells = 2;
            base.Capacity = 2;
        }

        public override void Run()
        {
            Console.Write("Soy una Motocicleta");
            base.Run();
        }
    }
}
