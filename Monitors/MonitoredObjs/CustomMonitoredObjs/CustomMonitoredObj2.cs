using HYMonitors.MonitoredObjs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYMonitors.CustomMonitors
{
    class CustomMonitoredObj2 : CustomMonitoredObj
    {
        protected override MonitorStatus GetStatus()
        {
            throw new NotImplementedException();
        }

        internal override bool Start(List<object> args)
        {
            throw new NotImplementedException();
        }

        internal override bool Stop()
        {
            throw new NotImplementedException();
        }
    }
}
