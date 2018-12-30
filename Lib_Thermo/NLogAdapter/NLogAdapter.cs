using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib_Thermo {
    public class NLogAdapter : ILogger
    {
        private Logger loger = LogManager.GetCurrentClassLogger();
        public void Log(LogEntry entry)
        {
            if (entry.Severity == LoggingEventType.Debug)
                loger.Debug(entry.Message);
            else if (entry.Severity == LoggingEventType.Information)
                loger.Info(entry.Message);
            else if (entry.Severity == LoggingEventType.Warning)
                loger.Warn(entry.Message);
            else if (entry.Severity == LoggingEventType.Error)
                loger.Error(entry.Exception, entry.Message);
            else
                loger.Fatal(entry.Exception, entry.Message);

        }
    }
}
