using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Monitors.BaseMonitors
{
    public class SchTaskMonitor : Monitor
    {
        private long isRunning;

        public SchTaskMonitor() : base()
        {
            isRunning = 0L;
        }

        public override bool IsAlive()
        {
            throw new NotImplementedException();
        }

        public override bool Start(List<object> args)
        {
            if (Interlocked.Read(ref isRunning) == 1L)
            {
                throw new Exception(string.Format("计划任务 {0} 正在运行", this.Name));
            }
            Interlocked.Exchange(ref isRunning, 1L);
            
            new Thread(StartWithThread).Start(args);
            
            return true;
        }

        private void StartWithThread(object o)
        {
            var args = (List<object>) o;

            //process
            //

            Interlocked.Exchange(ref isRunning, 0L);
        }

        public override bool Stop()
        {
            throw new NotImplementedException();
        }
    }
}
