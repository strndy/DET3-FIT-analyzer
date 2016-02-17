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
    }
}
