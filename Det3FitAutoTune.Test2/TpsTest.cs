using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Det3FitAutoTune.Model.Value;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Det3FitAutoTune.Test2
{
    [TestClass]
    public class MapTest
    {
        [TestMethod]
        public void TestSetValGet()
        {
            var map = new Tps();
            for (var i = 0; i < 100; i++)
            {
                map.Value = i;
                Assert.AreEqual(i, Math.Round(map.Value));
            }
        }

        [TestMethod]
        public void TestSetBytesGetBytes()
        {
            var map = new Tps();
            map.Bytes = 200;
            Assert.AreEqual(200, map.Bytes);
        }

        [TestMethod]
        public void TestSetBytesGetBytesAllValues()
        {
            var tps = new Tps();
            for (byte i = 0; i < byte.MaxValue; i += 1)
            {
                tps.Bytes = i;
                Assert.AreEqual(i, tps.Bytes);
            }
        }

        [TestMethod]
        public void TestSetRealVal()
        {
            var testSet = new Dictionary<int, byte>()
            {
                {0, 20},
                {25, 50},
                {49, 80},
                {66, 100},
                {91, 130},
                {99, 149},
 
            };

            var tps = new Tps();
            foreach(var item in testSet)
            {
                tps.Value = 
                Assert.AreEqual(11, Math.Round(map.Value));
            }

        }
    }
}
