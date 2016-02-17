
namespace Det3FitAutoTune.Model.Value
{
    public class AfrWideband : AbstractByteField
    {
        protected override byte Bytes1
        {
            get { return 0; }
        }

        protected override float Val1
        {
            get { return 7.35f; }
        }

        protected override byte Bytes2
        {
            get { return 255; }
        }

        protected override float Val2
        {
            get { return 22.39f; }
        }
    }
}
