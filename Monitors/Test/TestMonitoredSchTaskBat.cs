using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HYMonitors.MonitoredObjs;
using NUnit.Framework;

namespace HYMonitors.Test
{
    class TestMonitoredSchTaskBat
    {
        private BaseMonitoredObj instance;

        [SetUp]
        public void Init()
        {
            var monitors = MonitorBuilder.Build("TestMonitors.xml");
            instance = monitors["XXX"].MonitoredObjs["TestSchTask"];
        }

        [Test]
        public void TestGetStatus()
        {
            var status = instance.Status;
        }

        [Test]
        public void TestStart()
        {
            instance.Start(null);
            Assert.AreEqual(MonitorStatus.Running, instance.Status);
            while (true)
            {
                if (instance.Status == MonitorStatus.Stop)
                {
                    break;
                }
            }
            Thread.Sleep(5000);
            Assert.AreEqual(MonitorStatus.Stop, instance.Status);
        }
    }
}
