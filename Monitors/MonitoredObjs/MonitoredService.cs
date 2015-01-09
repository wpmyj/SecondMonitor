using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;


/*
 * 先连接ipc服务
 * C:\Users\Administrator>net use \\192.168.0.157\ipc$ * /user:administrator
 * 请键入 \\192.168.0.157\ipc$ 的密码:
 * 命令成功完成。

 * 再在服务中连接另外此计算机
 */
namespace HYMonitors.MonitoredObjs
{
    /// <summary>
    /// 权限问题：允许NETWORK SERVICE来启动service
    /// </summary>
    class MonitoredService : BaseMonitoredObj
    {
        private object _lock = new object();
        private ServiceController service;

        public override string Name {
            get { return base.Name; }
            set
            {
                base.Name = value;
                service.ServiceName = value;
            } 
        }

        public string MachineName
        {
            set
            {
                //service.MachineName = Environment.MachineName;
                service.MachineName = value;
            }
        }

        public MonitoredService()
            : base()
        {
            service = new ServiceController();
        }

        internal override MonitorStatus GetStatus()
        {
            var serviceStatus = service.Status;
            MonitorStatus status = MonitorStatus.Stop;

            switch (serviceStatus)
            {
                case ServiceControllerStatus.Running:
                    status = MonitorStatus.Running;
                    break;
                case ServiceControllerStatus.StartPending:
                    status = MonitorStatus.Starting;
                    break;
                case ServiceControllerStatus.StopPending:
                    status = MonitorStatus.Stopping;
                    break;
            }
            return status;
        }

        internal override bool Start(List<object> args)
        {
            if (service.Status == ServiceControllerStatus.Running)
            {
                return true;
            }
            lock (_lock)
            {
                if (service.Status == ServiceControllerStatus.Running)
                {
                    return true;
                }
                service.Start();
                try
                {
                    service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                    return true;
                }
                catch (System.ServiceProcess.TimeoutException ex)
                {
                    throw ex;
                }
            }
        }

        internal override bool Stop()
        {
            if (service.Status == ServiceControllerStatus.Stopped)
            {
                return true;
            }
            lock (_lock)
            {
                if (service.Status == ServiceControllerStatus.Stopped)
                {
                    return true;
                }
                service.Stop();
                try
                {
                    service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                    return true;
                }
                catch (System.ServiceProcess.TimeoutException ex)
                {
                    throw ex;
                }
            }
        }
    }
}
