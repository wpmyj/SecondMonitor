using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HYMonitors.MonitoredObjs;

namespace HYMonitors
{
    /// <summary>
    /// 监视器：一个完整的项目，如HBus
    /// 包含多个被监视的进程、服务、网站等
    /// </summary>
    public class Monitor
    {
        public string Name { get; set; }

        /// <summary>
        /// 计划任务不是实时监控
        /// </summary>
        public MonitorStatus Status
        {
            get
            {
                var status = MonitorStatus.Running;
                foreach (var pair in MonitoredObjs)
                {
                    var type = pair.Value.GetType();
                    if (type != typeof(MonitoredSchTaskBat))
                    {
                        var s = pair.Value.Status;
                        if ((int)status < (int)s)
                        {
                            status = s;
                        }
                    }
                }
                return status;
            }
        }

        public Dictionary<string, BaseMonitoredObj> MonitoredObjs { get; set; }

        public Monitor()
        {
            this.MonitoredObjs = new Dictionary<string, BaseMonitoredObj>();
        }

    }
}
