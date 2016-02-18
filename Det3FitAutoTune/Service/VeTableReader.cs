using System;

namespace Det3FitAutoTune.Service
{
    public class VeTableReader
    {
        public float[,] ReadTable(byte[] veTableBytes)
        {
            var veMap = new float[16,16];
            var rpmIndex = 15;
            var kpaIndex = 0;

            for (var i = 0; i < 1024; i+=4)
            {
                //Console.WriteLine("Re: i: {0}, rpm: {1} kpa: {2}", i, rpmIndex, kpaIndex);
                var val = veTableBytes[i];
                veMap[rpmIndex, kpaIndex] = val;
                rpmIndex--;
                if (rpmIndex < 0)
                {
                    rpmIndex = 15;
                    kpaIndex++;
                }
            }

            return veMap;
        }

        public byte[] GetBytes(float[,] veTable)
        {
            var bytes = new byte[1024];

            var index = 0;

            for (int kpaIndex = 0; kpaIndex < 16; kpaIndex++)
            {
                for (int rpmIndex = 15; rpmIndex >= 0; rpmIndex--)
                {
                    //Console.WriteLine("WR: i: {0},  rpm: {1} kpa: {2}", index, rpmIndex, kpaIndex);
                    
                    bytes[index] = (byte)Math.Round(veTable[rpmIndex, kpaIndex]);
                    index += 4;
                }
            }

            return bytes;
        }
    }
}
