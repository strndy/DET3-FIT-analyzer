using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Det3FitAutoTune.Model;
using Det3FitAutoTune.Model.Value;
using Det3FitAutoTune.Service;

namespace Det3FitAutoTune
{
    class Program
    {
        public const int samplesPerSec = 30;
        public const int lineLength = 40;

        public const string VeTablesDir = @"C:\Dev\repos\moje\DET3-FIT-analyzer\Samples\tables\";
        public const string LogDir = @"C:\Dev\repos\moje\DET3-FIT-analyzer\Samples\";

        
        private static string GetLastFileFromDir(string dir)
        {
            return Directory.GetFiles(dir, "*").OrderByDescending(d => new FileInfo(d).LastWriteTime).First();
        }

        static void Main(string[] args)
        {

            var coords = new MapCoordinates();
            var mapBuilder = new ValueMapBuilder(coords);
            var targetAfr = new TargerAfrMap();
            var analyser = new AfrAnalyser(targetAfr);
            var display = new MapShower(coords);
            var logReader = new LogReader();
            var veTableCorrector = new VeTableCorrector(coords);
            //var widebandDelayCalculator = new WidebandDelayCalculator(coords);

            var veTableReader = new VeTableReader();

            var veTableBytes = File.ReadAllBytes(@"C:\Dev\repos\moje\DET3-FIT-analyzer\Samples\tables\VETable_03_23_1921.bin");
            var veTable = veTableReader.ReadTable(veTableBytes);

            var logBytes = File.ReadAllBytes(@"C:\Dev\repos\moje\DET3-FIT-analyzer\Samples\log_2016323_1921.dlg");

            IEnumerable<LogLine> log = logReader.ReadLog(logBytes);

            var logArray = log.ToArray();
            var map = mapBuilder.BuildMap(logArray);

            //var widebandDelay = widebandDelayCalculator.CalculateAfrDelay(logArray);

            var analysed = analyser.GetCorrection(map);
            float[,] finalCorrection;
            var correctedVeTable = veTableCorrector.TuneVeTable(analysed, veTable, out finalCorrection);

            Console.WriteLine("Count");
            display.ShowAfrMap(analysed, ProjectedAfrCorrection.AfrCorrectionMethod.Count);

            //Console.WriteLine("AvgWidebandDelay");
            //display.ShowAfrTarget(widebandDelay);

            //Console.WriteLine("AvgKpa");
            //display.ShowAfrMap(analysed, ProjectedAfrCorrection.AfrCorrectionMethod.AvgKpa);

            //Console.WriteLine("AvgRpm");
            //display.ShowAfrMap(analysed, ProjectedAfrCorrection.AfrCorrectionMethod.AvgRpm);

            Console.WriteLine("AfrTarget");
            display.ShowAfrTarget(TargerAfrMap.AfrTargetMap);

            Console.WriteLine("AvgAfr");
            display.ShowAfrMap(analysed, ProjectedAfrCorrection.AfrCorrectionMethod.AvgAfr);

            Console.WriteLine("AfrDiffAbsolute");
            display.ShowAfrMap(analysed, ProjectedAfrCorrection.AfrCorrectionMethod.AfrDiffAbsolute);

            Console.WriteLine("AfrPercentDiff");
            display.ShowAfrMap(analysed, ProjectedAfrCorrection.AfrCorrectionMethod.AfrDiffPercent);

            Console.WriteLine("NboCorrection");
            display.ShowAfrMap(analysed, ProjectedAfrCorrection.AfrCorrectionMethod.NboCorrection);

            Console.WriteLine("VE table - delta");
            display.ShowVeMap(finalCorrection, ProjectedAfrCorrection.AfrCorrectionMethod.AfrDiffPercent);

            Console.WriteLine("VE table - original");
            display.ShowVeMap(veTable);

            //Console.WriteLine("VE table - corrected");
            //display.ShowVeMap(correctedVeTable);

            var bytes = veTableReader.GetBytes(correctedVeTable);

            Console.WriteLine("VE table - corrected, rounded");
            display.ShowVeMap(veTableReader.ReadTable(bytes));

            
            File.WriteAllBytes(@"C:\Dev\repos\moje\DET3-FIT-analyzer\Samples\tables\VETable_corrected.bin", bytes);

            //return;

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

            //var tps = new Tps { Value = 98 };
            //var mapVal = new Map { Value = 30 };

            ////Temps are really rought
            //var iat = new Iat { Value = 42f };
            //var coolant = new Coolant { Value = 42f };
            //var afrCorr = new AfrCorrection() { Value = 13 };

            //var afrWideband = new AfrWideband { Value = (float)14.7 };
            //var accEnr = new AccEnr() { Value = (float)12.3 };

            //var rpm = BitConverter.GetBytes(1234);

            //for (var line = 0; line < howManyLines; line += 1)
            //{
            //    firstLine[LogReader.Index["Tps"]] = tps.Bytes;
            //    firstLine[LogReader.Index["Map"]] = mapVal.Bytes;
            //    firstLine[LogReader.Index["Iat"]] = iat.Bytes;
            //    firstLine[LogReader.Index["Coolant"]] = coolant.Bytes;
            //    firstLine[LogReader.Index["AfrCorrection"]] = afrCorr.Bytes;
            //    firstLine[LogReader.Index["AfrWideband"]] = afrWideband.Bytes;
            //    firstLine[LogReader.Index["AccEnr"]] = accEnr.Bytes;

            //    //RPM
            //    firstLine[10] = rpm[0];
            //    firstLine[11] = rpm[1];


            //    var bits = new BitArray(8);
            //    bits[0] = true;
            //    bits[1] = true;
            //    bits[2] = true;
            //    bits[3] = true;
            //    bits[4] = true;
            //    bits[5] = true;
            //    bits[6] = true;
            //    bits[7] = true;

            //    byte[] newBytes = new byte[1];
            //    bits.CopyTo(newBytes, 0);

            //    firstLine[4] = (byte)line;
            //    firstLine[5] = (byte)line;
            //    firstLine[6] = (byte)line;
            //    firstLine[7] = (byte)line;
            //    firstLine[5] = (byte)line;
            //    firstLine[12] = (byte)line;
            //    firstLine[13] = (byte)line;
            //    firstLine[15] = (byte)line;
            //    firstLine[20] = (byte)line;
            //    result = result.Concat(firstLine).ToArray();
            //}

            //File.WriteAllBytes(@"C:\Dev\repos\moje\DET3-FIT-analyzer\Samples\log_mine.dlg", result);
        }
    }
}
