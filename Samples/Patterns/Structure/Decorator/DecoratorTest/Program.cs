using PnP.Patterns.Structure.Decorator;
using System;

namespace DecoratorTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string fileName = @"..\..\..\Files\Messages.xml";

            LoadFile loadFile = new LoadFile();
            string dataFile = loadFile.Load(fileName);

            Console.WriteLine(dataFile);
            Console.ReadLine();

            string logFileName = @"..\..\..\Files\LogFile.Log";

            LogFile logFile = new LogFile(loadFile, logFileName);
            string resultData = logFile.Load(fileName);
            Console.WriteLine(resultData);

            Console.ReadLine();
        }
    }
}