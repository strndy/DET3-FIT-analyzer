using System;
using System.Collections.Generic;
using System.Linq;
using Det3FitAutoTune.Extension;
using Det3FitAutoTune.Model;

namespace Det3FitAutoTune.Service
{
    public class MapCoordinates
    {
        public const int KpaMin = 25;
        public const int KpaMax = 225;
        private static float _realKpaOffset = (KpaMax - KpaMin) / (16);

        public const int RpmMax = 8000;
        public const int RpmOffset = 500;
        private static int _realRpmOffset = RpmMax / (16 * 2) + RpmOffset;

        private static int[] _kpaMap;
        private static int[] _rpmMap;

        public int[] KpaMap
        {
            get { return _kpaMap ?? (_kpaMap = CalcKpa()); }
        }

        public int[] RpmMap
        {
            get { return _rpmMap ?? (_rpmMap = CalcRpm()); }
        }

        private static int[] CalcKpa()
        {
            float step = (KpaMax - KpaMin) / (float)15;
            var values = new List<int> { KpaMin };
            float realVal = KpaMin;
            for (var i = 1; i <= 15; i++)
            {
                realVal += step;
                values.Add((int)Math.Round(realVal));
            }

            return values.ToArray();
        }

        private static int[] CalcRpm()
        {
            var step = RpmMax / 16;
            var values = new List<int> { step };
            
            for (var i = 1; i <= 15; i++)
            {
                values.Add(values.Last() + step);
            }

            return values.ToArray();
        }

        public int RpmIndex(float value)
        {
            return RpmMap.ClosestToIndex(value + _realRpmOffset);
        }

        public int KpaIndex(float value)
        {
            return KpaMap.ClosestToIndex(value - _realKpaOffset);
        }

        public IEnumerable<WeightedLocation> GetWeightedLocations(LogLine logLine)
        {
            var baseRpmIndex = RpmIndex(logLine.Rpm.Value);
            var baseKpaIndex = KpaIndex(logLine.Map.Value);

            var locations = new List<WeightedLocation>();

            for (int kpaI = SafeIndex(baseKpaIndex - 2); kpaI <= SafeIndex(baseKpaIndex + 2); kpaI++)
            {
                for (int rpmI = SafeIndex(baseRpmIndex - 2); rpmI <= SafeIndex(baseRpmIndex + 2); rpmI++)
                {
                    var location = new WeightedLocation
                    {
                        KpaIndex = kpaI,
                        RpmIndex = rpmI,
                        ProximityIndex = CalcProximityIndex(kpaI, rpmI, logLine)
                    };
                    if (location.ProximityIndex > 0.05)
                    {
                        locations.Add(location);    
                    }
                }
            }

            if (locations.Count == 1)
            {
                locations[0].ProximityIndex = 1;
            }

            return locations;
        }

        public float CalcProximityIndex(int kpaI, int rpmI, LogLine logLine)
        {
            var exactRpmIndex = ExactRpmIndex(logLine);
            var exactKpaIndex = ExactKpaIndex(logLine);

            var kpaDiff = Math.Abs(kpaI - exactKpaIndex + 0.5);
            var rpmDiff = Math.Abs(rpmI - exactRpmIndex + 0.5);

            var distance = Math.Sqrt(Math.Pow(kpaDiff, 2) + Math.Pow(rpmDiff, 2));

            //var index = (float)Math.Log(1 / distance, 10);
            //return Math.Min(1, Math.Max(0, index));

            if (distance < 1)
            {
                return (float) (1 - distance);
            }

            return 0;
        }

        public float ExactRpmIndex(LogLine logLine)
        {
            return ((logLine.Rpm.Value + _realRpmOffset) / RpmMax) * 16 - 1;
        }

        public float ExactKpaIndex(LogLine logLine)
        {
            return (logLine.Map.Value - _realKpaOffset) / (KpaMax - KpaMin) * 16 - 1;
        }

        private static int SafeIndex(int index)
        {
            return Math.Min(15, Math.Max(0, index));
        }
    }
}
