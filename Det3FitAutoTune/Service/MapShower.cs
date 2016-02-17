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

        public void ShowVeMap(float[,] data, ProjectedAfrCorrection.AfrCorrectionMethod? type = null)
        {
            for (int i = 15; i >= 0; i--)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("{0, 5}", _coords.RpmMap[i]);

                for (int j = 0; j < 16; j++)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;

                    ChangeColor(data[i, j], type);
                    var formatted = string.Format("{0:0.#}", data[i, j]);
                    Console.Write(_format, formatted);
                    
                }
                Console.WriteLine();
            }

            Console.Write(_format, "");
            Console.ForegroundColor = ConsoleColor.Gray;
            for (int j = 0; j < 16; j++)
            {
                Console.Write(_format, _coords.KpaMap[j]);
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        public void ShowAfrTarget(float[,] data)
        {
            for (int i = 15; i >= 0; i--)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("{0, 5}", _coords.RpmMap[i]);

                for (int j = 0; j < 16; j++)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;

                    ChangeColor(data[i, j], ProjectedAfrCorrection.AfrCorrectionMethod.AvgAfr);
                    var formatted = string.Format("{0:0.#}", data[i, j]);
                    Console.Write(_format, formatted);

                }
                Console.WriteLine();
            }

            Console.Write(_format, "");
            Console.ForegroundColor = ConsoleColor.Gray;
            for (int j = 0; j < 16; j++)
            {
                Console.Write(_format, _coords.KpaMap[j]);
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        public void ShowAfrMap(ProjectedAfrCorrection[,] data, ProjectedAfrCorrection.AfrCorrectionMethod type)
        {
            for (int i = 15; i >= 0; i--)
            {
                Console.Write("{0, 5}", _coords.RpmMap[i]);

                for (int j = 0; j < 16; j++)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    if (data[i, j] == null)
                    {
                        Console.Write(_format, "-");
                    }
                    else
                    {
                        ChangeColor(data[i, j].GetVal(type), type);
                        var formatted = string.Format("{0:0.#}", data[i, j].GetVal(type));
                        Console.Write(_format, formatted);
                    }
                }
                Console.WriteLine();
            }

            Console.Write(_format, "");
            for (int j = 0; j < 16; j++)
            {
                Console.Write(_format, _coords.KpaMap[j]);
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        private void ChangeColor(float number, ProjectedAfrCorrection.AfrCorrectionMethod? type = null)
        {
            var redOver = 2f;
            var yellowOver = 0.7f;
            var greenUnder = -0.7f;
            var blueUnder = -2f;

            switch (type)
            {
                case ProjectedAfrCorrection.AfrCorrectionMethod.AvgAfr:
                    redOver = 17f;
                    yellowOver = 15.0f;
                    greenUnder = 14.2f;
                    blueUnder = 13f;
                    break;
                case ProjectedAfrCorrection.AfrCorrectionMethod.Count:
                    return;
                case ProjectedAfrCorrection.AfrCorrectionMethod.NboCorrection:
                case ProjectedAfrCorrection.AfrCorrectionMethod.FinalCorrection:
                case ProjectedAfrCorrection.AfrCorrectionMethod.AfrDiffPercent:
                case ProjectedAfrCorrection.AfrCorrectionMethod.SumValue:
                    break;
                default:
                    redOver = 80;
                    yellowOver = 65;
                    greenUnder = 50;
                    blueUnder = 45;
                    break;
            }
            if (number < yellowOver && number > greenUnder)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
            } 
            else if (number > redOver)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if(number > yellowOver )
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else if (number < blueUnder)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            else if (number < greenUnder)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }
}
