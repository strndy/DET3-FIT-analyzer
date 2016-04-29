using System.Linq;
using Det3FitAutoTune.Model;
using Det3FitAutoTune.Model.Value;
using Det3FitAutoTune.Service;
using NUnit.Framework;

namespace Det3FitAutoTune.Test2
{
    public class MapCoordinatesTests
    {
        [TestCase(1000, 44)]
        [TestCase(1000, 38)]
        [TestCase(1000, 100)]
        [TestCase(1500, 50)]
        [TestCase(1250, 50)]
        [TestCase(1250, 52)]
        [TestCase(850, 32)]
        [TestCase(850, 94)]
        [TestCase(2000, 20)]
        [TestCase(7000, 198)]
        public void TestImportance(float rpm, float kpa)
        {
            var service = new MapCoordinates();
            var line = new LogLine()
            {
                Map = new Map { Value = kpa },
                Rpm = new Rpm { Value = rpm }
            };

            var location = service.GetWeightedLocations(line);

            Assert.GreaterOrEqual(location.Count(), 1);
            Assert.Greater(location.Sum(l => l.ProximityIndex), 0.85);
            Assert.Less(location.Sum(l => l.ProximityIndex), 1.18);

        }
    }
}
