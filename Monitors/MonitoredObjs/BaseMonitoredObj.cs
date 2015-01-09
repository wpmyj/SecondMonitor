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
        private TimeSpan timeout;

        public virtual string Name { get; set; }
        public string Url { get; set; }
        public string ProcessFile { get; set; }
        public string Desc { get; set; }
        public bool HasLog { get; set; }
        public bool Watched { get; set; }
        public bool Remote { get; set; }
        public MonitorStatus Status
        {
            get
            {
                MonitorStatus? _status;
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
                    _status = currentStatus;
                }
                finally
                {
                    statusLock.ReleaseReaderLock();
                }
                return _status.Value;
            }
        }

        public BaseMonitoredObj()
        {
            statusLock = new ReaderWriterLock();
            timeout = new TimeSpan(0, 0, 5);  //
        }

        public Logger Logger { get; set; }

        protected abstract MonitorStatus GetStatus();

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

        protected override MonitorStatus GetStatus()
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
