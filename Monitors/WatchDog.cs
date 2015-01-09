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
                if (monitoredObj.Status != MonitorStatus.Running)
                {
                    monitoredObj.Start(null);
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
    }
}
