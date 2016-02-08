using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Det3FitAutoTune.Model.Value
{
    public class Tps : IField
    {
        public const int Min = 20;
        public const int Max = 150;

        public const float Ratio = byte.MaxValue / (Max - Min);

        private byte _value;

        public int Position
        {
            get { return 0; }
        }

        public float Value
        {
            get
            {
                return (_value / Ratio - Min);
            }
            set
            {
                _value = (byte)Math.Round((value + Min) * Ratio);
            }
        }

        public byte Bytes
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        public void BytesFromLine(byte[] bytes)
        {
            throw new NotImplementedException();
        }
    }
}
