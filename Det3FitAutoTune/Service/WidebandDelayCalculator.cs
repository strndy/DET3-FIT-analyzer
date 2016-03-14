using System.Collections.Generic;
using System.Linq;
using Det3FitAutoTune.Model;

namespace Det3FitAutoTune.Service
{
    public class WidebandDelayCalculator
    {
        private readonly MapCoordinates _coord;

        public WidebandDelayCalculator(MapCoordinates coordsCoordinates)
        {
            _coord = coordsCoordinates;
        }

        public float[,] CalculateAfrDelay(LogLine[] log)
        {
            var map = new List<int>[16, 16];
            var fuelCut = false;
            for (int i = 0; i < log.Length; i++)
            {
                var logLine = log[i];
                int delay;

                var kpaIndex = _coord.KpaIndex(logLine.Map.Value);
                var rpmIndex = _coord.RpmIndex(logLine.Rpm.Value);

                if (map[rpmIndex, kpaIndex] == null)
                {
                    map[rpmIndex, kpaIndex] = new List<int>();
                }

                
                // fuel cut start
                if (!fuelCut && logLine.Map.Value < 26 && logLine.Tps.Value < 1 && logLine.Rpm.Value > 1650)
                {
                    fuelCut = true;
                    //fuel cut start
                    if (log[i + 1].Map.Value > 26 && log[i + 1].Tps.Value > 1 && log[i + 1].Rpm.Value > 1650)
                    {
                        for (int j = i; j < i + 30; j++)
                        {
                            if (log[j].AfrWideband.Value > 18)
                            {
                                map[rpmIndex, kpaIndex].Add(i - j);
                                break;
                            }
                            
                        }
                        //check how long it took
                    }
                }
                // fuel cut end
                else if (fuelCut && logLine.Map.Value > 26 && logLine.Tps.Value > 1 && logLine.Rpm.Value > 1650)
                {
                    fuelCut = false;
                    //fuel cut start
                    if (log[i + 1].Map.Value > 26 && log[i + 1].Tps.Value > 1 && log[i + 1].Rpm.Value > 1650)
                    {
                        for (int j = i; j < i + 30; j++)
                        {
                            if (log[j].AfrWideband.Value < 16)
                            {
                                map[rpmIndex, kpaIndex].Add(i-j);
                                break;
                            }

                        }
                        //check how long it took
                    }
                }
                else
                {
                    fuelCut = false;
                    continue;
                }


            }

            return Averange(map);
        }

        private float[,] Averange(IEnumerable<int>[,] map)
        {
            var averange = new float[16,16];

            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    averange[i,j] = map[i, j] != null &&  map[i,j].Any() ? (float)map[i, j].Average() : 0;
                }
            }

            return averange;
        }
    }
}
