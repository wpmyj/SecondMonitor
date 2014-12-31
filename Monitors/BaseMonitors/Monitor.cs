using Monitors.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitors.BaseMonitors
{
    public abstract class Monitor
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string ProcessFile { get; set; }
        public string Desc { get; set; }
        public bool HasLog { get; set; }

        public Logger Logger { get; set; }

        public abstract bool IsAlive();

        public abstract bool Start(List<object> args);

        public abstract bool Stop();

    }

}
