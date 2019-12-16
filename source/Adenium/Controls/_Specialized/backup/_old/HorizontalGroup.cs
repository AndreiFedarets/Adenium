using System;

namespace Adenium.Controls
{
    internal sealed class HorizontalGroup : Group
    {
        public HorizontalGroup(Element first, Element second)
            : base(first, second)
        {

        }


        public override int Height
        {
            get { return Math.Max(First.Height, Second.Height); }
            set
            {
                First.Height = value;
                Second.Height = value;
            }
        }

        public override int Width
        {
            get { return First.Width + Second.Width; }
            set
            {
                float percent = value / Width;
                First.Width = (int)(First.Width * percent);
                Second.Width = value - First.Width;
            }
        }
    }
}
