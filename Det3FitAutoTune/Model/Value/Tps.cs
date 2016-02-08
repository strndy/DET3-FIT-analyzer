using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Det3FitAutoTune.Model.Value
{
    public class Tps : IField
    {
        public const byte MinByte = 20;
        public const byte MaxByte = 150;

        public const float MinVal = 0;
        public const float MaxVal = 100;

        public const float Offset = (MaxVal * MinByte - MinVal * MaxByte) / (MaxVal - MinVal);

        public const float Ratio = (MaxByte - Offset) / MaxVal;

        protected byte _bytes;

        public int Position
        {
            get { return 0; }
        }

        public float Value
        {
            get
            {
                return (_bytes - Offset) / (Ratio);
            }
            set
            {
                _bytes = checked((byte) Math.Round(value * Ratio + Offset));
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
