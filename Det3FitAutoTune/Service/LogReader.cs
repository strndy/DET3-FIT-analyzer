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

        private readonly IDictionary<string, int> _index = new Dictionary<string, int>()
        {
            {"AccEnr", 18},
            {"AfrWideband", 9},
            {"AfrCorrection", 8},
            {"Coolant", 2},
            {"Iat", 3},
            {"Map", 1},
            {"Tps", 0},
            {"Rpm", 10},
        };

        public IEnumerable<LogLine> ReadLog(byte[] fileContent)
        {
            var result = new List<LogLine>();

            var howManyLines = (fileContent.Length - Header.Length) / LineLength;

            var copyArray = new byte[40];

            for (var lineNo = 0; lineNo < howManyLines; lineNo += 1)
            {
                var skip = Header.Length + lineNo * LineLength;
                //if slow, change to array.Copy()

                Array.Copy(fileContent, skip, copyArray, 0, 40);
                //var lineBytes = fileContent.Skip(skip).Take(LineLength).ToArray();
                var line = GetLogLine(copyArray);
                result.Add(line);
            }
            return result;

        }

        private LogLine GetLogLine(byte[] line)
        {
            var logLine = new LogLine()
            {
                Map = new Map() { Bytes = line[_index["Map"]]},
                AccEnr = new AccEnr() { Bytes = line[_index["AccEnr"]] },
                AfrCorrection = new AfrCorrection() { Bytes = line[_index["AfrCorrection"]] },
                AfrWideband = new AfrWideband() { Bytes = line[_index["AfrWideband"]] },
                Coolant = new Coolant() { Bytes = line[_index["Coolant"]] },
                Iat = new Iat() { Bytes = line[_index["Iat"]] },
                Tps = new Tps() { Bytes = line[_index["Tps"]] },

                Rpm = new Rpm() { Bytes = BitConverter.ToUInt16(line, _index["Rpm"]) },
            };
            return logLine;
        }

    }
}
