
namespace Det3FitAutoTune.Model.Value
{
    public class AccEnr : AbstractByteField
    {
        protected override byte Bytes1
        {
            get { return 10; }
        }

        protected override float Val1
        {
            get { return 1; }
        }

        protected override byte Bytes2
        {
            get { return 255; }
        }

        protected override float Val2
        {
            get { return (float)25.5; }
        }
    }
}
