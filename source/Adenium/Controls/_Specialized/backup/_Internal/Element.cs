using System;

namespace Adenium.Controls
{
    internal class Element
    {
        public static Range PossibleAdjustment;

        static Element()
        {
            PossibleAdjustment = new Range(0.8f, 1.2f);
        }

        public Element(int width, int height, object content)
        {
            Width = width;
            Height = height;
            Content = content;
        }

        public object Content { get; private set; }

        public int Height { get; private set; }

        public int Width { get; private set; }

        public int Left { get; private set; }

        public int Right { get; private set; }

        public int Top { get; private set; }

        public int Bottom { get; private set; }

        public int Area
        {
            get { return  Width * Height; }
        }

        private bool Fit(Placehoder placeholder)
        {
            if (placeholder.Width < Width)
            {
                int minWidth = (int)(PossibleAdjustment.Min * Width);
                if (placeholder.Width < minWidth)
                {
                    return false;
                }
            }
            if (placeholder.Height < Height)
            {
                int minHeight = (int)(PossibleAdjustment.Min * Height);
                if (placeholder.Height < minHeight)
                {
                    return false;
                }
            }
            return true;
        }

        public bool Apply(Placehoder placeholder)
        {
            if (!Fit(placeholder))
            {
                return false;
            }
            AdjustWidth(placeholder);
            AdjustHeight(placeholder);
            Left = placeholder.X;
            Right = Left + Width;
            Top = placeholder.Y;
            Bottom = Top + Height;
            return true;
        }

        private void AdjustWidth(Placehoder placeholder)
        {
            //compact element width to placeholder if needed
            if (Width > placeholder.Width)
            {
                Width = placeholder.Width;
            }
            //stretch element width to placeholder if possible
            else
            {
                int maxWidth = (int)(PossibleAdjustment.Max * Width);
                if (maxWidth > placeholder.Width)
                {
                    Width = placeholder.Width;
                }
            }
        }

        private void AdjustHeight(Placehoder placeholder)
        {
            //compact element height to placeholder if needed
            if (Height > placeholder.Height)
            {
                Height = placeholder.Height;
            }
            //stretch element height to placeholder if possible
            else
            {
                int maxHeight = (int)(PossibleAdjustment.Max * Height);
                if (maxHeight > placeholder.Height)
                {
                    Height = placeholder.Height;
                }
            }
        }

        public Element Clone()
        {
            return new Element(Width, Height) { Top = Top, Right = Right, Bottom = Bottom, Left = Left };
        }
    }
}
