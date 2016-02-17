using System;

namespace Det3FitAutoTune.Model
{
    public class ProjectedAfrCorrection
    {
        public float AvgAfr;

        public int Count;

        public float AfrDiffPercent;
        public float AfrDiffAbsolute;

        public float NboCorrection;

        public float SumValue;

        public float GetVal(AfrCorrectionMethod method)
        {
            switch (method)
            {
                case AfrCorrectionMethod.AfrDiffPercent:
                    return AfrDiffPercent;
                case AfrCorrectionMethod.AfrDiffAbsolute:
                    return AfrDiffAbsolute;
                case AfrCorrectionMethod.AvgAfr:
                    return AvgAfr;
                case AfrCorrectionMethod.Count:
                    return Count;
                case AfrCorrectionMethod.NboCorrection:
                    return NboCorrection;
                case AfrCorrectionMethod.SumValue:
                    return SumValue;
                default:
                    throw new ArgumentOutOfRangeException("method", method, null);
            }
        }

        public enum AfrCorrectionMethod
        {
            AvgAfr,
            Count,
            AfrDiffPercent,
            AfrDiffAbsolute,
            NboCorrection,
            SumValue,
            FinalCorrection
        };
    }
}
