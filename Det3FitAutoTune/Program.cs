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
        public const int samplesPerSec = 30;
        public const int lineLength = 40;

        static void Main(string[] args)
        {
            var logReader = new LogReader();
            var logBytes = File.ReadAllBytes(@"C:\Dev\repos\moje\DET3-FIT-analyzer\Samples\log_201627_1039_straight_start.dlg");

            var startBytes = logBytes.Take(12).ToArray();
            Console.WriteLine(BitConverter.ToString(startBytes));

            var timeInSeconds = 10;
            var howManyBytes = (samplesPerSec * lineLength * timeInSeconds);

            var shortened = logBytes.Skip(12).Take(howManyBytes).ToArray();


            var total = (shortened.Length)/40;
            Console.WriteLine("total lines {0}", total);

            for (int i = 0; i < shortened.Length; i += lineLength)
            {
                var newArray = shortened.Skip(i).Take(40).ToArray();
                
                string hex = BitConverter.ToString(newArray);

                Console.WriteLine(hex);

                Console.WriteLine(Encoding.ASCII.GetChars(shortened, i, lineLength));
            }

            var result = startBytes.Concat(shortened).ToArray();
            File.WriteAllBytes(@"C:\Dev\repos\moje\DET3-FIT-analyzer\Samples\log_mine.dlg", result);
        }

        private static byte[] ChangeValue(byte[] data)
        {
            throw new Exception();
        }
    }
}
