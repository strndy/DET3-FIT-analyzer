using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Det3FitAutoTune.Model;
using Det3FitAutoTune.Model.Value;

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
            4,4,4,5,5,6,6,6,6,7,7,7,7,7,7,7
        };

        public const int AfrCorrDelay = 1;

        public ValueMapBuilder(MapCoordinates coord)
        {
            _coord = coord;
        }

        public IEnumerable<LogLine>[,] BuildMap(LogLine[] log)
        {
            var logArray = log.ToArray();
            var map = new List<LogLine>[16,16];

            for (int i = 0; i < log.Length - 1; i++)
            {
                var logLine = log[i];
                var kpaIndex = _coord.KpaIndex(logLine.Map.Value);
                var rpmIndex = _coord.RpmIndex(logLine.Rpm.Value);

                //cool engine
                if (logLine.Coolant.Value < 70) continue;
                // acceleration enrichment
                if (logLine.AccEnr.Value > 5)
                {
                    continue;
                }

                //fuel cut
                if (logLine.Map.Value < 20 && logLine.Tps.Value < 1 && logLine.Rpm.Value > 1650) continue;

                //engine stop
                if (logLine.Rpm.Value < 3) continue;

                //fuel cut
                if(Math.Abs(logLine.AfrCorrection.Value) < 0.1 && logLine.AfrWideband.Value > 22) continue;

                if (i + AfrCorrDelay < log.Length)
                {
                    logLine.AfrCorrection.Bytes = log[i + AfrCorrDelay].AfrCorrection.Bytes;
                }

                //lambda delay (cca 100ms)
                if (i + LambdaDelay[rpmIndex] < log.Length)
                {
                    //Console.WriteLine("Lambda diff: {0}", logLine.AfrWideband.Value - log[i + LambdaDelay].AfrWideband.Value);
                    logLine.AfrWideband.Bytes = log[i + LambdaDelay[rpmIndex]].AfrWideband.Bytes;
                }


                if (map[rpmIndex, kpaIndex] == null)
                {
                    map[rpmIndex, kpaIndex] = new List<LogLine>();
                }
                map[rpmIndex, kpaIndex].Add(logLine);
            }

            return map;
        }

    }
}
