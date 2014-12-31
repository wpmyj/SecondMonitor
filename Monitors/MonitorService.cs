using HYMonitors.MonitoredObjs;
using HYMonitors.Loggers;
using System;
using System.Collections.Generic;

namespace HYMonitors
{
    public class MonitorService
    {

        private MonitorService instance;
        private object _lock = new object();

        private Dictionary<string, Monitor> monitors;

        public MonitorService Instance
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new MonitorService();
                    }
                    return instance;
                }
            }
        }

        private MonitorService()
        {
            monitors = MonitorBuilder.Build();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>json格式</returns>
        public string GetMonitorInfos()
        {
            throw new NotImplementedException();
        }

        public bool StartMonitor(string monitorName, string monitoredObjName, List<Object> args)
        {
            return this.monitors[monitorName].MonitoredObjs[monitoredObjName].Start(args);
        }

        public bool StopMonitor(string monitorName, string monitoredObjName)
        {
            return this.monitors[monitorName].MonitoredObjs[monitoredObjName].Stop();
        }

        public List<Log> GetMonitorLogs(string monitorName, string monitoredObjName, int take, int skip)
        {
            var monitor = this.monitors[monitorName].MonitoredObjs[monitoredObjName];
            if (monitor.HasLog)
            {
                return monitor.Logger.GetLogs(take, skip);
            }
            else
            {
                throw new Exception(string.Format("{0} 没实现日志服务", monitoredObjName));
            }
        }
    }
}