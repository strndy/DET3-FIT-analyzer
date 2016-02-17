using System;
using System.Collections.Generic;
using System.Linq;
using Det3FitAutoTune.Model;

namespace Det3FitAutoTune.Service
{
    public class AfrAnalyser
    {
        public const float Stoich = 14.7f;

        /// <summary>
        /// Minimum values to take into account field 30- one second
        /// </summary>
        public const int MinValues = 30;

        public ProjectedAfrCorrection[,] GetAverangeAfrCorrection(IEnumerable<LogLine>[,] allValues)
        {
            var ANALysed = new ProjectedAfrCorrection[16, 16];
            for (var i = 0; i < 16; i++)
            {
                for (var j = 0; j < 16; j++)
                {
                    var values = allValues[i, j];
                    if (values == null) continue;

                    var correction = AverangeCorrection(values);
                    ANALysed[i, j] = correction;
                }
            }
            return ANALysed;
        }

        private ProjectedAfrCorrection AverangeCorrection(IEnumerable<LogLine> lines)
        {
            //TODO target AFR per boost!
            var wideband = new List<float>();
            var correction = new List<float>();
            var sumarized = new List<float>();
            var afr = new List<float>();

            foreach (var logLine in lines)
            {
                if (!(Math.Abs(logLine.AfrWideband.Value - Stoich) < 2)) continue;
                if (logLine.AccEnr.Value > 5) continue;

                wideband.Add(logLine.AfrWideband.Value/Stoich*100 - 100);
                correction.Add(logLine.AfrCorrection.Value);
                sumarized.Add(wideband.Last() + correction.Last());
                afr.Add(logLine.AfrWideband.Value);
            }
            return wideband.Count < MinValues ? null :  new ProjectedAfrCorrection()
            {
                AfrDiff = wideband.Average(),
                NboCorrection = correction.Average(),
                SumValue = sumarized.Average(),
                Count = wideband.Count,
                AvgAfr = afr.Average()
            };
        }
    }
}
