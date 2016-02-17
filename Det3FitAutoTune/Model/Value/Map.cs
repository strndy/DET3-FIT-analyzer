namespace Det3FitAutoTune.Model.Value
{
    public class Map : AbstractByteField
    {
        protected override byte Bytes1
        {
            get { return 30; }
        }

        protected override float Val1
        {
            get { return 49; }
        }

        protected override byte Bytes2
        {
            get { return 200; }
        }

        protected override float Val2
        {
            get { return 216; }
        }

        /// <summary>
        /// 0 = -6 kpa
        /// 65535 (ushort.MaxVal) = 253 
        /// </summary>

    }
}
