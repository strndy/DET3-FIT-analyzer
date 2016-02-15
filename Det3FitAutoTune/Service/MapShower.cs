using System;
using Det3FitAutoTune.Model;

namespace Det3FitAutoTune.Service
{
    public class MapShower
    {
        private const string _format = "{0, 6}";
        private readonly MapCoordinates _coords;

        public MapShower(MapCoordinates coords)
        {
            _coords = coords;
        }

        public void DisplayMap(ProjectedAfrCorrection[,] data)
        {
            for (int i = 15; i >= 0; i--)
            {
                Console.Write("{0, 5}", _coords.RpmMap[i]);

                for (int j = 0; j < 16; j++)
                {
                    if (data[i, j] == null)
                    {
                        Console.Write(_format, "-");
                    }
                    else
                    {
                        var formatted = string.Format("{0:##.#}", data[i, j].AfrDiff);
                        Console.Write(_format, formatted);
                    }
                    
                }
                Console.WriteLine();
            }
            Console.WriteLine();

            for (int j = 0; j < 16; j++)
            {
                Console.Write(_format, _coords.KpaMap[j]);
            }
        }
    }
}
