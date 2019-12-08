using System;

namespace Adenium.Controls
{
    internal sealed class Range
    {
        public Range(float left, float right)
        {
            Left = left;
            Right = right;
        }

        public float Left { get; private set; }

        public float Right { get; private set; }

        public bool Matches(float value)
        {
            return value >= Left && value <= Right;
        }
    }
}
