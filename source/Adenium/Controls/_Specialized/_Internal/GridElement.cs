namespace Adenium.Controls
{
    internal class GridElement
    {
        public GridElement(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public GridElement()
        {

        }

        public virtual int Height { get; set; }

        public virtual int Width { get; set; }

        public int Area
        {
            get { return  Width * Height; }
        }

        public float AspectRatio
        {
            get { return Width / Height; }
        }

    }
}
