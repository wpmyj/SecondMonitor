using HYMonitors.MonitoredObjs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HYMonitors.CustomMonitors
{
    class CustomMonitoredObj1 : CustomMonitoredObj
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