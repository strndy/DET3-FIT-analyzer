using System;

namespace Det3FitAutoTune.Model.Value
{
    public class Tps : AbstractByteField
    {
        protected override byte Bytes2
        {
            get { return 20; }
        }

        protected override byte Bytes1
        {
            get { return 140; }
        }

        protected override float Val2
        {
            get { return 0; }
        }

        protected override float Val1
        {
            get { return 99; }
        }
    }
}
