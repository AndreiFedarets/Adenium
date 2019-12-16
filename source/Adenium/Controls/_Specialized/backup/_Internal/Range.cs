using System;

namespace Adenium.Controls
{
    internal sealed class Range
    {
        public Range(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public float Min { get; private set; }

        public float Max { get; private set; }
    }
}
