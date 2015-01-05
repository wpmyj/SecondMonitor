using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HYMonitors.MonitoredObjs
{
    class MonitoredSchTask : BaseMonitoredObj
    {
        private long isRunning;

        public MonitoredSchTask() : base()
        {
            isRunning = 0L;
        }

        public override MonitorStatus GetStatus()
        {
            if (Interlocked.Read(ref isRunning) == 1L)
            {
                return MonitorStatus.Running;
            }
            else
            {
                return MonitorStatus.Stop;
            }
        }

        public override bool Start(List<object> args)
        {
            if (Interlocked.Read(ref isRunning) == 1L)
            {
                throw new Exception(string.Format("计划任务 {0} 正在运行", this.Name));
            }
            Interlocked.Exchange(ref isRunning, 1L);
            
            new Thread(RunTaskProcess).Start(args);
            
            return true;
        }

        /// <summary>
        /// 计划任务耗时未知，所以在新线程中启动计划任务，
        /// </summary>
        /// <param name="o"></param>
        private void RunTaskProcess(object o)
        {
            var args = (List<object>) o;

            //process
            try
            {

            }
            catch (Exception)
            {
                
            }
            Interlocked.Exchange(ref isRunning, 0L);
        }

        public override bool Stop()
        {
            throw new NotImplementedException();
        }
    }
}
