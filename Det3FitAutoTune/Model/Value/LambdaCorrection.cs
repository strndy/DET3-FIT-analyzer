using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Det3FitAutoTune.Model.Value
{
    public class LambdaCorrection : AbstractField
    {
        protected override byte Bytes1
        {
            get { return 180; }
        }

        protected override float Val1
        {
            get { return -10; }
        }

        protected override byte Bytes2
        {
            get { return 220; }
        }

        protected override float Val2
        {
            get { return 10; }
        }

        public override int Index
        {
            get { return 8; }
        }
    }
}
