using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleFactory
{
    public class BusFactory : IVehiculeFactory
    {
        public Vehicule Create()
        {
            return new Bus();
        }
    }
}
