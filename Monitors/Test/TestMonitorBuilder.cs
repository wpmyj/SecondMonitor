using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using HYMonitors.CustomMonitors;
using NUnit.Framework;

namespace HYMonitors.Test
{
    using HYMonitors;

    [TestFixture]
    class TestMonitorBuilder
    {
        [SetUp]
        public void Init()
        {
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
        public void TestSubList()
        {
            var list = new List<double> {0.2, 0.2, 0.2, 0.2};
            var subList = list.GetRange(0, list.Count/2);
            Assert.AreEqual(2, subList.Count);
        }


        [Test]

        public void TestLoadConfig()
        {
            var monitors = MonitorBuilder.LoadConfig("Monitors.xml");

            Assert.IsTrue(monitors.Count > 0);
        }

        [Test]
        public void TestBuild()
        {
            var monitors = MonitorBuilder.Build();

            Assert.IsTrue(monitors.Count > 0);
        }

        [Test]
        public void TestType()
        {
            var type = typeof(CustomMonitoredObj1);

            Console.WriteLine(type.FullName); 
        }
    }
}
