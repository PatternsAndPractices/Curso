using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleFactory
{
    class Program
    {
        static void Main(string[] args)
        {
            // Metodo 1
            Console.WriteLine("Metodo 1");
            IVehiculeFactory[] creators = new IVehiculeFactory[3];

            creators[0] = new CarFactory();
            creators[1] = new MotorcycleFactory();
            creators[2] = new BusFactory();

            foreach (IVehiculeFactory creator in creators)
            {
                Vehicule vehicule = creator.Create();
                vehicule.Run();
            }

            Console.WriteLine();
            Console.ReadKey();


            // Metood 2
            Console.WriteLine("Metodo 1");
            foreach (VehiculeType type in Enum.GetValues(typeof(VehiculeType)))
            {
                Vehicule vehicule = VehiculeFactory.Create(type);
                vehicule.Run();

            }

            Console.ReadKey();


        }
    }
}
