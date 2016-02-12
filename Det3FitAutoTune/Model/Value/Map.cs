namespace Det3FitAutoTune.Model.Value
{
    public class Map : AbstractField
    {
        protected override byte MaxByte
        {
            get { return 250; }
        }

        protected override float MaxVal
        {
            get { return 238; }
        }

        protected override byte MinByte
        {
            get { return 10; }
        }

        protected override float MinVal
        {
            get { return 4; }
        }

        public override int Position
        {
            get { return 1; }
        }

        /// <summary>
        /// 0 = -6 kpa
        /// 65535 (ushort.MaxVal) = 253 
        /// </summary>

    }
}
