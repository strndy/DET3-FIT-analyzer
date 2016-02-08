using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var logBytes = File.ReadAllBytes(@"C:\Dev\repos\moje\DET3-FIT-analyzer\Samples\log_2016131_1555.dlg");

            var startBytes = logBytes.Take(12).ToArray();
            //Console.WriteLine(BitConverter.ToString(startBytes));

            var timeInSeconds = 10;
            var howManyLines = (samplesPerSec * timeInSeconds);

            var firstLine = logBytes.Skip(12 + 40 * 10).Take(40).ToArray();
            string hex = BitConverter.ToString(firstLine);
            Console.WriteLine(hex);
            Console.WriteLine(Encoding.ASCII.GetChars(firstLine, 0, lineLength));

            var value = BitConverter.ToInt16(firstLine, 0);
            Console.WriteLine(value);

            var result = startBytes;

            var tps = new Tps();
            tps.Value = 0;

            for (int line = 0; line < howManyLines; line += 1)
            {
                tps.Bytes = 45;
                firstLine[0] = tps.Bytes;
                if (line % 15 == 0) 
                {
                    if (tps.Value < 100) 
                    { 
                        
                    }
                }

                result = result.Concat(firstLine).ToArray();
            }

            File.WriteAllBytes(@"C:\Dev\repos\moje\DET3-FIT-analyzer\Samples\log_mine.dlg", result);
        }

        private static byte[] ChangeValue(byte[] data)
        {
            throw new Exception();
        }
    }
}
