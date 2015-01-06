using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

        internal WatchDog(List<BaseMonitoredObj> monitoredObjs)
        {
            this.monitoredObjs = monitoredObjs;
        }

        private void Loop()
        {
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
                new Thread(Loop).Start();
                Interlocked.Exchange(ref isRunning, 1L);
            }

        }

        public void Stop()
        {
            Interlocked.Exchange(ref isRunning, 0L);
        }
    }
}
