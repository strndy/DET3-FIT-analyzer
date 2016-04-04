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

        public const int RpmMax = 8000;

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
            return RpmMap.ClosestToIndex(value);
        }

        public int KpaIndex(float value)
        {
            return KpaMap.ClosestToIndex(value);
        }

        public IEnumerable<WeightedLocation> GetWeightedLocations(LogLine logLine)
        {
            var baseRpmIndex = RpmIndex(logLine.Rpm.Value);
            var baseKpaIndex = KpaIndex(logLine.Map.Value);

            var locations = new List<WeightedLocation>();

            for (int kpa = SafeIndex(baseKpaIndex - 1); kpa < SafeIndex(baseKpaIndex + 1); kpa++)
            {
                for (int rpm = SafeIndex(baseRpmIndex - 1); rpm < SafeIndex(baseRpmIndex + 1); rpm++)
                {
                    var location = new WeightedLocation()
                    {
                        KpaIndex = kpa,
                        RpmIndex = rpm,
                        Importance = GetImportance(kpa, rpm, logLine)
                    };
                    if (location.Importance > 0.1)
                    {
                        locations.Add(location);    
                    }
                }    
            }

            return locations;
        }

        public float GetImportance(int kpa, int rpm, LogLine logLine)
        {
            var exactRpmIndex = ExactRpmIndex(logLine);
            var exactKpaIndex = ExactKpaIndex(logLine);

            throw new NotImplementedException();
        }

        public float ExactRpmIndex(LogLine logLine)
        {
            return (logLine.Rpm.Value / RpmMax) * 16 - 1;
        }

        public float ExactKpaIndex(LogLine logLine)
        {
            return (logLine.Map.Value / (KpaMax - KpaMin)) * 16 - 1;
        }

        private static int SafeIndex(int index)
        {
            if (index > 15)
            {
                return 15;
            }
            if (index < 0)
            {
                return 0;
            }

            return index;
        }
    }
}
