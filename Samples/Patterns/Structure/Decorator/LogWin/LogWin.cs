using System;
using System.Diagnostics;

namespace PnP.Patterns.Structure.Decorator
{
    public class LogWin : Traceable
    {
        private static readonly string SourceName = "LogWin";

        public LogWin(ILoad component) : base(component)
        {
            EventLog evt = new EventLog();

            // NOTA: PARA QUE ESTA INSTRUCCION FUNCIONES SE DEBE
            // EJECUTAR VISUAL STUDIO EN MODO ADMINISTRADOR
            if (!EventLog.SourceExists(SourceName))
            {
                EventLog.CreateEventSource(SourceName, "Application");
            }

            evt.Close();
        }

        public override string Load(string fileName)
        {
            Console.WriteLine("Ejecutando Decorador: LogWin");

            EventLog evt = new EventLog();
            evt.Source = SourceName;

            evt.WriteEntry("Start Load", EventLogEntryType.Information);

            string dataTile = base.Load(fileName);

            evt.WriteEntry("Finish Load", EventLogEntryType.Information);

            return dataTile;
        }
    }
}