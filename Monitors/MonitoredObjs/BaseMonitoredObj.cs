using HYMonitors.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYMonitors.MonitoredObjs
{
    internal abstract class BaseMonitoredObj
    {
        internal string Name { get; set; }
        internal string Url { get; set; }
        internal string ProcessFile { get; set; }
        internal string Desc { get; set; }
        internal bool HasLog { get; set; }
        internal bool Watched { get; set; }
        internal bool Remote { get; set; }

        public Logger Logger { get; set; }

        public abstract MonitorStatus GetStatus();

        public abstract bool Start(List<object> args);

        public abstract bool Stop();

    }

    internal abstract class NestedMonitoredObj:BaseMonitoredObj
    {
        protected BaseMonitoredObj container;

        public NestedMonitoredObj(BaseMonitoredObj container)
        {
            this.container = container;
        }

        public override MonitorStatus GetStatus()
        {
            if (container.GetStatus() != MonitorStatus.Running)
            {
                return container.GetStatus();
            }
            else
            {
                return GetSelfStatus();
            }
        }

        protected abstract MonitorStatus GetSelfStatus();

        public override bool Start(List<object> args)
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
            if (container.GetStatus() != MonitorStatus.Running)
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
