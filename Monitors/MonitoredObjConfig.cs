using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYMonitors
{
    public class MonitoredObjConfig
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string ProcessFile { get; set; }
    }

    public class MonitorConfig
    {
        public string Name { get; set; }
        public List<MonitoredObjConfig> MonitoredObjs { get; set; }
    }
}
