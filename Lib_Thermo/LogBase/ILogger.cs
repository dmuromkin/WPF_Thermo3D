using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib_Thermo {
    public interface ILogger
    {
        void Log(LogEntry entry);
    }
}
