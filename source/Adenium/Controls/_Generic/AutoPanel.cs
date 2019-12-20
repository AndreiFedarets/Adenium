﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Adenium.Controls
{
    public class AutoPanel : Panel
    {
        //private const int ElementsSpace = 1; // space between elements, must be >= 0
        private static readonly Range PossibleElementAdjustment;
        private static readonly Range PossibleAspectRationScale;
        public static DependencyProperty AreaProperty;
        private readonly List<UIElement> _measuredElements;
        private double _initialAspectRatio;
        private Rect _previousDesiredArea;

        static AutoPanel()
        {
            PossibleElementAdjustment = new Range(0.7f, 1.3f);
            PossibleAspectRationScale = new Range(0.8f, 1.2f);
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
                element.Arrange(area);
            }
            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Rect availableArea = new Rect(new Point(0, 0), availableSize);
            Rect measureArea;
            double aspectRatio = 0;
            if (double.IsPositiveInfinity(availableSize.Width) && double.IsPositiveInfinity(availableSize.Height))
            {
                measureArea = new Rect(new Point(0, 0), availableSize);
                //TODO: get current screen aspect ratio
                aspectRatio = 4f / 3f; 
            }
            else if (double.IsPositiveInfinity(availableSize.Width) || double.IsPositiveInfinity(availableSize.Height))
            {
                measureArea = new Rect(new Point(0, 0), availableSize);
                aspectRatio = 0;
            }
            else
            {
                measureArea = new Rect(new Point(0, 0), new Size(double.PositiveInfinity, double.PositiveInfinity));
                aspectRatio = availableSize.Width / availableSize.Height;
            }
            Rect desiredArea = _previousDesiredArea;
            if (!VerifyAllElementsMeasured() || !VerifyAspectRatioUnchanged())
            {
                desiredArea = MeasureElements(measureArea, aspectRatio);
                AdjustElements(desiredArea);
                desiredArea = ScaleElements(availableArea, desiredArea);
                _initialAspectRatio = desiredArea.Width / desiredArea.Height;
            }
            else
            {
                desiredArea = ScaleElements(availableArea, desiredArea);
            }
            _previousDesiredArea = desiredArea;
            return desiredArea.Size;
        }

        private bool VerifyAspectRatioUnchanged()
        {
            if (_initialAspectRatio == 0 || _previousDesiredArea == default(Rect))
            {
                return false;
            }
            double currentAspectRatio = _previousDesiredArea.Width / _previousDesiredArea.Height;
            return currentAspectRatio * PossibleAspectRationScale.Min < _initialAspectRatio &&
                   currentAspectRatio * PossibleAspectRationScale.Max > _initialAspectRatio;
        }

        private bool VerifyAllElementsMeasured()
        {
            if (_measuredElements.Count != InternalChildren.Count)
            {
                return false;
            }
            for (int i = 0; i < _measuredElements.Count; i++)
            {
                if (!ReferenceEquals(_measuredElements[i], InternalChildren[i]))
                {
                    return false;
                }
            }
            return true;
        }

        private Rect ScaleElements(Rect availableArea, Rect desiredArea)
        {
            if (double.IsPositiveInfinity(availableArea.Width) || double.IsPositiveInfinity(availableArea.Height))
            {
                return desiredArea;
            }
            double scaleX = availableArea.Width / desiredArea.Width;
            double scaleY = availableArea.Height / desiredArea.Height;
            foreach (UIElement element in InternalChildren)
            {
                Rect elementArea = GetAreaProperty(element);
                elementArea.Scale(scaleX, scaleY);
                element.Measure(elementArea.Size);
                SetAreaProperty(element, elementArea);
            }
            return availableArea;
        }

        private Rect MeasureElements(Rect availableArea, double aspectRatio)
        {
            Rect desiredArea = new Rect();
            _measuredElements.Clear();
            foreach (UIElement element in InternalChildren)
            {
                if (element != null)
                {
                    Rect elementArea = MeasureElement(element, availableArea, aspectRatio);
                    SetAreaProperty(element, elementArea);
                    _measuredElements.Add(element);
                    desiredArea.Union(elementArea);
                }
            }
            return desiredArea;
        }

        private void AdjustElements(Rect desiredArea)
        {
            List<Rect> placeholders = new List<Rect>();
            foreach (UIElement element in InternalChildren)
            {
                if (element != null)
                {
                    Rect elementArea = GetAreaProperty(element);

                    placeholders.Clear();
                    Point topRightPoint = new Point(elementArea.TopRight.X, elementArea.TopRight.Y);
                    BuildPlaceholder(topRightPoint, desiredArea, placeholders);

                    Point bottomLeftPoint = new Point(elementArea.BottomLeft.X, elementArea.BottomLeft.Y);
                    BuildPlaceholder(bottomLeftPoint, desiredArea, placeholders);

                    foreach (Rect placeholder in placeholders)
                    {
                        elementArea = Rect.Union(elementArea, placeholder);
                    }
                    element.Measure(elementArea.Size);
                    SetAreaProperty(element, elementArea);
                }
            }
        }

        private Rect MeasureElement(UIElement element, Rect availableArea, double aspectRatio)
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
                double freeArea = CalculateFreeArea(elementRect, availableArea.Size, aspectRatio);
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
            return desiredRect;
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
                BuildPlaceholder(topRightPoint, availableArea, placeholders);

                Point bottomLeftPoint = new Point(elementArea.BottomLeft.X, elementArea.BottomLeft.Y);
                BuildPlaceholder(bottomLeftPoint, availableArea, placeholders);
            }
            return placeholders;
        }

        private void BuildPlaceholder(Point location, Rect availableArea, List<Rect> placeholders)
        {
            const double offset = 0.1;
            Point checkPoint = new Point(location.X + offset, location.Y + offset);
            //verify if this location is out of available area space
            if (!availableArea.Contains(checkPoint))
            {
                return;
            }
            //verify if this location is already used by another element
            foreach (UIElement element in _measuredElements)
            {
                Rect elementArea = GetAreaProperty(element);
                if (elementArea.Contains(checkPoint))
                {
                    return;
                }
            }
            //verify if this location is already included into existing placeholder
            foreach (Rect placeholder in placeholders)
            {
                if (placeholder.Contains(checkPoint))
                {
                    return;
                }
            }
            //create raw placeholder
            Rect newPlaceholder = Rect.Offset(availableArea, location.X, location.Y);
            newPlaceholder = Rect.Intersect(availableArea, newPlaceholder);
            // placeholder could became Rect.Empty because it goes outside of availableArea
            if (newPlaceholder == Rect.Empty)
            {
                return;
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
            placeholders.Add(newPlaceholder);
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

        private double CalculateFreeArea(Rect candidateArea, Size availableSize, double aspectRatio)
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
            if (aspectRatio != 0)
            {
                //calculate area size pro rata to aspect ratio
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
                double minPossibleWidth = PossibleElementAdjustment.Min * elementSize.Width;
                if (placeholder.Width < minPossibleWidth)
                {
                    return default(Rect);
                }
            }
            //compact placeholder to element desired width if needed and possible
            else
            {
                double maxPossibleWidth = PossibleElementAdjustment.Max * elementSize.Width;
                if (placeholder.Width > maxPossibleWidth)
                {
                    placeholder.Width = elementSize.Width;
                }
            }

            //check placeholder height fits element desired height
            if (placeholder.Height < elementSize.Height)
            {
                double minPossibleHeight = PossibleElementAdjustment.Min * elementSize.Height;
                if (placeholder.Height < minPossibleHeight)
                {
                    return default(Rect);
                }
            }
            //compact placeholder to element desired height if needed and possible
            else
            {
                double maxPossibleHeight = PossibleElementAdjustment.Max * elementSize.Height;
                if (placeholder.Height > maxPossibleHeight)
                {
                    placeholder.Height = elementSize.Height;
                }
            }

            return placeholder;
        }
    }
}