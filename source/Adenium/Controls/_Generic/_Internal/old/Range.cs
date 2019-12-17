namespace Adenium.Controls
{
    internal sealed class Range
    {
        public Range(double min, double max)
        {
            Min = min;
            Max = max;
        }

        public double Min { get; private set; }

        public double Max { get; private set; }
    }
}
