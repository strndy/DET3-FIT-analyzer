namespace Det3FitAutoTune.Model.Value
{
    public class AfrCorrection : AbstractByteField
    {
        protected override byte Bytes1
        {
            get { return 180; }
        }

        protected override float Val1
        {
            get { return -10; }
        }

        protected override byte Bytes2
        {
            get { return 220; }
        }

        protected override float Val2
        {
            get { return 10; }
        }
    }
}
