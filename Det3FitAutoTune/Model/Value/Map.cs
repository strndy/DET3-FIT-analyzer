using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Det3FitAutoTune.Model.Value
{
    public class Map : AbstractField
    {
        public override double MaxValue
        {
            get { return 253; }
        }
        public override double Offset
        {
            get { return 6; }
        }

        public override int Position
        {
            get { return 0; }
        }

        /// <summary>
        /// 0 = -6 kpa
        /// 65535 (ushort.MaxVal) = 253 
        /// </summary>

    }
}
