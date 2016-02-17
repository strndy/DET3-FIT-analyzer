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
        public const int LambdaDelay = 3;

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
                if (logLine.Map.Value < 35 && logLine.Tps.Value < 3 && logLine.Rpm.Value > 1500) continue;

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
