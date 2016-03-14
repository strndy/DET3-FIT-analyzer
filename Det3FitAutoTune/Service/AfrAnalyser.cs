using System;
using System.Collections.Generic;
using System.Linq;
using Det3FitAutoTune.Model;

namespace Det3FitAutoTune.Service
{
    public class AfrAnalyser
    {
        private readonly TargerAfrMap _targerAfr;

        public AfrAnalyser(TargerAfrMap targerAfr)
        {
            _targerAfr = targerAfr;
        }

        /// <summary>
        /// Minimum values to take into account field 30 is one second
        /// </summary>
        public const int MinValues = 1;

        public ProjectedAfrCorrection[,] GetCorrection(IEnumerable<LogLine>[,] allValues)
        {
            var ANALysed = new ProjectedAfrCorrection[16, 16];
            for (var rpmIndex = 0; rpmIndex < 16; rpmIndex++)
            {
                for (var kpaIndex = 0; kpaIndex < 16; kpaIndex++)
                {
                    var values = allValues[rpmIndex, kpaIndex];
                    if (values == null) continue;
                    var targetAfr = _targerAfr.GetTargetAfr(rpmIndex, kpaIndex);

                    var correction = AverangeCorrection(values, targetAfr);
                    ANALysed[rpmIndex, kpaIndex] = correction;
                }
            }
            return ANALysed;
        }

        private ProjectedAfrCorrection AverangeCorrection(IEnumerable<LogLine> lines, float targetAfr)
        {
            var widebandDiffPercent = new List<float>();
            var widebandDiffAbsolute = new List<float>();
            var correction = new List<float>();
            var sumarized = new List<float>();
            var afr = new List<float>();

            var avgRpm = new List<double>();
            var avgKpa = new List<float>();
            

            foreach (var logLine in lines)
            {
                if (logLine.AccEnr.Value > 5) continue;
                widebandDiffAbsolute.Add(targetAfr - logLine.AfrWideband.Value);
                widebandDiffPercent.Add(logLine.AfrWideband.Value / targetAfr * 100 - 100);
                correction.Add(logLine.AfrCorrection.Value);
                sumarized.Add(widebandDiffPercent.Last() + correction.Last());
                afr.Add(logLine.AfrWideband.Value);
                avgRpm.Add(logLine.Rpm.Value);
                avgKpa.Add(logLine.Map.Value);
            }
            return widebandDiffPercent.Count < MinValues ? null :  new ProjectedAfrCorrection()
            {
                AfrDiffPercent = widebandDiffPercent.Average(),
                AfrDiffAbsolute = widebandDiffAbsolute.Average(),
                NboCorrection = correction.Average(),
                SumValue = sumarized.Average(),
                Count = widebandDiffPercent.Count,
                AvgAfr = afr.Average(),
                AvgKpa = avgKpa.Average(),
                AvgRPM = (float)Math.Round(avgRpm.Average())
            };
        }
    }
}
