using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HYMonitors.MonitoredObjs;
using NUnit.Framework;

namespace HYMonitors.Test
{
    [TestFixture]
    class TestMonitoredIISApp
    {
        private BaseMonitoredObj instance;

        [SetUp]
        public void Init()
        {
            var monitors = MonitorBuilder.Build("TestMonitors.xml");
            instance = monitors["HBus"].MonitoredObjs["TestWeb"];
        }

        [Test]
        public void TestGetStatus()
        {
            var status = instance.GetStatus();
        }

        [Test]
        public void TestStart()
        {
            
        }

        [Test]
        public void TestStop()
        {
            
        }
    }
}
