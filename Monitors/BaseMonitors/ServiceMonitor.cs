using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitors.BaseMonitors
{
    class ServiceMonitor: Monitor
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
