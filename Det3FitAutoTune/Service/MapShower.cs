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

        public void ShowMap(ProjectedAfrCorrection[,] data, ProjectedAfrCorrection.AfrCorrectionMethod type)
        {
            for (int i = 15; i >= 0; i--)
            {
                Console.Write("{0, 5}", _coords.RpmMap[i]);

                for (int j = 0; j < 16; j++)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    if (data[i, j] == null)
                    {
                        Console.Write(_format, "-");
                    }
                    else
                    {
                        this.ChangeColor(type, data[i, j].GetVal(type));
                        var formatted = string.Format("{0:0.#}", data[i, j].GetVal(type));
                        Console.Write(_format, formatted);
                    }
                    
                }
                Console.WriteLine();
            }

            for (int j = 0; j < 16; j++)
            {
                Console.Write(_format, _coords.KpaMap[j]);
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        private void ChangeColor(ProjectedAfrCorrection.AfrCorrectionMethod type, float number)
        {
            var redOver = 1.5f;
            var yellowOver = 0.5f;
            var greenUnder = -0.5f;
            var blueUnder = -1.5f;

            switch (type)
            {
                case ProjectedAfrCorrection.AfrCorrectionMethod.AvgAfr:
                    redOver = 17f;
                    yellowOver = 15.3f;
                    greenUnder = 14.2f;
                    blueUnder = 13f;
                    break;
                case ProjectedAfrCorrection.AfrCorrectionMethod.Count:
                    return;
                case ProjectedAfrCorrection.AfrCorrectionMethod.NboCorrection:
                case ProjectedAfrCorrection.AfrCorrectionMethod.FinalCorrection:
                case ProjectedAfrCorrection.AfrCorrectionMethod.AfrDiff:
                case ProjectedAfrCorrection.AfrCorrectionMethod.SumValue:
                    break;


                default:
                    throw new ArgumentOutOfRangeException("type", type, null);
            }

            if (number > redOver)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if(number > yellowOver )
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else if (number < greenUnder)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else if (number < blueUnder)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
            }
        }
    }
}
