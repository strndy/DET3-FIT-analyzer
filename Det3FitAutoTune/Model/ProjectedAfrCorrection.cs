using System;

namespace Det3FitAutoTune.Model
{
    public class ProjectedAfrCorrection
    {
        public float AvgAfr;

        public int Count;

        public float AfrDiff;

        public float NboCorrection;

        public float SumValue;

        public float FinalCorrection
        {
            get { return AfrDiff*-1 + NboCorrection; }  
        }

        public float GetVal(AfrCorrectionMethod method)
        {
            switch (method)
            {
                case AfrCorrectionMethod.AfrDiff:
                    return AfrDiff;
                case AfrCorrectionMethod.AvgAfr:
                    return AvgAfr;
                case AfrCorrectionMethod.Count:
                    return Count;
                case AfrCorrectionMethod.NboCorrection:
                    return NboCorrection;
                case AfrCorrectionMethod.SumValue:
                    return SumValue;
                case AfrCorrectionMethod.FinalCorrection:
                    return FinalCorrection;
                default:
                    throw new ArgumentOutOfRangeException("method", method, null);
            }
        }

        public enum AfrCorrectionMethod
        {
            AvgAfr,
            Count,
            AfrDiff,
            NboCorrection,
            SumValue,
            FinalCorrection
        };
    }
}
