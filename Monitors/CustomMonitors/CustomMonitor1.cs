using Monitors.BaseMonitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecondMonitor.Monitors
{
    public class CustomMonitor1 : CustomMonitor
    {
        public override bool IsAlive()
        {
            throw new NotImplementedException();
        }

        public override bool Start(List<object> args)
        {
            throw new NotImplementedException();
        }

        public override bool Stop()
        {
            throw new NotImplementedException();
        }
    }
}