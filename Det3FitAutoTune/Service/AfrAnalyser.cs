using System;
using System.Collections.Generic;
using System.Linq;
using Det3FitAutoTune.Model;

namespace Det3FitAutoTune.Service
{
    public class AfrAnalyser
    {
        public const float Stoich = 14.7f;

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
            var wideband = new List<float>();
            var correction = new List<float>();
            foreach (var logLine in lines)
            {
                if (!(Math.Abs(logLine.AfrWideband.Value - Stoich) < 2)) continue;
                if (logLine.AccEnr.Value > 5) continue;

                wideband.Add(100 - logLine.AfrWideband.Value/Stoich*100);
                correction.Add(logLine.AfrCorrection.Value);
            }
            return wideband.Count == 0 ? null :  new ProjectedAfrCorrection()
            {
                AfrDiff = wideband.Average(),
                NboCorrection = correction.Average(),
                Count = wideband.Count
            };
        }

        private float CalculateCorrection(float afr, float correction)
        {
            var afrDiff = 100 - ((afr/Stoich)*100);
            return afrDiff + correction;
        }
    }
}
