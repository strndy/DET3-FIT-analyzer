
using Det3FitAutoTune.Model;
using Det3FitAutoTune.Model.Value;
using Det3FitAutoTune.Service;
using NUnit.Framework;

namespace Det3FitAutoTune.Test2
{
    [TestFixture]
    public class CoordsTest
    {
        [Test]
        public void TestAssign()
        {
            var coords = new MapCoordinates();

            Assert.AreEqual(3, coords.RpmIndex(1888));
            Assert.AreEqual(2, coords.RpmIndex(1251));
            Assert.AreEqual(1, coords.RpmIndex(1249));
            Assert.AreEqual(1, coords.RpmIndex(1000));
            Assert.AreEqual(1, coords.RpmIndex(780));
            Assert.AreEqual(1, coords.RpmIndex(751));
            Assert.AreEqual(1, coords.RpmIndex(752));
            Assert.AreEqual(0, coords.RpmIndex(749));
            Assert.AreEqual(0, coords.RpmIndex(749));
        }

        [Test]
        public void TestIndexing()
        {
            var coords = new MapCoordinates();

            var logline = new LogLine(){Rpm = new Rpm(){Value = 1888}, Map = new Map(){Value = 45}};
            Assert.AreEqual(2.7777, coords.ExactRpmIndex(logline));
            Assert.AreEqual(2.6, coords.ExactKpaIndex(logline));
        }
    }
}
