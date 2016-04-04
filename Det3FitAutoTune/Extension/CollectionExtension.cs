using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Det3FitAutoTune.Extension
{
    public static class CollectionExtension
    {
        public static int ClosestToIndex(this int[] collection, float target)
        {
            var minDifference = int.MaxValue;
            var closestIndex = 0;

            for (int i = 0; i < collection.Length; i++)
            {
                var element = collection[i];
                var difference = Math.Abs(element - target);
                if (minDifference > difference)
                {
                    minDifference = (int)difference;
                    closestIndex = i;
                }
            }

            return closestIndex;
        }

        public static uint UpToIndex(this int[] collection, float target)
        {
            for (uint i = 0; i <= collection.Length; i++)
            {
                var element = collection[i];
                if (target < element)
                {
                    uint result = checked (i + 1);
                    return result;
                }
            }

            return 0;
        }
    }
}
