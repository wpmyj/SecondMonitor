using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYMonitors.MonitoredObjs
{
    class MonitoredRemoteIISApp : NestedMonitoredObj
    {
        public MonitoredRemoteIISApp(MonitoredIISPool container)
            : base(container)
        {
        }

        protected override MonitorStatus GetSelfStatus()
        {
            throw new NotImplementedException();
        }

        protected override bool StartSelf(List<object> args)
        {
            throw new NotImplementedException();
        }

        internal override bool Stop()
        {
            throw new NotImplementedException();
        }
    }
}
