using System;

namespace Det3FitAutoTune.Model
{
    public class ProjectedAfrCorrection
    {
        public float AvgAfr;

        public int Count;

        public float AfrDiffPercent;
        //{
        //    get { return AfrDiffAbsolute * 100; }
        //}

        public float AfrDiffAbsolute;

        public float NboCorrection;

        public float AvgRpm;
        public float AvgKpa;


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
                case AfrCorrectionMethod.AvgKpa:
                    return AvgKpa;
                case AfrCorrectionMethod.AvgRpm:
                    return AvgRpm;
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
            FinalCorrection,
            AvgKpa,
            AvgRpm,
        };
    }
}
