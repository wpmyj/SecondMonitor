using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYMonitors.Loggers
{
    public abstract class Logger
    {
        public abstract List<Log> GetLogs();
        public abstract List<Log> GetLogs(int take, int skip);
    }

    public class Log
    {
        public DateTime DealDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int? Status { get; set; }
        public string Exception { get; set; }
    }
}
