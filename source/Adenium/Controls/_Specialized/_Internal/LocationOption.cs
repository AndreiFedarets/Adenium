using System;
using System.Windows.Controls;

namespace Adenium.Controls
{
    internal class LocationOption
    {
        public static readonly float SampleAspectRatio;
        public static readonly Range AspectRatioRange;
        public static readonly Range AutoAdjustRange;

        static LocationOption()
        {
            SampleAspectRatio = 4 / 3;
            AspectRatioRange = new Range(-0.25f, 0.25f);
            AutoAdjustRange = new Range(-0.25f, 0.25f);
        }

        public LocationOption(GridElement first, GridElement second, Orientation orientation)
        {
            First = first;
            Second = second;
            Orientation = orientation;
        }

        public GridElement First { get; private set; }

        public GridElement Second { get; private set; }

        public Orientation Orientation { get; private set; }

        public int GroupHeight
        {
            get
            {
                switch (Orientation)
                {
                    case Orientation.Horizontal:
                        return Math.Max(First.Height, Second.Height);
                    case Orientation.Vertical:
                        return First.Height + Second.Height;
                    default:
                        throw new NotSupportedException();
                }
            }
        }
        public int GroupWidth
        {
            get
            {
                switch (Orientation)
                {
                    case Orientation.Horizontal:
                        return First.Width + Second.Width;
                    case Orientation.Vertical:
                        return Math.Max(First.Width, Second.Width);
                    default:
                        throw new NotSupportedException();
                }
            }
        }

        public float AspectRatio
        {
            get { return GroupWidth / GroupHeight; }
        }

        public float AspectRatioChange
        {
            get { return 1 - AspectRatio / SampleAspectRatio; }
        }

        private int GetTouchSideSize(GridElement element)
        {
            switch (Orientation)
            {
                case Orientation.Horizontal:
                    return element.Height;
                case Orientation.Vertical:
                    return element.Width;
            }
            return 0;
        }

        private void SetTouchSideSize(GridElement element, int size)
        {
            switch (Orientation)
            {
                case Orientation.Horizontal:
                    element.Height = size;
                    break;
                case Orientation.Vertical:
                    element.Width = size;
                    break;
            }
        }

        public bool TryAdjustSize()
        {
            GridElement main = GetMainElement();
            GridElement additional = GetAdditionalElement();
            int absoluteAdjustment = GetTouchSideSize(main) - GetTouchSideSize(additional);
            float relativeAdjustment = absoluteAdjustment / GetTouchSideSize(additional);
            if (AutoAdjustRange.Matches(relativeAdjustment))
            {
                SetTouchSideSize(additional, GetTouchSideSize(main));
                return true;
            }
            return false;
        }

        private GridElement GetMainElement()
        {
            return First.Area >= Second.Area ? First : Second;
        }

        private GridElement GetAdditionalElement()
        {
            return First.Area < Second.Area ? First : Second;
        }

        private int CalculateWasteArea()
        {
            //area('waste')  =  area(group) - area(first) - area(second)
            int groupArea = GroupWidth * GroupHeight;
            int wasteArea = groupArea - First.Area - Second.Area;
            return wasteArea;
        }

        public static LocationOption SelectOptimal(LocationOption horizontalOption, LocationOption verticalOption)
        {
            float horizontalAspectRatioChange = horizontalOption.AspectRatioChange;
            float verticalAspectRatioChange = verticalOption.AspectRatioChange;

            if (AspectRatioRange.Matches(horizontalAspectRatioChange) &&
                AspectRatioRange.Matches(verticalAspectRatioChange))
            {
                if (horizontalOption.CalculateWasteArea() <= verticalOption.CalculateWasteArea())
                {
                    return horizontalOption;
                }
                return verticalOption;
            }
            else if (AspectRatioRange.Matches(horizontalAspectRatioChange))
            {
                return horizontalOption;
            }
            else if (AspectRatioRange.Matches(verticalAspectRatioChange))
            {
                return verticalOption;
            }
            else if (horizontalAspectRatioChange <= verticalAspectRatioChange)
            {
                return horizontalOption;
            }
            else
            {
                return verticalOption;
            }
        }
    }
}
