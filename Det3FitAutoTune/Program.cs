using System;
using System.IO;
using System.Linq;
using System.Text;
using Det3FitAutoTune.Service;
using Det3FitAutoTune.Model.Value;

namespace Det3FitAutoTune
{
    class Program
    {
        public const int samplesPerSec = 30;
        public const int lineLength = 40;

        static void Main(string[] args)
        {
            var logReader = new LogReader();
            var logBytes = File.ReadAllBytes(@"C:\Dev\repos\moje\DET3-FIT-analyzer\Samples\log_2016215_031.dlg");

            var coords = new MapCoordinates();
            var mapBuilder = new ValueMapBuilder(coords);
            var analyser = new AfrAnalyser();
            var display = new MapShower(coords);

            var log = logReader.ReadLog(logBytes);
            var map = mapBuilder.BuildMap(log);

            var analysed = analyser.GetAverangeAfrCorrection(map);

            display.DisplayMap(analysed);

            return;

            //var startBytes = logBytes.Take(12).ToArray();

            //var timeInSeconds = 10;
            //var howManyLines = (samplesPerSec * timeInSeconds);

            //var firstLine = logBytes.Skip(12 + 40 * 10).Take(40).ToArray();
            //var hex = BitConverter.ToString(firstLine);
            //Console.WriteLine(hex);
            //Console.WriteLine(Encoding.ASCII.GetChars(firstLine, 0, lineLength));

            //var value = BitConverter.ToInt16(firstLine, 0);
            //Console.WriteLine(value);

            //var result = startBytes;

            //var tps = new Tps { Value = 42};
            //var map = new Map { Value = 42};
            
            ////Temps are really rought
            //var iat = new Iat {Value = 42};
            //var coolant = new Coolant { Value = 42 };
            //var lambdaCorrection = new AfrCorrection() {Value = 13};
            //var afr = new AfrWideband {Value = (float)14.7};
            //var accEnr = new AccEnr() { Value = (float)12.3 };

            //var rpm = BitConverter.GetBytes(1234);

            //for (var line = 0; line < howManyLines; line += 1)
            //{
            //    firstLine[tps.Index] = tps.Bytes;
            //    firstLine[map.Index] = map.Bytes;
            //    firstLine[iat.Index] = iat.Bytes;
            //    firstLine[coolant.Index] = coolant.Bytes;
            //    firstLine[lambdaCorrection.Index] = lambdaCorrection.Bytes;
            //    firstLine[afr.Index] = afr.Bytes;
            //    firstLine[accEnr.Index] = accEnr.Bytes;

            //    firstLine[10] = rpm[0];
            //    firstLine[11] = rpm[1];

            //    //Ase 16
            //    //Ign 17
            //    //
            //    // acc voltage 19
            //    firstLine[19] = (byte)line;
            //    result = result.Concat(firstLine).ToArray();
            //}

            //File.WriteAllBytes(@"C:\Dev\repos\moje\DET3-FIT-analyzer\Samples\log_mine.dlg", result);
        }

        private static byte[] ChangeValue(byte[] data)
        {
            throw new Exception();
        }
    }
}
