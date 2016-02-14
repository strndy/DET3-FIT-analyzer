namespace Det3FitAutoTune.Model.Value
{
    public class Iat : AbstractByteField
    {
        protected override byte Bytes1
        {
            get { return 80; }
        }

        protected override byte Bytes2
        {
            get { return 200; }
        }

        protected override float Val1
        {
            get { return 23; }
        }

        protected override float Val2
        {
            get { return 92; }
        }
    }
}
