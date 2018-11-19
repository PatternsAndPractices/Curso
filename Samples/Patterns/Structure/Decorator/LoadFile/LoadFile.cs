using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PnP.Patterns.Structure.Decorator
{
    public class LoadFile : ILoad
    {
        public string Load(string fileName)
        {

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
