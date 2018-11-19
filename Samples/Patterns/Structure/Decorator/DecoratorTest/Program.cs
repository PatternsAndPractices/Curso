using PnP.Patterns.Structure.Decorator;
using System;

namespace DecoratorTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string fileName = @"..\..\..\Files\Messages.csv";

            // Llamar al componente de forma Normal
            LoadFile loadFile = new LoadFile();
            string dataFile = loadFile.Load(fileName);
            Console.WriteLine(dataFile);

            Console.WriteLine("Presione una tecla para continuar");
            Console.ReadLine();

            // Deocrar el Componente Principal y reportar en un Archivo de Log
            string logFileName = @"..\..\..\Files\LogFile.Log";
            LogFile logFile = new LogFile(loadFile, logFileName);
            string resultData = logFile.Load(fileName);
            Console.WriteLine(resultData);

            Console.WriteLine("Presione una tecla para continuar");
            Console.ReadLine();

            // Decorar el Componente Principal y reportar en el Log de windows
            LogWin logWin = new LogWin(loadFile);
            string resultString = logWin.Load(fileName);
            Console.WriteLine(resultString);

            Console.WriteLine("Presione una tecla para continuar");
            Console.ReadLine();

            // Decorar el compoentne secundario y reportar enel log de Archivos y en el Log de Windows
            LogWin logFileWin = new LogWin(logFile);
            string result = logFileWin.Load(fileName);
            Console.WriteLine(resultString);

            Console.WriteLine("Presione una tecla para continuar");
            Console.ReadLine();


        }
    }
}