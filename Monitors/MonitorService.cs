﻿using System.Linq;
using System.Runtime.InteropServices;
using HYMonitors.MonitoredObjs;
using HYMonitors.Loggers;
using System;
using System.Collections.Generic;

namespace HYMonitors
{
    public delegate void WriteLog(string msg);

    /// <summary>
    /// 
    /// </summary>
    public class MonitorService
    {

        private MonitorService instance;
        private object _lock = new object();
        private WatchDog watchDog;
        private Dictionary<string, Monitor> monitors;
        public event WriteLog Logger;

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
            if (MonitorBuilder.WatchDog)
            {
                var watchedMonitoredObjs = new List<BaseMonitoredObj>();
                monitors.Values.ToList().ForEach(x => watchedMonitoredObjs.AddRange(x.MonitoredObjs.Values.ToList().FindAll(y => y.Watched)));
                watchDog = new WatchDog(watchedMonitoredObjs, MonitorBuilder.WatchInterval);
            }
        }

        public void Start()
        {
            if (watchDog != null)
            {
                watchDog.Start();
            }
        }

        public void Stop()
        {
            if (watchDog != null)
            {
                watchDog.Stop();
            }
        }

        public List<Monitor> GetMonitorInfos()
        {
            return monitors.Values.ToList();
        }

        public bool StartMonitor(string monitorName, string monitoredObjName, List<Object> args)
        {
            try
            {
                return this.monitors[monitorName].MonitoredObjs[monitoredObjName].Start(args);
            }
            catch (Exception ex)
            {
                Logger(ex.ToString());
                throw ex;
            }
        }

        public bool StopMonitor(string monitorName, string monitoredObjName)
        {
            try
            {
                return this.monitors[monitorName].MonitoredObjs[monitoredObjName].Stop();
            }
            catch (Exception ex)
            {
                Logger(ex.ToString());
                throw ex;
            }
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