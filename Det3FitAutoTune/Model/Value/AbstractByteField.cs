using System;
using System.Collections.Generic;

namespace Det3FitAutoTune.Model.Value
{
    public abstract class AbstractByteField : IField<byte>
    {
        private static readonly IDictionary<Type, float?> _ratio = new Dictionary<Type, float?>();
        private static readonly IDictionary<Type, float?> _offset = new Dictionary<Type, float?>();

        protected abstract byte Bytes1 { get; }
        protected abstract float Val1 { get; }

        protected abstract byte Bytes2 { get; }
        protected abstract float Val2 { get; }

        protected byte _bytes;

        protected float Ratio
        {
            get 
            {
                if (!_ratio.ContainsKey(this.GetType()))
                {
                    _ratio[this.GetType()] = (Bytes1 - Offset) / Val1;
                }
                return (float)_ratio[this.GetType()];
            }
        }

        protected float Offset
        {
            get 
            {
                if (!_offset.ContainsKey(this.GetType()))
                {
                    _offset[this.GetType()] = (Val1 * Bytes2 - Val2 * Bytes1) / (Val1 - Val2);
                }
                return (float)_offset[this.GetType()];
            }
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
    }
}
