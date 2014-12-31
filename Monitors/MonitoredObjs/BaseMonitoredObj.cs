using HYMonitors.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYMonitors.MonitoredObjs
{
    internal abstract class BaseMonitoredObj
    {
        internal string Name { get; set; }
        internal string Url { get; set; }
        internal string ProcessFile { get; set; }
        internal string Desc { get; set; }
        internal bool HasLog { get; set; }

        public Logger Logger { get; set; }

        public abstract MonitorStatus GetStatus();

        public abstract bool Start(List<object> args);

        public abstract bool Stop();

    }

}
