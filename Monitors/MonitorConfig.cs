using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYMonitors
{
    internal class MonitorConfig
    {
        public string Name { get; set; }
        public List<MonitoredObjConfig> MonitoredObjs { get; set; }
    }

    internal class MonitoredObjConfig
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Type { get; set; }
        public bool Watched { get; set; }
        public bool Remote { get; set; }
        public string Property1 { get; set; }
        public string Property2 { get; set; }
    }
}
