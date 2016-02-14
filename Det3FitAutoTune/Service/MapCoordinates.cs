using System;
using System.Collections.Generic;
using System.Linq;
using Det3FitAutoTune.Extension;

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
            for (var i = 0; i < 16; i++)
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
            
            for (var i = 1; i < 16; i++)
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
    }
}
