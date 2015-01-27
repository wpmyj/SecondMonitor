using System;
using System.Collections.Generic;
using System.Threading;
using HYMonitors.MonitoredObjs;

namespace HYMonitors
{
    /// <summary>
    /// 守护线程
    /// </summary>
    class WatchDog
    {
        private long isRunning;
        private object _lock = new object();
        private List<BaseMonitoredObj> monitoredObjs;
        private int interval;  //second

        internal event Action<string> Logger;

        internal WatchDog(List<BaseMonitoredObj> monitoredObjs, int interval = 5)
        {
            this.monitoredObjs = monitoredObjs;
            this.interval = interval;
        }

        private void WatchAllThreaded()
        {
            foreach (var monitoredObj in monitoredObjs)
            {
                new Thread(Loop).Start(monitoredObj);
            }
        }

        private void Loop(object o)
        {
            var monitoredObj = o as BaseMonitoredObj;
            while (Interlocked.Read(ref isRunning) == 1L)
            {
                if (monitoredObj.GetStatus() != MonitorStatus.Running)
                {
                    try
                    {
                        WriteLog(string.Format("WatchDoc监控到 {0} 非运行状态，尝试启动", monitoredObj.Name));
                        var ret = monitoredObj.Start(null);
                        if (ret)
                        {
                            WriteLog(string.Format("WatchDoc启动 {0} 成功", monitoredObj.Name));
                        }
                        else
                        {
                            WriteLog(string.Format("WatchDoc启动 {0} 失败", monitoredObj.Name));
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLog(string.Format("WatchDoc启动 {0} 出现异常：\r\n{1}", monitoredObj.Name, ex.ToString()));
                    }
                }
                for (int i = 0; i < interval; i++)
                {
                    if (Interlocked.Read(ref isRunning) == 0L)
                    {
                        return;
                    }
                    Thread.Sleep(1000);
                }
            }
        }

        public void Start()
        {
            if (Interlocked.Read(ref isRunning) == 1L)
            {
                return;
            }
            lock (_lock)
            {
                if (Interlocked.Read(ref isRunning) == 1L)
                {
                    return;
                }
                WatchAllThreaded();
                Interlocked.Exchange(ref isRunning, 1L);
            }
        }

        public void Stop()
        {
            Interlocked.Exchange(ref isRunning, 0L);
        }

        private void WriteLog(string msg)
        {
            if (this.Logger != null)
            {
                this.Logger(msg);
            }
        }
    }
}
