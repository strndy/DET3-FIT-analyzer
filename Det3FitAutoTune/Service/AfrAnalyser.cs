using System;
using System.Collections.Generic;
using System.Linq;
using Det3FitAutoTune.Extension;
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
        public const int MinValues = 10;

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
            return new ProjectedAfrCorrection()
            {
                AfrDiffPercent = (float)lines.WeightedAverage(
                    l => (l.AfrWideband.Value / targetAfr) * 100 - 100,
                    l => l.ProximityIndex),
                //widebandDiffPercent.Average(),

                AfrDiffAbsolute = (float) lines.WeightedAverage(
                    l => targetAfr - l.AfrWideband.Value, 
                    l => l.ProximityIndex), //widebandDiffAbsolute.Average(),

                NboCorrection = (float) lines.WeightedAverage(
                    l => l.AfrCorrection.Value,
                    l => l.ProximityIndex
                ),

                Count = lines.Count(),

                AvgAfr = (float)lines.WeightedAverage(
                    l => l.AfrWideband.Value,
                    l => l.ProximityIndex
                ),
                AvgKpa = lines.Average(l => l.Map.Value),
                AvgRpm = (int)lines.Average(l => l.Rpm.Value)
            };
        }
    }
}
