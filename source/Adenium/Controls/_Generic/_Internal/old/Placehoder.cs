namespace Adenium.Controls
{
    internal sealed class Placehoder
    {
        public Placehoder(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public double X { get; private set; }

        public double Y { get; private set; }

        public double Width { get; set; }

        public double Height { get; set; }
    }
}
