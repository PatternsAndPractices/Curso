using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleFactory
{
    public class CarFactory : IVehiculeFactory
    {
        public Vehicule Create()
        {
            return new Car();
        }
    }
}
