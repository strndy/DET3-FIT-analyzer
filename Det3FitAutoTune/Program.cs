﻿using System;
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

        public const string VeTablesDir = @"C:\dev\repos\DET3-FIT-analyzer\Samples\tables\";
        public const string LogDir = @"C:\dev\repos\DET3-FIT-analyzer\Samples\";
        public const string ExportDir = @"C:\dev\repos\DET3-FIT-analyzer\Samples\tables\";
        


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

            var veTableBytes = File.ReadAllBytes(VeTablesDir + "VETable_2017220_1021.bin");
            var veTable = veTableReader.ReadTable(veTableBytes);

            var logBytes = File.ReadAllBytes(LogDir + "log_2017220_1021.dlg");
            IEnumerable <LogLine> log = logReader.ReadLog(logBytes);
            //var logBytes2 = File.ReadAllBytes(@"C:\Dev\repos\moje\DET3-FIT-analyzer\Samples\log_2016710_2340.dlg");
            //IEnumerable<LogLine> log2 = logReader.ReadLog(logBytes);
            //var logBytes3 = File.ReadAllBytes(@"C:\Dev\repos\moje\DET3-FIT-analyzer\Samples\log_2016528_1814_CESTA_zpet.dlg");
            //IEnumerable<LogLine> log3 = logReader.ReadLog(logBytes);

            //log = log.Concat(log2);

            var logArray = log.ToArray();
            var map = mapBuilder.BuildMap(logArray);

            //var widebandDelay = widebandDelayCalculator.CalculateAfrDelay(logArray);

            var analysed = analyser.GetCorrection(map);
            float[,] finalCorrection;
            var correctedVeTable = veTableCorrector.TuneVeTable(analysed, veTable, out finalCorrection);

            var maxBoost = log.OrderByDescending(l => l.Map.Value).First();
            Console.WriteLine("Max boost: {0}kpa @ {1}rpm", maxBoost.Map.Value, maxBoost.Rpm.Value);

            var maxClt = log.Max(l => l.Coolant.Value);
            Console.WriteLine("Max clt: {0}° C", maxClt);

            var maxIat = log.Max(l => l.Iat.Value);
            Console.WriteLine("Max IAT: {0}° C", maxIat);

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

            var now = DateTime.Now;
            var filename = ExportDir + "VETable_corrected" + now.ToString("-yyyy-MM-dd-HH-mm-ss") + ".bin";
            File.WriteAllBytes(filename, bytes);

            return;

            var startBytes = logBytes.Take(12).ToArray();

            var timeInSeconds = 100;
            var howManyLines = (samplesPerSec * timeInSeconds);

            var firstLine = logBytes.Skip(12 + 40 * 10).Take(40).ToArray();
            var hex = BitConverter.ToString(firstLine);
            Console.WriteLine(hex);
            Console.WriteLine(Encoding.ASCII.GetChars(firstLine, 0, lineLength));

            var value = BitConverter.ToInt16(firstLine, 0);
            Console.WriteLine(value);

            var result = startBytes;

            var tps = new Tps { Value = 98 };
            var mapVal = new Map { Value = 180};

            //Temps are really rought
            var iat = new Iat { Value = 42f };
            var coolant = new Coolant { Value = 80f };
            var afrCorr = new AfrCorrection() { Value = 15 };

            var afrWideband = new AfrWideband { Value = 16f };
            var accEnr = new AccEnr() { Value = 0 };

            var rpm = BitConverter.GetBytes(1500);

            for (var line = 0; line < howManyLines; line += 1)
            {
                afrWideband.Value = 10 + line * 4 / 1000;
                rpm = BitConverter.GetBytes(line*4);
                try
                {
                    mapVal.Value = line/10;
                    
                }
                catch (Exception)
                {
                    mapVal.Value = 100;
                }
                
                firstLine[LogReader.Index["Tps"]] = tps.Bytes;
                firstLine[LogReader.Index["Map"]] = mapVal.Bytes;
                firstLine[LogReader.Index["Iat"]] = iat.Bytes;
                firstLine[LogReader.Index["Coolant"]] = coolant.Bytes;
                firstLine[LogReader.Index["AfrCorrection"]] = afrCorr.Bytes;
                firstLine[LogReader.Index["AfrWideband"]] = afrWideband.Bytes;
                firstLine[LogReader.Index["AccEnr"]] = accEnr.Bytes;

                //RPM
                firstLine[10] = rpm[0];
                firstLine[11] = rpm[1];


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

            //firstLine[1] = (byte)line;
            //    firstLine[4] = (byte)line;
            //    firstLine[5] = (byte)line;
            //    firstLine[6] = (byte)line;
            //    firstLine[7] = (byte)line;
            //    firstLine[5] = (byte)line;
            //    firstLine[12] = (byte)line;
            //    firstLine[13] = (byte)line;
            //    firstLine[15] = (byte)line;
            //    firstLine[20] = (byte)line;
                result = result.Concat(firstLine).ToArray();
            }

            //File.WriteAllBytes(@"C:\Dev\repos\moje\DET3-FIT-analyzer\Samples\log_mine.dlg", result);
        }
    }
}
