using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Adenium.Controls
{
    public class AutoPanel : Panel
    {
        private static readonly Range PossibleAdjustment;
        public static DependencyProperty AreaProperty;

        private readonly List<UIElement> _measuredElements;
        private readonly List<Rect> _placeholders;

        static AutoPanel()
        {
            PossibleAdjustment = new Range(0.75f, 1.25f);
            AreaProperty = DependencyProperty.RegisterAttached("Area", typeof(Rect), typeof(AutoPanel), new PropertyMetadata(default(Rect)));
        }

        public AutoPanel()
        {
            _measuredElements = new List<UIElement>();
            _placeholders = new List<Rect>();
        }

        public static Rect GetAreaProperty(DependencyObject element)
        {
            return (Rect)element.GetValue(AreaProperty);
        }

        public static void SetAreaProperty(DependencyObject element, Rect value)
        {
            element.SetValue(AreaProperty, value);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (UIElement element in InternalChildren)
            {
                Rect area = GetAreaProperty(element);
                element.Arrange(new Rect(area.Location, area.Size));
            }
            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            _measuredElements.Clear();
            foreach (UIElement element in InternalChildren)
            {
                if (element != null)
                {
                    MeasureElement(element, availableSize);
                }
            }
            return availableSize;
        }

        private void MeasureElement(UIElement element, Size availableSize)
        {
            element.Measure(availableSize);
            double minimumFreeArea = double.MaxValue;
            Rect desiredArea = default(Rect);
            foreach (Rect placeholder in GetPlaceholders(availableSize))
            {
                Rect elementArea = MeasureElement(element, placeholder);
                if (elementArea == default(Rect))
                {
                    continue;
                }
                //TODO: handle also best fit to placeholder
                double freeArea = CalculateFreeArea(elementArea, availableSize);
                if (freeArea < minimumFreeArea)
                {
                    desiredArea = elementArea;
                }
            }
            if (desiredArea == default(Rect))
            {
                throw new InvalidOperationException();
            }
            element.Measure(desiredArea.Size);
            SetAreaProperty(element, desiredArea);
        }

        private IEnumerable<Rect> GetPlaceholders(Size availableSize)
        {
            if (_measuredElements.Count == 0)
            {
                yield return new Rect(new Point(0, 0), availableSize));
            }
            Size areaSize = default(Size);
            foreach (UIElement element in _measuredElements)
            {
                Rect elementArea = GetAreaProperty(element);

                //check top right placeholder
                Point topRight = elementArea.TopRight;
                //
                yield return new Rect(topRight, areaSize);

                //check bottom left placeholder
                Point bottomLeft = elementArea.BottomLeft;
                //
                yield return new Rect(bottomLeft, areaSize);
            }
        }

        private double CalculateFreeArea(Rect candidateArea, Size availableSize)
        {
            //calculate actual width and height of elements and summ of their areas
            double elementsWidth = candidateArea.Right;
            double elementsHeight = candidateArea.Bottom;
            double elementsArea = candidateArea.Width * candidateArea.Height;
            foreach (UIElement element in _measuredElements)
            {
                Rect elementArea = GetAreaProperty(element);
                if (elementArea.Right > elementsWidth)
                {
                    elementsWidth = elementArea.Right;
                }
                if (elementArea.Bottom > elementsHeight)
                {
                    elementsHeight = elementArea.Bottom;
                }
                elementsArea += elementArea.Width * elementArea.Height;
            }

            double areaWidth;
            double areaHeight;
            //should we match aspectReatio or not?
            if (!double.IsInfinity(availableSize.Width) && !double.IsInfinity(availableSize.Height))
            {
                //calculate area size pro rata to aspect ratio
                double aspectRatio = availableSize.Width / availableSize.Height;
                areaWidth = Math.Max(elementsWidth, aspectRatio * elementsHeight);
                areaHeight = Math.Max(elementsHeight, elementsWidth / aspectRatio);
            }
            else
            {
                areaWidth = elementsWidth;
                areaHeight = elementsHeight;
            }

            //calculate areas
            double area = areaWidth * areaHeight;
            double freeArea = area - elementsArea;
            return freeArea;
        }

        private Rect MeasureElement(UIElement element, Rect placeholder)
        {
            Size elementSize = element.DesiredSize;
            //check placeholder width fits element desired width
            if (placeholder.Width < elementSize.Width)
            {
                double minAdjustedWidth = PossibleAdjustment.Min * elementSize.Width;
                if (placeholder.Width < minAdjustedWidth)
                {
                    return default(Rect);
                }
            }
            //compact placeholder to element desired width if needed and possible
            else
            {
                double maxAdjustedWidth = PossibleAdjustment.Max * elementSize.Width;
                if (placeholder.Width > maxAdjustedWidth)
                {
                    placeholder.Width = elementSize.Width;
                }
            }

            //check placeholder height fits element desired height
            if (placeholder.Height < elementSize.Height)
            {
                double minAdjustedHeight = PossibleAdjustment.Min * elementSize.Height;
                if (placeholder.Height < minAdjustedHeight)
                {
                    return default(Rect);
                }
            }
            //compact placeholder to element desired height if needed and possible
            else
            {
                double maxAdjustedHeight = PossibleAdjustment.Max * elementSize.Height;
                if (placeholder.Height > maxAdjustedHeight)
                {
                    placeholder.Height = elementSize.Height;
                }
            }

            return placeholder;
        }

        //private void MeasureElement(UIElement element, List<UIElement> measuredElements, List<Rect> placeholders, double aspectRatio)
        //{
        //    int minimumFreeArea = int.MaxValue;
        //    Rect desiredArea = default(Rect);
        //    Rect targetPlaceholder = default(Rect);
        //    foreach (Rect placeholder in placeholders)
        //    {
        //        Rect candidateArea = new Rect(new Point(0, 0), element.DesiredSize);
        //        if (!ApplyAreaToPlaceholder(ref candidateArea, placeholder))
        //        {
        //            continue;
        //        }
        //        //TODO: handle also best fit to placeholder
        //        double freeArea = CalculateFreeArea(candidateElement);
        //        if (freeArea < minimumFreeArea)
        //        {
        //            desiredArea = candidateArea;
        //            targetPlaceholder = placeholder;
        //        }
        //    }
        //    if (desiredArea == default(Rect))
        //    {
        //        throw new InvalidOperationException();
        //    }
        //    element.Measure(desiredArea.Size);
        //    SetAreaProperty(element, desiredArea);
        //}

        //private bool ApplyAreaToPlaceholder(ref Rect elementArea, Rect placeholder)
        //{
        //    //compact area width to placeholder if needed and possible
        //    if (elementArea.Width > placeholder.Width)
        //    {
        //        double minAdjustedWidth = PossibleAdjustment.Min * elementArea.Width;
        //        if (minAdjustedWidth > placeholder.Width)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            elementArea.Width = placeholder.Width;
        //        }
        //    }
        //    //stretch area width to placeholder if needed and possible
        //    else
        //    {
        //        double maxAdjustedWidth = PossibleAdjustment.Max * elementArea.Width;
        //        if (maxAdjustedWidth >= placeholder.Width)
        //        {
        //            elementArea.Width = placeholder.Width;
        //        }
        //    }

        //    //compact area height to placeholder if needed and possible
        //    if (elementArea.Height > placeholder.Height)
        //    {
        //        double minAdjustedHeight = PossibleAdjustment.Min * elementArea.Height;
        //        if (minAdjustedHeight > placeholder.Height)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            elementArea.Height = placeholder.Height;
        //        }
        //    }
        //    //stretch area height to placeholder if needed and possible
        //    else
        //    {
        //        double maxAdjustedHeight = PossibleAdjustment.Max * Height;
        //        if (maxAdjustedHeight >= placeholder.Height)
        //        {
        //            elementArea.Height = placeholder.Height;
        //        }
        //    }

        //    //set area position
        //    elementArea.X = placeholder.X;
        //    elementArea.Y = placeholder.Y;
        //    return true;
        //}
    }
}