using System;
using Det3FitAutoTune.Model;

namespace Det3FitAutoTune.Service
{
    public class VeTableCorrector
    {
        public const float BulharskaKonstanta = 0.8f;
        public float[,] TuneVeTable(ProjectedAfrCorrection[,] corrections, float[,] veTable, out float[,] finalCorrection)
        {
            finalCorrection = new float[16, 16];
            var correctedTable = new float[16, 16];
            Array.Copy(veTable, correctedTable, veTable.Length);

            for (int rpmIndex = 1; rpmIndex < 16; rpmIndex++)
            {
                for (int kpaIndex = 1; kpaIndex < 16; kpaIndex++)
                {
                    var corr = corrections[rpmIndex, kpaIndex];
                    if (corr == null) continue;

                    var coeficient = (corr.AfrDiffPercent + corr.NboCorrection) * BulharskaKonstanta;
                    var delta = correctedTable[rpmIndex, kpaIndex] * coeficient / 100;
                    finalCorrection[rpmIndex, kpaIndex] = delta;
                    correctedTable[rpmIndex, kpaIndex] += delta;
                }
            }
            return correctedTable;
        }
    }
}
