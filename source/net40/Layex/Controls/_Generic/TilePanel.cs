using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Layex.Controls
{
    public class TilePanel : Panel
    {
        //private const int ElementsSpace = 1; // space between elements, must be >= 0
        private static readonly Point PossibleElementAdjustment;
        private static readonly Point PossibleAspectRationScale;
        public static DependencyProperty AreaProperty;
        private readonly List<UIElement> _measuredElements;
        private Size _previousDesiredSize;

        static TilePanel()
        {
            PossibleElementAdjustment = new Point(0.9f, 1.1f);
            PossibleAspectRationScale = new Point(0.9f, 1.1f);
            AreaProperty = DependencyProperty.RegisterAttached("Area", typeof(Rect), typeof(TilePanel), new PropertyMetadata(default(Rect)));
        }

        public TilePanel()
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
                element.Arrange(area);
            }
            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            //aspectRatio = SystemParameters.PrimaryScreenWidth / SystemParameters.PrimaryScreenHeight; 

            Size desiredSize = MeasureElements(availableSize);
            AdjustElements(desiredSize);
            desiredSize = ScaleElements(availableSize, desiredSize);

            //if (!VerifyAllElementsMeasured())
            //{
            //    desiredSize = MeasureElements(availableSize);
            //    AdjustElements(desiredSize);
            //    desiredSize = ScaleElements(availableSize, desiredSize);
            //}
            //else
            //{
            //    desiredSize = ScaleElements(availableSize, desiredSize);
            //}
            //_previousDesiredSize = desiredSize;
            return desiredSize;
        }

        //private bool VerifyAllElementsMeasured()
        //{
        //    if (_measuredElements.Count != InternalChildren.Count)
        //    {
        //        return false;
        //    }
        //    for (int i = 0; i < _measuredElements.Count; i++)
        //    {
        //        if (!ReferenceEquals(_measuredElements[i], InternalChildren[i]))
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        private Size ScaleElements(Size availableSize, Size desiredSize)
        {
            if (double.IsPositiveInfinity(availableSize.Width) || double.IsPositiveInfinity(availableSize.Height))
            {
                return desiredSize;
            }
            double scaleX = availableSize.Width / desiredSize.Width;
            double scaleY = availableSize.Height / desiredSize.Height;
            foreach (UIElement element in InternalChildren)
            {
                Rect elementArea = GetAreaProperty(element);
                elementArea.Scale(scaleX, scaleY);
                element.Measure(elementArea.Size);
                SetAreaProperty(element, elementArea);
            }
            return availableSize;
        }

        private Size MeasureElements(Size availableSize)
        {
            Rect desiredArea = new Rect();
            _measuredElements.Clear();
            foreach (UIElement element in InternalChildren)
            {
                if (element != null)
                {
                    Rect elementArea = MeasureElement(element, ref availableSize);
                    SetAreaProperty(element, elementArea);
                    _measuredElements.Add(element);
                    desiredArea.Union(elementArea);
                }
            }
            return desiredArea.Size;
        }

        private void AdjustElements(Size desiredSize)
        {
            List<Rect> placeholders = new List<Rect>();
            foreach (UIElement element in InternalChildren)
            {
                if (element != null)
                {
                    Rect elementArea = GetAreaProperty(element);

                    placeholders.Clear();
                    Point topRightPoint = new Point(elementArea.TopRight.X, elementArea.TopRight.Y);
                    Rect topRight = BuildPlaceholder(topRightPoint, desiredSize, placeholders);
                    if (topRight != default(Rect))
                    {
                        elementArea.Width = elementArea.Width + topRight.Width;
                    }

                    Point bottomLeftPoint = new Point(elementArea.BottomLeft.X, elementArea.BottomLeft.Y);
                    Rect bottomLeft = BuildPlaceholder(bottomLeftPoint, desiredSize, placeholders);
                    if (bottomLeft != default(Rect))
                    {
                        elementArea.Height = elementArea.Height + bottomLeft.Height;
                    }
                    element.Measure(elementArea.Size);
                    SetAreaProperty(element, elementArea);
                }
            }
        }

        private Rect MeasureElement(UIElement element, ref Size availableSize)
        {
            element.Measure(availableSize);
            double minimumFreeArea = double.MaxValue;
            Rect desiredRect = default(Rect);
            foreach (Rect placeholder in GetPlaceholders(availableSize))
            {
                Rect elementRect = AdjustPlaceholder(element, placeholder);
                if (elementRect == default(Rect))
                {
                    continue;
                }
                //TODO: handle also best fit to placeholder
                double freeArea = CalculateFreeArea(elementRect, availableSize);
                if (freeArea < minimumFreeArea)
                {
                    minimumFreeArea = freeArea;
                    desiredRect = elementRect;
                }
            }
            if (desiredRect == default(Rect))
            {
                availableSize = ScaleAvailableSize(availableSize);
                return MeasureElement(element, ref availableSize);
            }
            element.Measure(desiredRect.Size);
            return desiredRect;
        }

        private Size ScaleAvailableSize(Size availableSize)
        {
            const double scaleRatio = 1.2;
            return new Size(availableSize.Width * scaleRatio, availableSize.Height * scaleRatio);
        }

        private List<Rect> GetPlaceholders(Size availableSize)
        {
            List<Rect> placeholders = new List<Rect>();
            if (_measuredElements.Count == 0)
            {
                placeholders.Add(new Rect(new Point(0, 0), availableSize));
                return placeholders;
            }
            foreach (UIElement element in _measuredElements)
            {
                Rect elementArea = GetAreaProperty(element);

                Point topRightPoint = new Point(elementArea.TopRight.X, elementArea.TopRight.Y);
                Rect topRight = BuildPlaceholder(topRightPoint, availableSize, placeholders);
                if (topRight != default(Rect))
                {
                    placeholders.Add(topRight);
                }

                Point bottomLeftPoint = new Point(elementArea.BottomLeft.X, elementArea.BottomLeft.Y);
                Rect bottomLeft = BuildPlaceholder(bottomLeftPoint, availableSize, placeholders);
                if (bottomLeft != default(Rect))
                {
                    placeholders.Add(bottomLeft);
                }
            }
            return placeholders;
        }

        private Rect BuildPlaceholder(Point location, Size availableSize, List<Rect> placeholders)
        {
            const double offset = 0.1;
            Rect availableArea = new Rect(new Point(0, 0), availableSize);
            Point checkPoint = new Point(location.X + offset, location.Y + offset);
            //verify if this location is out of available area space
            //if (!availableArea.Contains(checkPoint))
            //{
            //    return default(Rect);
            //}
            //verify if this location is already used by another element
            foreach (UIElement element in _measuredElements)
            {
                Rect elementArea = GetAreaProperty(element);
                if (elementArea.Contains(checkPoint))
                {
                    return default(Rect);
                }
            }
            //verify if this location is already included into existing placeholder
            foreach (Rect placeholder in placeholders)
            {
                if (placeholder.Contains(checkPoint))
                {
                    return default(Rect);
                }
            }
            //create raw placeholder
            Rect newPlaceholder = Rect.Offset(availableArea, location.X, location.Y);
            newPlaceholder = Rect.Intersect(availableArea, newPlaceholder);
            // placeholder could became Rect.Empty because it goes outside of availableArea
            if (newPlaceholder == Rect.Empty)
            {
                return default(Rect);
            }
            //calculate placeholder width and height
            foreach (UIElement element in _measuredElements)
            {
                Rect elementArea = GetAreaProperty(element);
                //of element is out of our placeholder area - skip it
                if (elementArea.Right <= newPlaceholder.Left || elementArea.Bottom <= newPlaceholder.Top)
                {
                    continue;
                }
                Point horizontalIntersectionPoint = new Point(elementArea.Left, newPlaceholder.Top + 1);
                if (elementArea.Contains(horizontalIntersectionPoint))
                {
                    double distance = elementArea.Left - newPlaceholder.Left;
                    if (distance < newPlaceholder.Width)
                    {
                        newPlaceholder.Width = distance;
                    }
                }
                Point verticalIntersectionPoint = new Point(newPlaceholder.Left + 1, elementArea.Top);
                if (elementArea.Contains(verticalIntersectionPoint))
                {
                    double distance = elementArea.Top - newPlaceholder.Top;
                    if (distance < newPlaceholder.Height)
                    {
                        newPlaceholder.Height = distance;
                    }
                }
            }
            //remove placeholders which are included into our placeholder
            for (int i = 0; i < placeholders.Count; i++)
            {
                Rect placeholder = placeholders[i];
                if (Rect.Intersect(newPlaceholder, placeholder).Equals(placeholder))
                {
                    placeholders.RemoveAt(i);
                    i--;
                }
            }
            return newPlaceholder;
        }

        private Rect GetPlaceholder(Point location, Rect availableArea)
        {
            foreach (UIElement element in InternalChildren)
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
            if (!double.IsPositiveInfinity(availableSize.Width) && !double.IsPositiveInfinity(availableSize.Height))
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

        private Rect AdjustPlaceholder(UIElement element, Rect placeholder)
        {
            Size elementSize = element.DesiredSize;
            if (placeholder.Width < elementSize.Width)
            {
                double minPossibleWidth = PossibleElementAdjustment.X * elementSize.Width;
                if (placeholder.Width < minPossibleWidth)
                {
                    return default(Rect);
                }
            }
            //compact placeholder to element desired width if needed and possible
            else
            {
                double maxPossibleWidth = PossibleElementAdjustment.Y * elementSize.Width;
                if (placeholder.Width > maxPossibleWidth)
                {
                    placeholder.Width = elementSize.Width;
                }
            }

            //check placeholder height fits element desired height
            if (placeholder.Height < elementSize.Height)
            {
                double minPossibleHeight = PossibleElementAdjustment.X * elementSize.Height;
                if (placeholder.Height < minPossibleHeight)
                {
                    return default(Rect);
                }
            }
            //compact placeholder to element desired height if needed and possible
            else
            {
                double maxPossibleHeight = PossibleElementAdjustment.Y * elementSize.Height;
                if (placeholder.Height > maxPossibleHeight)
                {
                    placeholder.Height = elementSize.Height;
                }
            }

            return placeholder;
        }
    }
}