using System.Threading;
using HYMonitors.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYMonitors.MonitoredObjs
{
    public abstract class BaseMonitoredObj
    {
        private MonitorStatus? currentStatus;
        private ReaderWriterLock statusLock;
        private DateTime statusUpdateTime;
        protected TimeSpan timeout;

        public virtual string Name { get; set; }
        public string Url { get; set; }
        
        public string Desc { get; set; }
        public bool HasLog { get; set; }
        public bool Watched { get; set; }
        public bool Remote { get; set; }

        /// <summary>
        /// 程序集外访问缓存的状态
        /// </summary>
        public MonitorStatus Status
        {
            get
            {
                MonitorStatus status;
                try
                {
                    statusLock.AcquireReaderLock(timeout);
                    if (currentStatus == null ||
                        statusUpdateTime == null ||
                        DateTime.Now - statusUpdateTime > timeout)
                    {
                        var lc = statusLock.UpgradeToWriterLock(timeout);
                        currentStatus = GetStatus();
                        statusUpdateTime = DateTime.Now;
                        statusLock.DowngradeFromWriterLock(ref lc);
                    }
                    status = currentStatus.Value;
                }
                finally
                {
                    statusLock.ReleaseReaderLock();
                }
                return status;
            }
        }

        public BaseMonitoredObj()
        {
            statusLock = new ReaderWriterLock();
            timeout = new TimeSpan(0, 0, MonitorBuilder.WatchInterval);  //
        }

        public Logger Logger { get; set; }

        /// <summary>
        /// 程序集内部可以实时访问状态
        /// </summary>
        /// <returns></returns>
        internal abstract MonitorStatus GetStatus();

        internal abstract bool Start(List<object> args);

        internal abstract bool Stop();

    }

    public abstract class NestedMonitoredObj : BaseMonitoredObj
    {
        protected BaseMonitoredObj container;

        public NestedMonitoredObj(BaseMonitoredObj container)
        {
            this.container = container;
        }

        internal override MonitorStatus GetStatus()
        {
            if (container.Status != MonitorStatus.Running)
            {
                return container.Status;
            }
            else
            {
                return GetSelfStatus();
            }
        }

        protected abstract MonitorStatus GetSelfStatus();

        internal override bool Start(List<object> args)
        {
            if (StartContainer(args))
            {
                return StartSelf(args);
            }
            else
            {
                return false;
            }
        }

        private bool StartContainer(List<object> args)
        {
            if (container.Status != MonitorStatus.Running)
            {
                return container.Start(args);
            }
            else
            {
                return true;
            }
        }

        protected abstract bool StartSelf(List<object> args);

    }
}
