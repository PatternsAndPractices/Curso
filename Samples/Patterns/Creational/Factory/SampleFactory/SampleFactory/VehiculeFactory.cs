using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleFactory
{
    public enum VehiculeType
    {
        Car,
        MotorCycle,
        Bus
    };

    public class VehiculeFactory
    {
        public static Vehicule Create(VehiculeType type)
        {
            Vehicule vehicule ;
            switch (type)
            {
                case VehiculeType.Car:
                    vehicule = new Car();
                    break;
                case VehiculeType.MotorCycle:
                    vehicule = new Motorcycle();
                    break;
                case VehiculeType.Bus:
                    vehicule = new Bus();
                    break;
                default:
                    vehicule = null;
                    break;
            }
            return vehicule;
        }
    }
}
