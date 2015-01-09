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
            var status = instance.Status;
        }

        [Test]
        public void TestStart()
        {
            instance.Stop();
            instance.Start(null);
            Assert.AreEqual(MonitorStatus.Running, instance.Status);
        }

        [Test]
        public void TestStop()
        {
            instance.Stop();
            Assert.AreEqual(MonitorStatus.Stop, instance.Status);
        }
    }
}
