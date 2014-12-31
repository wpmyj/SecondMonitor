using Monitors.BaseMonitors;
using Monitors.Loggers;
using System;
using System.Collections.Generic;

namespace SecondMonitor.Monitors
{
    public class MonitorService
    {

        private MonitorService instance;
        private object _lock =new object();

        private Dictionary<string,Monitor> monitors;

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
            monitors = new Dictionary<string, Monitor>();
            BuildMonitors();
        }

        private void BuildMonitors()
        {
            BuildConfigMonitors();
            BuildCustomMonitors();
        }

        /// <summary>
        /// 根据xml配置生成监视器
        /// </summary>
        private void BuildConfigMonitors()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 生成自定义监视器
        /// </summary>
        private void BuildCustomMonitors()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>json格式</returns>
        public string GetMonitorInfos()
        {
            throw new NotImplementedException();
        }

        public bool StartMonitor(string monitorName,List<Object> args)
        {
            return this.monitors[monitorName].Start(args);
        }

        public bool StopMonitor(string monitorName)
        {
            return this.monitors[monitorName].Stop();
        }

        public List<Log> GetMonitorLogs(string monitorName,int take, int skip)
        {
            var monitor = this.monitors[monitorName];
            if (monitor.HasLog)
            {
                return monitor.Logger.GetLogs(take, skip);
            }
            else
            {
                throw new NotImplementedException();
            }
        } 
    }
}