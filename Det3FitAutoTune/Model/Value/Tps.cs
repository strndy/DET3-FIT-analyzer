using System;

namespace Det3FitAutoTune.Model.Value
{
    public class Tps : AbstractField
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

        public override int Index
        {
            get { return 0; }
        }

        public void BytesFromLine(byte[] bytes)
        {
            throw new NotImplementedException();
        }
    }
}
