using System;

namespace Det3FitAutoTune.Model.Value
{
    public class Tps : AbstractField
    {
        protected override byte MinByte
        {
            get { return 20; }
        }

        protected override byte MaxByte
        {
            get { return 150; }
        }

        protected override float MinVal
        {
            get { return 0; }
        }

        protected override float MaxVal
        {
            get { return 100; }
        }

        public override int Position
        {
            get { return 0; }
        }

        public void BytesFromLine(byte[] bytes)
        {
            throw new NotImplementedException();
        }
    }
}
