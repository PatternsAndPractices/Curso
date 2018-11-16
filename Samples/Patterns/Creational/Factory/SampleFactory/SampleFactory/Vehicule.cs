using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleFactory
{
    public abstract class Vehicule
    {
        private int doors;
        private int whells;
        private int capacity;

        public int Doors { get => doors; set => doors = value; } 
        public int Wheells { get => whells; set=> whells = value; }
        public int Capacity { get => capacity; set => capacity = value; }

        public virtual void Run()
        {
            //Console.WriteLine($"soy {this.GetType().Name} tengo {whells} ruedas, {doors} puertas y una capacidad para {capacity} personas");
            Console.WriteLine($" y tengo ({whells}) ruedas, ({doors}) puertas y una capacidad para ({capacity}) personas");
        }

    }
}

