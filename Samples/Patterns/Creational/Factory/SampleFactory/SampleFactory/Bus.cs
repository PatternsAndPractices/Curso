using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleFactory
{
    public class Bus : Vehicule
    {
        public Bus()
        {
            base.Doors = 2;
            base.Wheells = 4;
            base.Capacity = 30;
        }

        public override void Run()
        {
            Console.Write("Soy un Bus");
            base.Run();
        }
    }
}
