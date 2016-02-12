namespace Det3FitAutoTune.Model.Value
{
    public class Map : AbstractField
    {
        protected override byte Bytes1
        {
            get { return 50; }
        }

        protected override float Val1
        {
            get { return 43; }
        }

        protected override byte Bytes2
        {
            get { return 200; }
        }

        protected override float Val2
        {
            get { return 190; }
        }

        public override int Index
        {
            get { return 1; }
        }

        /// <summary>
        /// 0 = -6 kpa
        /// 65535 (ushort.MaxVal) = 253 
        /// </summary>

    }
}
