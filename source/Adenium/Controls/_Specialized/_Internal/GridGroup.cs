using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Adenium.Controls
{
    internal sealed class GridGroup : GridElement
    {
        public static readonly float SampleAspectRatio;
        public static readonly Range AspectRatioRange;
        public static readonly Range AutoAdjustRange;

        static GridGroup()
        {
            SampleAspectRatio = 4 / 3;
            AspectRatioRange = new Range(-0.25f, 0.25f);
            AutoAdjustRange = new Range(-0.25f, 0.25f);
        }

        public GridGroup(GridElement first, GridElement second, Orientation orientation)
        {
            First = first;
            Second = second;
            Orientation = orientation;
        }

        public Orientation Orientation { get; set; }

        public GridElement First { get; set; }

        public GridElement Second { get; set; }

        public override int Height
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
            set
            {
                switch (Orientation)
                {
                    case Orientation.Horizontal:
                        First.Height = value;
                        Second.Height = value;
                        break;
                    case Orientation.Vertical:
                        float percent = value / Height;
                        First.Height = (int)(First.Height * percent);
                        Second.Height = value - First.Height;
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
        }

        public override int Width
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
            set
            {
                switch (Orientation)
                {
                    case Orientation.Horizontal:
                        float percent = value / Width;
                        First.Width = (int)(First.Width * percent);
                        Second.Width = value - First.Width;
                        break;
                    case Orientation.Vertical:
                        First.Width = value;
                        Second.Width = value;
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
        }

        public int WasteArea
        {
            get { return Area - First.Area - Second.Area; }
        }

        private static GridGroup CalculateOptimalGrouping(GridElement first, GridElement second)
        {
            GridGroup horizontalGroup = new GridGroup(first, second, Orientation.Horizontal);
            GridGroup verticalGroup = new GridGroup(first, second, Orientation.Vertical);

            //We make that decicion by:
            //1. calculating change of AspectRatio for each option
            //2. calculating of 'waste' area for each option and then select option with min 'waste' area
            
            float horizontalAspectRatioChange = 1 - horizontalGroup.AspectRatio / SampleAspectRatio;
            float verticalAspectRatioChange = 1 - verticalGroup.AspectRatio / SampleAspectRatio;

            if (AspectRatioRange.Matches(horizontalAspectRatioChange) &&
                AspectRatioRange.Matches(verticalAspectRatioChange))
            {
                if (horizontalGroup.WasteArea <= verticalGroup.WasteArea)
                {
                    return horizontalGroup;
                }
                return verticalGroup;
            }
            else if (AspectRatioRange.Matches(horizontalAspectRatioChange))
            {
                return horizontalGroup;
            }
            else if (AspectRatioRange.Matches(verticalAspectRatioChange))
            {
                return verticalGroup;
            }
            else if (horizontalAspectRatioChange <= verticalAspectRatioChange)
            {
                return horizontalGroup;
            }
            else
            {
                return verticalGroup;
            }
        }

        private bool TryAdjustSize()
        {
            GridElement main;
            GridElement additional;
            if (First.Area >= Second.Area)
            {
                main = First;
                additional = Second;
            }
            else
            {
                main = Second;
                additional = First;
            }
            switch (Orientation)
            {
                case Orientation.Horizontal:
                    {
                        int adjustment = main.Height - additional.Height;
                        float relativeAdjustment = adjustment / additional.Height;
                        if (AutoAdjustRange.Matches(relativeAdjustment))
                        {
                            additional.Height = main.Height;
                            return true;
                        }
                    }
                    break;
                case Orientation.Vertical:
                    {
                        int adjustment = main.Width - additional.Width;
                        float relativeAdjustment = adjustment / additional.Width;
                        if (AutoAdjustRange.Matches(relativeAdjustment))
                        {
                            additional.Width = main.Width;
                            return true;
                        }
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
            return false;
        }


        public static GridGroup GroupElements(GridElement first, GridElement second, List<GridElement> restElements)
        {
            //Step#1: deside orientation of grouping, where to attach 'second' to 'first' - right or bottom.
            GridGroup group = CalculateOptimalGrouping(first, second);

            //Step#2: Adjust elements size depending on gouping orientation. Height for Horizontal, Width for Vertical
            //Size of elements could be different on the touch edge so we need to make them equal.
            if (group.TryAdjustSize())
            {
                return group;
            }

            //Step#3: If adjustment was not successful, the we need to try to regroup smaller element with something else
            GridElement smaller;
            Orientation targetOrientation;
            switch (group.Orientation)
            {
                case Orientation.Horizontal:
                    smaller = group.First.Height < group.Second.Height ? group.First : group.Second;
                    targetOrientation = Orientation.Vertical;
                    break;
                case Orientation.Vertical:
                    smaller = group.First.Width < group.Second.Width ? group.First : group.Second;
                    targetOrientation = Orientation.Horizontal;
                    break;
                default:
                    throw new NotSupportedException();
            }

        }

        public static GridElement GroupElements(List<GridElement> elements)
        {
            GridElement main = TakeFirst(elements);
            while (elements.Count > 0)
            {
                GridElement additional = TakeFirst(elements);
                main = GroupElements(main, additional, elements);
            }
            return main;
        }

        private static T TakeFirst<T>(List<T> list)
        {
            T first = list[0];
            list.RemoveAt(0);
            return first;
        }
    }
}
