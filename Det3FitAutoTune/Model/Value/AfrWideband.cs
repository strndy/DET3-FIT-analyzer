
namespace Det3FitAutoTune.Model.Value
{
    public class AfrWideband : AbstractField
    {
        protected override byte Bytes1
        {
            get { return 0; }
        }

        protected override float Val1
        {
            get { return 10; }
        }

        protected override byte Bytes2
        {
            get { return 255; }
        }

        protected override float Val2
        {
            get { return 20; }
        }

        public override int Index
        {
            get { return 9; }
        }
    }
}
