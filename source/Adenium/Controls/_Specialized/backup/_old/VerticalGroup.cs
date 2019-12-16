using System;

namespace Adenium.Controls
{
    internal sealed class VerticalGroup : Group
    {
        public VerticalGroup(Element first, Element second)
            : base(first, second)
        {

        }

        public override int Height
        {
            get { return First.Height + Second.Height; }
            set
            {
                float percent = value / Height;
                First.Height = (int)(First.Height * percent);
                Second.Height = value - First.Height;
            }
        }

        public override int Width
        {
            get { return Math.Max(First.Width, Second.Width); }
            set
            {
                First.Width = value;
                Second.Width = value;
            }
        }
    }
}
