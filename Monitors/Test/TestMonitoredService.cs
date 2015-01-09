using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HYMonitors.MonitoredObjs;
using NUnit.Framework;

namespace HYMonitors.Test
{
    class TestMonitoredService
    {
        private BaseMonitoredObj instance;
        private BaseMonitoredObj remoteInstance;

        [SetUp]
        public void Init()
        {
            var monitors = MonitorBuilder.Build("TestMonitors.xml");
            instance = monitors["HBus"].MonitoredObjs["HaiYi queue_msgcenter_cmds"];
            remoteInstance = monitors["HBus"].MonitoredObjs["gupdate"];
        }

        [Test]
        public void TestNullableRef()
        {
            int? x = 1;
            int? y = x;
            x = 2;
            Assert.AreNotEqual(x, y);
        }

        [Test]
        public void TestGetStatus()
        {
            var status = instance.Status;
            Console.WriteLine(status);
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

        [Test]
        public void TestRemoteGetStatus()
        {
            var status = remoteInstance.Status;
            Console.WriteLine(status);
        }

        [Test]
        public void TestRemoteStart()
        {
            remoteInstance.Stop();
            remoteInstance.Start(null);
            Assert.AreEqual(MonitorStatus.Running, remoteInstance.Status);
        }

        [Test]
        public void TestRemoteStop()
        {
            remoteInstance.Stop();
            Assert.AreEqual(MonitorStatus.Stop, remoteInstance.Status);
        }
    }
}
