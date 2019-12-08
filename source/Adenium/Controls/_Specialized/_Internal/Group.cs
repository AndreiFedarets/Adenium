using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Adenium.Controls
{
    internal class Group : Element
    {
        public static readonly float SampleAspectRatio;
        public static readonly Range AspectRatioRange;
        public static readonly Range AutoAdjustRange;

        static Group()
        {   
            SampleAspectRatio = 4 / 3;
            AspectRatioRange = new Range(-0.25f, 0.25f);
            AutoAdjustRange = new Range(-0.25f, 0.25f);
        }

        public Group(Element first, Element second)
        {
            First = first;
            Second = second;
        }

        //public Orientation Orientation { get; set; }

        public Element First { get; set; }

        public Element Second { get; set; }

        public int WasteArea
        {
            get { return Area - First.Area - Second.Area; }
        }

        private static Group CalculateOptimalGrouping(Element first, Element second)
        {
            Group horizontal = new HorizontalGroup(first, second);
            Group vertical = new VerticalGroup(first, second);

            //We make that decicion by:
            //1. calculating change of AspectRatio for each option
            //2. calculating of 'waste' area for each option
            
            float horizontalAspectRatioChange = 1 - horizontal.AspectRatio / SampleAspectRatio;
            float verticalAspectRatioChange = 1 - vertical.AspectRatio / SampleAspectRatio;

            if (AspectRatioRange.Matches(horizontalAspectRatioChange) &&
                AspectRatioRange.Matches(verticalAspectRatioChange))
            {
                if (horizontal.WasteArea <= vertical.WasteArea)
                {
                    return horizontal;
                }
                return vertical;
            }
            else if (AspectRatioRange.Matches(horizontalAspectRatioChange))
            {
                return horizontal;
            }
            else if (AspectRatioRange.Matches(verticalAspectRatioChange))
            {
                return vertical;
            }
            else if (horizontalAspectRatioChange <= verticalAspectRatioChange)
            {
                return horizontal;
            }
            else
            {
                return vertical;
            }
        }

        private bool TryAdjustSize()
        {
            Element main;
            Element additional;
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


        public static Group GroupElements(Element first, Element second, List<Element> restElements)
        {
            //Step#1: deside orientation of grouping, where to attach 'second' to 'first' - right or bottom.
            Group group = CalculateOptimalGrouping(first, second);

            //Step#2: Adjust elements size depending on gouping orientation. Height for Horizontal, Width for Vertical
            //Size of elements could be different on the touch edge so we need to make them equal.
            if (group.TryAdjustSize())
            {
                return group;
            }

            //Step#3: If adjustment was not successful, the we need to try to regroup smaller element with something else
            Element smaller;
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

        public static Element GroupElements(List<Element> elements)
        {
            Element main = TakeFirst(elements);
            while (elements.Count > 0)
            {
                Element additional = TakeFirst(elements);
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
