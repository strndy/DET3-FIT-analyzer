using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Det3FitAutoTune.Model.Value
{
    public abstract class AbstractField
    {
        public const float Offset = (MaxVal * MinByte - MinVal * MaxByte) / (MaxVal - MinVal);

        public const float Ratio = (MaxByte - Offset) / MaxVal;

        public float Ratio
        {
            get { return ushort.MaxValue / (MaxValue + Offset); }
        }

        public abstract double MaxValue 
        {
            get;
        }
        public abstract double Offset
        {
            get;
        }

        public abstract int Position
        {
            get;
        }

        protected byte _value;

        public int Short
        {
            get { return _value; }
        }

        public float Value 
        { 
            get 
            { 
                return (float) (_value / Ratio + Offset);
            } 
            set 
            { 
                _value = (byte)Math.Round((value - Offset) * Ratio);
            } 
        }

        public byte Bytes 
        { 
            get 
            {
                return _value;// BitConverter.GetBytes(_value);
            } 
            set 
            {
                _value = value;
            } 
        }
    }
}
