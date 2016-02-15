namespace Det3FitAutoTune.Model
{
    public class ProjectedAfrCorrection
    {
        public int Count;

        public float AfrDiff;

        public float NboCorrection;

        public float FinalCorrection
        {
            get { return AfrDiff*-1 + NboCorrection; }  
        }
    }
}
