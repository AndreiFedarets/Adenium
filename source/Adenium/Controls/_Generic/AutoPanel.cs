using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Adenium.Controls
{
    public class AutoPanel : Panel
    {
        //private const int ElementsSpace = 1; // space between elements, must be >= 0
        private static readonly Range PossibleAdjustment;
        public static DependencyProperty AreaProperty;

        private readonly List<UIElement> _measuredElements;

        static AutoPanel()
        {
            PossibleAdjustment = new Range(0.75f, 1.25f);
            AreaProperty = DependencyProperty.RegisterAttached("Area", typeof(Rect), typeof(AutoPanel), new PropertyMetadata(default(Rect)));
        }

        public AutoPanel()
        {
            _measuredElements = new List<UIElement>();
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
            Rect availableArea = new Rect(new Point(0, 0), availableSize);
            foreach (UIElement element in InternalChildren)
            {
                if (element != null)
                {
                    MeasureElement(element, availableArea);
                }
            }
            //return availableSize;
            return new Size(0, 0);
        }

        private void MeasureElement(UIElement element, Rect availableArea)
        {
            element.Measure(availableArea.Size);
            double minimumFreeArea = double.MaxValue;
            Rect desiredRect = default(Rect);
            foreach (Rect placeholder in GetPlaceholders(availableArea))
            {
                Rect elementRect = AdjustPlaceholder(element, placeholder);
                if (elementRect == default(Rect))
                {
                    continue;
                }
                //TODO: handle also best fit to placeholder
                double freeArea = CalculateFreeArea(elementRect, availableArea.Size);
                if (freeArea < minimumFreeArea)
                {
                    minimumFreeArea = freeArea;
                    desiredRect = elementRect;
                }
            }
            if (desiredRect == default(Rect))
            {
                throw new InvalidOperationException();
            }
            element.Measure(desiredRect.Size);
            SetAreaProperty(element, desiredRect);
            _measuredElements.Add(element);
        }

        private List<Rect> GetPlaceholders(Rect availableArea)
        {
            List<Rect> placeholders = new List<Rect>();
            if (_measuredElements.Count == 0)
            {
                placeholders.Add(availableArea);
                return placeholders;
            }
            foreach (UIElement element in _measuredElements)
            {
                Rect elementArea = GetAreaProperty(element);

                Point topRightPoint = new Point(elementArea.TopRight.X, elementArea.TopRight.Y);
                Rect topRightPlaceholder = GetPlaceholder(topRightPoint, availableArea);
                if (topRightPlaceholder != default(Rect))
                {
                    bool placeholderExists = false;
                    Rect placeholderToRemove = default(Rect);
                    foreach (Rect exisingPlaceholder in placeholders)
                    {
                        Rect intersectPlaceholder = Rect.Intersect(exisingPlaceholder, topRightPlaceholder);
                        if (intersectPlaceholder.Equals(topRightPlaceholder))
                        {
                            placeholderExists = true;
                            break;
                        }
                        if (intersectPlaceholder.Equals(exisingPlaceholder))
                        {
                            placeholderToRemove = exisingPlaceholder;
                            break;
                        }
                    }
                    if (!placeholderExists)
                    {
                        placeholders.Add(topRightPlaceholder);
                    }
                    if (placeholderToRemove != default(Rect))
                    {
                        placeholders.Remove(placeholderToRemove);
                    }
                }

                Point bottomLeftPoint = new Point(elementArea.BottomLeft.X, elementArea.BottomLeft.Y);
                Rect bottomLeftPlaceholder = GetPlaceholder(bottomLeftPoint, availableArea);
                if (bottomLeftPlaceholder != default(Rect))
                {
                    bool placeholderExists = false;
                    Rect placeholderToRemove = default(Rect);
                    foreach (Rect exisingPlaceholder in placeholders)
                    {
                        Rect intersectPlaceholder = Rect.Intersect(exisingPlaceholder, bottomLeftPlaceholder);
                        if (intersectPlaceholder.Equals(bottomLeftPlaceholder))
                        {
                            placeholderExists = true;
                            break;
                        }
                        if (intersectPlaceholder.Equals(exisingPlaceholder))
                        {
                            placeholderToRemove = exisingPlaceholder;
                            break;
                        }
                    }
                    if (!placeholderExists)
                    {
                        placeholders.Add(bottomLeftPlaceholder);
                    }
                    if (placeholderToRemove != default(Rect))
                    {
                        placeholders.Remove(placeholderToRemove);
                    }
                }
            }
            return placeholders;
        }

        private Rect GetPlaceholder(Point location, Rect availableArea)
        {
            foreach (UIElement element in _measuredElements)
            {
                Rect elementArea = GetAreaProperty(element);
                if (elementArea.Location.Equals(location))
                {
                    return default(Rect);
                }
            }
            Rect placeholder = Rect.Offset(availableArea, location.X, location.Y);
            placeholder = Rect.Intersect(availableArea, placeholder);
            // placeholder could became Rect.Empty because it goes outside of availableArea
            if (placeholder == Rect.Empty)
            {
                return placeholder;
            }
            foreach (UIElement element in _measuredElements)
            {
                Rect elementArea = GetAreaProperty(element);
                if (elementArea.Left > placeholder.Left)
                {
                    Point horizontalIntersectionPoint = new Point(elementArea.Left, placeholder.Top + 1);
                    if (elementArea.Contains(horizontalIntersectionPoint))
                    {
                        double distance = elementArea.Left - placeholder.Left;
                        if (distance == 0)
                        {
                            return default(Rect);
                        }
                        if (distance < placeholder.Width)
                        {
                            placeholder.Width = distance;
                        }
                    }
                }
                if (elementArea.Top > placeholder.Top)
                {
                    Point verticalIntersectionPoint = new Point(placeholder.Left + 1, elementArea.Top);
                    if (elementArea.Contains(verticalIntersectionPoint))
                    {
                        double distance = elementArea.Top - placeholder.Top;
                        if (distance == 0)
                        {
                            return default(Rect);
                        }
                        if (distance < placeholder.Height)
                        {
                            placeholder.Height = distance;
                        }
                    }
                }
            }
            return placeholder;
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
            else if (double.IsInfinity(availableSize.Width) && double.IsInfinity(availableSize.Height))
            {
                //calculate area size pro rata to aspect ratio
                double aspectRatio = 4f / 3f;
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

        private Rect AdjustPlaceholder(UIElement element, Rect placeholder)
        {
            Size elementSize = element.DesiredSize;
            //check placeholder width fits element desired width
            if (placeholder.Width < elementSize.Width)
            {
                double minPossibleWidth = PossibleAdjustment.Min * elementSize.Width;
                if (placeholder.Width < minPossibleWidth)
                {
                    return default(Rect);
                }
            }
            //compact placeholder to element desired width if needed and possible
            else
            {
                double maxPossibleWidth = PossibleAdjustment.Max * elementSize.Width;
                if (placeholder.Width > maxPossibleWidth)
                {
                    placeholder.Width = elementSize.Width;
                }
            }

            //check placeholder height fits element desired height
            if (placeholder.Height < elementSize.Height)
            {
                double minPossibleHeight = PossibleAdjustment.Min * elementSize.Height;
                if (placeholder.Height < minPossibleHeight)
                {
                    return default(Rect);
                }
            }
            //compact placeholder to element desired height if needed and possible
            else
            {
                double maxPossibleHeight = PossibleAdjustment.Max * elementSize.Height;
                if (placeholder.Height > maxPossibleHeight)
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