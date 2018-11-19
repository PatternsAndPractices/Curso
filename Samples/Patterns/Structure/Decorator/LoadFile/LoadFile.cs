using System;
using System.IO;
using System.Threading;

namespace PnP.Patterns.Structure.Decorator
{
    public class LoadFile : ILoad
    {
        public string Load(string fileName)
        {
            Console.WriteLine("Ejecutando Componente Real: LoadFile");

            // Start
            string result = string.Empty;

            // Similar demora en la Carga del Archivo
            Random randomTime = new Random();
            int secondsTime = randomTime.Next(1, 10) * 1000;
            Thread.Sleep(secondsTime);
            // Cargar el Archivo
            result = File.ReadAllText(fileName);

            // Finish
            return result;
        }
    }
}