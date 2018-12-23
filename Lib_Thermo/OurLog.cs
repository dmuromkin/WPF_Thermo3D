using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeploLibrary
{
    public class OurLog : ILogger
    {
        private string _LogFilePath = "CulcLog.txt";
        public void Log(LogEntry entry)
        {
            if (entry.Severity == LoggingEventType.Error || entry.Severity == LoggingEventType.Fatal)
            {
                string messages = DateTime.Now.ToString() + " | ОШИБКА! " + entry.Message + Environment.NewLine;
                File.AppendAllText(_LogFilePath, messages);
            }
            else {
                string messages = DateTime.Now.ToString() + " | " + entry.Message + Environment.NewLine;
                File.AppendAllText(_LogFilePath,messages);
            }
        }
    }
}
