using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Det3FitAutoTune.Model.Value
{
    public interface IField
    {
        float Value
        {
            get;
            set;
        }

        int Position
        {
            get;
        }

        byte Bytes
        {
            get;
            set;
        }

        void BytesFromLine(byte[] bytes);
    }
}
