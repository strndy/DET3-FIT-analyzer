using System;
using Det3FitAutoTune.Model;

namespace Det3FitAutoTune.Service
{
    public class VeTableCorrector
    {
        private readonly MapCoordinates _mapCoordinates;

        public VeTableCorrector(MapCoordinates mapCoordinates)
        {
            _mapCoordinates = mapCoordinates;
        }

        public const float BulharskaKonstanta = 0.8f;
        public float[,] TuneVeTable(ProjectedAfrCorrection[,] corrections, float[,] veTable, out float[,] finalCorrection)
        {
            finalCorrection = new float[16, 16];
            var correctedTable = new float[16, 16];
            Array.Copy(veTable, correctedTable, veTable.Length);

            var maxKpaForCorrection = _mapCoordinates.KpaIndex(92);

            for (int rpmIndex = 1; rpmIndex < 16; rpmIndex++)
            {
                for (int kpaIndex = 1; kpaIndex < 16; kpaIndex++)
                {
                    var corr = corrections[rpmIndex, kpaIndex];
                    if (corr == null) continue;
                    float coeficient = 1;

                    if (kpaIndex <= maxKpaForCorrection)
                    {
                        coeficient = (corr.NboCorrection + (corr.AfrDiffPercent * 0.3f)) * BulharskaKonstanta;
                    }
                    else
                    {
                        coeficient = corr.AfrDiffPercent * BulharskaKonstanta;
                    }

                    //AFR only
                    //coeficient = corr.AfrDiffPercent * BulharskaKonstanta;}}

                    var delta = correctedTable[rpmIndex, kpaIndex] * coeficient / 100;
                    finalCorrection[rpmIndex, kpaIndex] = delta;
                    correctedTable[rpmIndex, kpaIndex] += delta;
                }
            }
            return correctedTable;
        }
    }
}
