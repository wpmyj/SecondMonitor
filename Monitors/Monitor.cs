using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HYMonitors.MonitoredObjs;

namespace HYMonitors
{
    class Monitor
    {
        public string Name { get; set; }

        public MonitorStatus Status {
            get
            {
                var status = MonitorStatus.Running;
                foreach (var pair in MonitoredObjs)
                {
                    var type = pair.Value.GetType();
                    if (type == typeof (MonitoredService) ||
                        type == typeof (MonitoredWebSite))
                    {
                        var s = pair.Value.GetStatus();
                        if ((int) status < (int) s)
                        {
                            status = s;
                        }
                    }
                }
                return status;
            }
        }

        public Dictionary<string,BaseMonitoredObj> MonitoredObjs { get; set; }

        public Monitor()
        {
            this.MonitoredObjs = new Dictionary<string, BaseMonitoredObj>();
        }

    }
}
