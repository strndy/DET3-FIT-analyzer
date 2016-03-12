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
        public const int LambdaDelay = 4;

        public const int AfrCorrDelay = 1;

        public ValueMapBuilder(MapCoordinates coord)
        {
            _coord = coord;
        }

        public IEnumerable<LogLine>[,] BuildMap(LogLine[] log)
        {
            var logArray = log.ToArray();
            var map = new List<LogLine>[16,16];

            LogLine logLine;
            for (int i = 0; i < log.Length - 1; i++)
            {
                logLine = log[i];

                //cool engine
                if (logLine.Coolant.Value < 60) continue;
                // acceleration enrichment
                if (logLine.AccEnr.Value > 5)
                {
                    continue;
                }
                //fuel cut
                if (logLine.Map.Value < 26 && logLine.Tps.Value < 1 && logLine.Rpm.Value > 1700) continue;

                if (logLine.Rpm.Value < 1) continue;

                if(Math.Abs(logLine.AfrCorrection.Value) < 0.1 && logLine.AfrWideband.Value > 22) continue;

                if (i + AfrCorrDelay < log.Length)
                {
                    logLine.AfrCorrection.Bytes = log[i + AfrCorrDelay].AfrCorrection.Bytes;
                }

                //lambda delay (cca 100ms)
                if (i + LambdaDelay < log.Length)
                {
                    //Console.WriteLine("Lambda diff: {0}", logLine.AfrWideband.Value - log[i + LambdaDelay].AfrWideband.Value);
                    logLine.AfrWideband.Bytes = log[i + LambdaDelay].AfrWideband.Bytes;
                }

                var kpaIndex = _coord.KpaIndex(logLine.Map.Value);
                var rpmIndex = _coord.RpmIndex(logLine.Rpm.Value);
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
