using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Det3FitAutoTune.Service;

namespace Det3FitAutoTune
{
    class Program
    {
        static void Main(string[] args)
        {
            var logReader = new LogReader();
            //var logBytes = File.ReadAllBytes(@"C:\Dev\repos\moje\DET3-FIT-analyzer\Samples\log_2016131_1613_short.dlg");
            var logBytes = File.ReadAllBytes(@"C:\Dev\repos\moje\DET3-FIT-analyzer\Samples\log_2016131_1555.dlg");

            var count = 0;
            for (int i = 16; i < logBytes.Length; i++)
            {
                
                Console.Write(Encoding.ASCII.GetChars(logBytes, i, 1));
                count++;
                if (count%40 == 0)
                {
                    Console.WriteLine();
                }
            }
            var len = logBytes.Length;
            //var log = logReader.GetLogLine(logBytes);
            var resized = logBytes;
            Array.Resize(ref resized, 40*100 + 12);
            File.WriteAllBytes(@"C:\Dev\repos\moje\DET3-FIT-analyzer\Samples\log_2016131_1555_mine.dlg", resized);


        }
    }
}
