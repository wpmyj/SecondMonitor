using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace HYMonitors.MonitoredObjs
{
    /// <summary>
    /// 用schtasks命令和系统互交
    /// </summary>
    class MonitoredSchTask : BaseMonitoredObj
    {
        private long isRunning;
        private object _lock = new object();

        public MonitoredSchTask()
            : base()
        {
            isRunning = 0L;
            this.Watched = false;
        }

        protected override MonitorStatus GetStatus()
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

        internal override bool Start(List<object> args)
        {
            if (Interlocked.Read(ref isRunning) == 1L)
            {
                throw new Exception(string.Format("计划任务 {0} 正在运行", this.Name));
            }
            lock (_lock)
            {
                if (Interlocked.Read(ref isRunning) == 1L)
                {
                    throw new Exception(string.Format("计划任务 {0} 正在运行", this.Name));
                }
                new Thread(RunTaskProcess).Start(args);
                Interlocked.Exchange(ref isRunning, 1L);
                return true;
            }
        }

        /// <summary>
        /// 计划任务耗时未知，所以在新线程中启动计划任务，
        /// </summary>
        /// <param name="o"></param>
        private void RunTaskProcess(object o)
        {
            var args = (List<object>)o;
            var command = string.Empty;
            if (args != null)
            {
                command = string.Format("{0} {1}", this.ProcessFile, string.Join(" ", args));
            }
            else
            {
                command = this.ProcessFile;
            }
            //process
            try
            {
                var pro = new Process();
                pro.StartInfo.FileName = "cmd.exe";
                pro.StartInfo.Arguments = " /c " + command;
                pro.StartInfo.UseShellExecute = true;
                pro.StartInfo.CreateNoWindow = false; //不显示windows窗口
                pro.Start();
                pro.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Interlocked.Exchange(ref isRunning, 0L);
        }

        internal override bool Stop()
        {
            throw new NotImplementedException();
        }
    }
}
