using System;

namespace Det3FitAutoTune.Model.Value
{
    public abstract class AbstractField : IField
    {
        private static float? _ratio;
        private static float? _offset;

        protected abstract byte MaxByte { get; }
        protected abstract byte MinByte { get; }
        protected abstract float MaxVal { get; }
        protected abstract float MinVal { get; }

        protected byte _bytes;

        protected float Ratio
        {
            get 
            { 
                if(_ratio == null)
                {
                    _ratio = (MaxByte - Offset) / MaxVal;
                }
                return (float)_ratio;
            }
        }

        protected float Offset
        {
            get 
            { 
                if(_offset == null)
                {
                    _offset = (MaxVal * MinByte - MinVal * MaxByte) / (MaxVal - MinVal);
                }
                return (float)_offset;
            }
        }

        public abstract int Position
        {
            get;
        }

        public float Value
        {
            get
            {
                return (_bytes - Offset) / (Ratio);
            }
            set
            {
                _bytes = checked((byte)Math.Round(value * Ratio + Offset));
            }
        }

        public byte Bytes
        {
            get
            {
                return _bytes;
            }
            set
            {
                _bytes = value;
            }
        }

        public void BytesFromLine(byte[] bytes)
        {
            throw new NotImplementedException();
        }
    }
}
