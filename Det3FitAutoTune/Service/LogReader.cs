using System;
using System.Collections.Generic;
using System.Linq;
using Det3FitAutoTune.Model;
using Det3FitAutoTune.Model.Value;

namespace Det3FitAutoTune.Service
{
    public class LogReader
    {
        public const int SamplesPerSec = 30;
        public const int LineLength = 40;

        public readonly byte[] Header = { 106, 100, 97, 103, 2, 0, 0, 0, 145, 9, 0, 0 };

        public static readonly IDictionary<string, int> Index = new Dictionary<string, int>()
        {
            {"Tps", 0},
            {"Map", 1},
            {"Coolant", 2},
            {"Iat", 3},



            {"AfrCorrection", 8},
            {"AfrWideband", 9},
            {"Rpm", 10},
            //Rpm 10-11

            // ignnition error 14

            //Ase 16
            //Ign 17
            {"AccEnr", 18},
            // acc voltage 19
        };

        public LogLine[] ReadLog(byte[] fileContent)
        {
            var howManyLines = (fileContent.Length - Header.Length) / LineLength;
            var result = new LogLine[howManyLines];

            var copyArray = new byte[40];

            for (var lineNo = 0; lineNo < howManyLines; lineNo += 1)
            {
                var skip = Header.Length + lineNo * LineLength;
                //if slow, change to array.Copy()

                Array.Copy(fileContent, skip, copyArray, 0, 40);
                //var lineBytes = fileContent.Skip(skip).Take(LineLength).ToArray();
                var line = GetLogLine(copyArray);
                result[lineNo] = line;
            }
            return result;

        }

        private LogLine GetLogLine(byte[] line)
        {
            var logLine = new LogLine()
            {
                Map = new Map() { Bytes = line[Index["Map"]]},
                AccEnr = new AccEnr() { Bytes = line[Index["AccEnr"]] },
                AfrCorrection = new AfrCorrection() { Bytes = line[Index["AfrCorrection"]] },
                AfrWideband = new AfrWideband() { Bytes = line[Index["AfrWideband"]] },
                Coolant = new Coolant() { Bytes = line[Index["Coolant"]] },
                Iat = new Iat() { Bytes = line[Index["Iat"]] },
                Tps = new Tps() { Bytes = line[Index["Tps"]] },

                Rpm = new Rpm() { Bytes = BitConverter.ToUInt16(line, Index["Rpm"]) },
            };
            return logLine;
        }

    }
}
