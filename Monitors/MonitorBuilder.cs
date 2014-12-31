using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HYMonitors.MonitoredObjs;

namespace HYMonitors
{
    static class MonitorBuilder
    {
        internal static Dictionary<string, Monitor> Build()
        {
            throw new NotImplementedException();
        }

    }

    static class MonitoredObjFactory
    {
        internal static BaseMonitoredObj CreateMonitoredObj(MonitoredObjConfig config)
        {
            switch (config.Type)
            {
                case "MonitoredWebSite":
                    return CreateWebMonitor(config);
                case "MonitoredService":
                    return CreateServiceMonitor(config);
                case "MonitoredSchTask":
                    return CreateSchTaskMonitor(config);
                default:
                    break;
            }
            //扩展的自定义类
            if (config.Type.StartsWith("CustomMonitoredObj"))
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new Exception(string.Format("未知的监视类型: {0}", config.Type));
            }
        }

        private static BaseMonitoredObj CreateWebMonitor(MonitoredObjConfig config)
        {
            throw new NotImplementedException();
        }

        private static BaseMonitoredObj CreateServiceMonitor(MonitoredObjConfig config)
        {
            throw new NotImplementedException();
        }

        private static BaseMonitoredObj CreateSchTaskMonitor(MonitoredObjConfig config)
        {
            throw new NotImplementedException();
        }
    }
}
