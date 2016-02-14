namespace Det3FitAutoTune.Model.Value
{
    public class Rpm : IField<ushort>
    {
        public float Value
        {
            get
            {
                return (float) Bytes;
            }
            set
            {
                Bytes = (ushort)value;
            }
        }
        public ushort Bytes { get; set; }
    }
}
