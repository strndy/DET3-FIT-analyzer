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

        public const float BulharskaKonstanta = 0.9f;

        public float[,] TuneVeTable(ProjectedAfrCorrection[,] corrections, float[,] veTable, out float[,] finalCorrection)
        {
            finalCorrection = new float[16, 16];
            var correctedTable = new float[16, 16];
            Array.Copy(veTable, correctedTable, veTable.Length);

            var maxKpaForCorrection = _mapCoordinates.KpaIndex(92);

            for (int rpmIndex = 0; rpmIndex < 16; rpmIndex++)
            {
                for (int kpaIndex = 0; kpaIndex < 16; kpaIndex++)
                {
                    var corr = corrections[rpmIndex, kpaIndex];
                    if (corr == null) continue;
                    float coeficient = 1;

                    if (kpaIndex <= maxKpaForCorrection)
                    {
                        coeficient = (corr.NboCorrection + (corr.AfrDiffPercent*0.5f))  * GetImportance(corr.Count) * BulharskaKonstanta;
                    }
                    else
                    {
                        coeficient = corr.AfrDiffPercent * BulharskaKonstanta * GetImportance(corr.Count);
                    }

                    //AFR only
                    //coeficient = corr.AfrDiffPercent * BulharskaKonstanta;}}

                    var delta = correctedTable[rpmIndex, kpaIndex]*coeficient/100;
                    finalCorrection[rpmIndex, kpaIndex] = delta;
                    correctedTable[rpmIndex, kpaIndex] += delta;
                }
            }
            return correctedTable;
        }

        private float GetImportance(int count)
        {
            var importance = Math.Log(count, 10) * 0.5 + 0.01;

            if (importance < 0)
            {
                return 0;
            }
            if (importance > 1)
            {
                return 1;
            }

            return (float)importance;
        }
    }
}
