﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

///
/// 避免IIS和IISPool的Name不正常，没采用装饰者模式
/// 
namespace HYMonitors.MonitoredObjs
{
    class MonitoredIIS : BaseMonitoredObj
    {
        public MonitoredIIS()
        {
            this.Name = "IIS";
        }

        public override MonitorStatus GetStatus()
        {
            throw new NotImplementedException();
        }

        public override bool Start(List<object> args)
        {
            throw new NotImplementedException();
        }

        public override bool Stop()
        {
            throw new NotImplementedException();
        }
    }


    class MonitoredIISPool : NestedMonitoredObj
    {
        public MonitoredIISPool(MonitoredIIS container)
            : base(container)
        {
        }
        protected override MonitorStatus GetSelfStatus()
        {
            throw new NotImplementedException();
        }

        protected override bool StartSelf(List<object> args)
        {
            throw new NotImplementedException();
        }

        public override bool Stop()
        {
            throw new NotImplementedException();
        }
    }


    class MonitoredIISApp : NestedMonitoredObj
    {

        public MonitoredIISApp(MonitoredIISPool container)
            : base(container)
        {

        }

        protected override MonitorStatus GetSelfStatus()
        {
            throw new NotImplementedException();
        }

        protected override bool StartSelf(List<object> args)
        {
            throw new NotImplementedException();
        }

        public override bool Stop()
        {
            throw new NotImplementedException();
        }
    }
}