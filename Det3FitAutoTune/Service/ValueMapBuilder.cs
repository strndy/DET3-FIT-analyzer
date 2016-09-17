using System;
using System.Collections.Generic;
using System.Linq;
using Det3FitAutoTune.Model;

namespace Det3FitAutoTune.Service
{
    public class ValueMapBuilder
    {
        private readonly MapCoordinates _coord;

        /// <summary>
        /// Lambda delay in frames (1/30s = 33ms), innovate lambda delay is cca 100ms
        /// </summary>
        public static readonly int[] LambdaDelay = new int[16]
        {
            4,4,4,3,3,3,3,2,2,2,2,2,2,2,2,2
            //5,5,5,5,5,5,5,4,4,4,3,3,3,3,3,3
            //4,4,4,5,5,6,6,6,6,7,7,7,7,7,7,7
            //10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10
        };

        public const int AfrCorrDelay = 0;

        public ValueMapBuilder(MapCoordinates coord)
        {
            _coord = coord;
        }

        public IEnumerable<LogLine>[,] BuildMap(LogLine[] log)
        {
            var map = new List<LogLine>[16,16];

            for (int i = 0; i < log.Length - 1; i++)
            {
                var logLine = log[i];

                //cool engine
                if (logLine.Coolant.Value < 70) continue;

                // acceleration enrichment
                if (logLine.AccEnr.Value > 5)
                {
                    continue;
                }

                //fuel cut
                if (logLine.Map.Value < 20 && logLine.Tps.Value < 2 && logLine.Rpm.Value > 1650) continue;

                //engine stop
                if (logLine.Rpm.Value < 3) continue;

                //fuel cut
                if(Math.Abs(logLine.AfrCorrection.Value) < 0.1 && logLine.AfrWideband.Value > 22) continue;

                if (i + AfrCorrDelay < log.Length)
                {
                    logLine.AfrCorrection.Bytes = log[i + AfrCorrDelay].AfrCorrection.Bytes;
                }

                var rpmIndex = _coord.RpmIndex(logLine.Rpm.Value);
                var lambdaDelay = LambdaDelay[rpmIndex];
                //var lambdaDelay = 0;
                //lambda delay (cca 100ms)
                if (i + lambdaDelay < log.Length)
                {
                    //Console.WriteLine("Lambda diff: {0}", logLine.AfrWideband.Value - log[i + LambdaDelay].AfrWideband.Value);
                    logLine.AfrWideband.Bytes = log[i + lambdaDelay].AfrWideband.Bytes;
                }


                var weightedLocations = _coord.GetWeightedLocations(logLine);

                var topOne = weightedLocations.OrderByDescending(w => w.ProximityIndex).FirstOrDefault();
                //if (topOne != null)
                //{
                //    topOne.ProximityIndex = 1;    
                //}
                

                foreach (var weightedLocation in weightedLocations)
                {
                    if (map[weightedLocation.RpmIndex, weightedLocation.KpaIndex] == null)
                    {
                        map[weightedLocation.RpmIndex, weightedLocation.KpaIndex] = new List<LogLine>();
                    }

                    logLine.ProximityIndex = weightedLocation.ProximityIndex;
                    map[weightedLocation.RpmIndex, weightedLocation.KpaIndex].Add(logLine);          
                }

                
                
            }

            return map;
        }

    }
}
