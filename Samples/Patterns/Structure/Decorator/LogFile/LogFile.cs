﻿using System;
using System.IO;

namespace PnP.Patterns.Structure.Decorator
{
    /// <summary>
    /// Descripcion del objetivo de la clase
    /// </summary>
    public class LogFile : Traceable
    {
        private string logFileName;

        public LogFile(ILoad component, string logFileName) : base(component)
        {
            this.logFileName = logFileName;
        }

        public override string Load(string fileName)
        {
            StreamWriter file = new StreamWriter(logFileName);

            file.WriteLine("{0}, Start Load", DateTime.Now.ToString("yyyy-MMM-dd HH:mm:ss.fffffff"));

            string dataTile = base.Load(fileName);

            file.WriteLine("{0}, Finish Load", DateTime.Now.ToString("yyyy-MMM-dd HH:mm:ss.fffffff"));

            file.Close();

            return dataTile;
        }
    }
}