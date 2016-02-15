﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Det3FitAutoTune.Model;
using Det3FitAutoTune.Model.Value;

namespace Det3FitAutoTune.Service
{
    public class ValueMapBuilder
    {
        private readonly MapCoordinates _coord;

        public ValueMapBuilder(MapCoordinates coord)
        {
            _coord = coord;
        }

        public IEnumerable<LogLine>[,] BuildMap(IEnumerable<LogLine> log)
        {
            var map = new List<LogLine>[16,16];

            foreach (var logLine in log)
            {
                //TODO ignore acc enr., warmup, atd
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
