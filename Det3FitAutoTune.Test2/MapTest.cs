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
    public class TpsTest
    {
        [TestMethod]
        public void TestSetValGet()
        {
            var map = new Map();
            for (var i = 0; i < 200; i++)
            {
                map.Value = i;
                Assert.AreEqual(i, Math.Round(map.Value));
            }
        }

        [TestMethod]
        public void TestSetBytesGetBytes()
        {
            var map = new Map();
            for (byte i = 0; i < 255; i++)
            {
                map.Bytes = i;
                Assert.AreEqual(i, map.Bytes);
            }
        }

        [TestMethod]
        public void TestSetRealVal()
        {
            var testSet = new Dictionary<int, byte>()
            {
                {14, 20},
                {92, 100},
                {190, 200},
 
            };

            var tps = new Map();
            foreach (var item in testSet)
            {
                var expValue = item.Key;
                var expBytes = item.Value;

                tps.Value = expValue;
                int diff = Math.Abs(expBytes - tps.Bytes);
                Assert.IsTrue(diff < 3);
                Assert.IsTrue(Math.Abs(expValue - tps.Value) < 3);

                
            }

        }
    }
}
