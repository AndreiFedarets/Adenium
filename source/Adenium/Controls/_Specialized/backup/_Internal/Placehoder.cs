namespace Adenium.Controls
{
    internal sealed class Placehoder
    {
        public Placehoder(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public int X { get; private set; }

        public int Y { get; private set; }

        public int Width { get; set; }

        public int Height { get; set; }
    }
}
