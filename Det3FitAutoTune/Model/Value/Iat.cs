using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Det3FitAutoTune.Model.Value
{
    public class Iat : AbstractField
    {
        protected override byte Bytes1
        {
            get { return 80; }
        }

        protected override byte Bytes2
        {
            get { return 200; }
        }

        protected override float Val1
        {
            get { return 23; }
        }

        protected override float Val2
        {
            get { return 92; }
        }

        public override int Index
        {
            get { return 3; }
        }


    }
}
