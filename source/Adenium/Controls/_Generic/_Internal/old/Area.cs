using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Adenium.Controls
{
    internal sealed class Area : ReadOnlyCollection<Element>
    {
        public const double PlaceholderMaxSize = 1000000;
        private readonly List<Rect> _placeholders;
        private double _aspectRatio;

        public Area(Size availableSize)
            : base(new List<Element>())
        {
            if (!double.IsInfinity(availableSize.Width) || !double.IsInfinity(availableSize.Height))
            {
                throw new InvalidOperationException();
            }
            Rect initialPlacehoder = new Rect(new Point(0, 0), new Size(availableSize.Width, availableSize.Height));
            _placeholders = new List<Rect> { initialPlacehoder };
        }

        private Area(double aspectRatio)
            : base(new List<Element>())
        {
            _aspectRatio = aspectRatio;
            Rect initialPlacehoder = new Rect(new Point(0, 0), new Size(double.MaxValue, double.MaxValue));
            _placeholders = new List<Rect> { initialPlacehoder };
        }

        public void Append(Element element)
        {
            int minimumFreeArea = int.MaxValue;
            Element targetElement = null;
            Rect targetPlaceholder = default(Rect);
            foreach (Rect placeholder in _placeholders)
            {
                Element candidateElement = element.Clone();
                if (!candidateElement.Apply(placeholder))
                {
                    continue;
                }
                //TODO: handle also best fit to placeholder
                double freeArea = CalculateFreeArea(candidateElement);
                if (freeArea < minimumFreeArea)
                {
                    targetElement = candidateElement;
                    targetPlaceholder = placeholder;
                }
            }
            if (targetElement == null)
            {
                throw new InvalidOperationException();
            }
            InsertElement(targetElement, targetPlaceholder);
        }

        private void InsertElement(Element element, Rect placehoder)
        {
            if (_placeholders.Remove(placehoder))
            {
                Items.Add(element);
                AddRightPlaceholder(element, placehoder);
                AddBottomPlaceholder(element, placehoder);
                UpdatePlaceholdersSize();
                TrimPlaceholders();
            }
        }

        private void TrimPlaceholders()
        {
            List<Rect> placehodersToRemove = null;
            foreach (Rect placehoder in _placeholders)
            {
                Rect rightPlaceholder = GetRightPlaceholder(placehoder.X, placehoder.Y);
                if (rightPlaceholder != null)
                {
                    //do not use + because we can get overflow in case of MaxValue
                    //placehoder.X + placehoder.Width > rightPlaceholder.X -> 
                    if (placehoder.Width > rightPlaceholder.X - placehoder.X)
                    {
                        placehodersToRemove = placehodersToRemove ?? new List<Rect>();
                        placehodersToRemove.Add(rightPlaceholder);
                    }
                }
                Rect bottomPlaceholder = GetBottomPlaceholder(placehoder.X, placehoder.Y);
                if (bottomPlaceholder != null)
                {
                    //do not use + because we can get overflow in case of MaxValue
                    //placehoder.Y + placehoder.Height > bottomPlaceholder.Y ->
                    if (placehoder.Height > bottomPlaceholder.Y - placehoder.Y)
                    {
                        placehodersToRemove = placehodersToRemove ?? new List<Rect>();
                        placehodersToRemove.Add(bottomPlaceholder);
                    }
                }
            }
            if (placehodersToRemove != null)
            {
                foreach (Rect placehoder in placehodersToRemove)
                {
                    _placeholders.Remove(placehoder);
                }
            }
        }

        private Rect GetRightPlaceholder(double x, double y)
        {
            Rect rightPlacehoder = default(Rect);
            double minDistance = double.MaxValue;
            foreach (Rect placehoder in _placeholders)
            {
                if (HaveMinDifference(placehoder.Y, y))
                {
                    double distance = placehoder.X - x;
                    if (distance > 0 && distance < minDistance)
                    {
                        rightPlacehoder = placehoder;
                        minDistance = distance;
                    }
                }
            }
            return rightPlacehoder;
        }

        private Rect GetBottomPlaceholder(double x, double y)
        {
            Rect bottomPlaceholder = default(Rect);
            double minDistance = double.MaxValue;
            foreach (Rect placehoder in _placeholders)
            {
                if (HaveMinDifference(placehoder.X, x))
                {
                    double distance = placehoder.Y - y;
                    if (distance > 0 && distance < minDistance)
                    {
                        bottomPlaceholder = placehoder;
                        minDistance = distance;
                    }
                }
            }
            return bottomPlaceholder;
        }

        private void AddRightPlaceholder(Element element, Rect elementPlaceholder)
        {
            if (HaveMinDifference(elementPlaceholder.Width, element.Width))
            {
                //this means that element fills placeholder width and there is no space for right placeholder
                return;
            }
            //We don't need to calculate placeholder width and height here as they will be recalculated later in UpdatePlaceholdersSize
            Rect placeholder = new Rect(element.Right, element.Top, PlaceholderMaxSize, PlaceholderMaxSize);
            _placeholders.Add(placeholder);
        }

        private void AddBottomPlaceholder(Element element, Rect elementPlaceholder)
        {
            if (HaveMinDifference(elementPlaceholder.Height, element.Height))
            {
                //this means that element fills placeholder height and there is no space for bottom placeholder
                return;
            }
            //We don't need to calculate placeholder width and height here as they will be recalculated later in UpdatePlaceholdersSize
            Rect placeholder = new Rect(element.Left, element.Bottom, PlaceholderMaxSize, PlaceholderMaxSize);
            _placeholders.Add(placeholder);
        }

        private void UpdatePlaceholdersSize()
        {
            for (int i = 0; i < _placeholders.Count; i++)
            {
                Rect placeholder = _placeholders[i];
                //update placeholder Width
                Element rightElement = GetRightElement(placeholder.X, placeholder.Y);
                if (rightElement == null)
                {
                    placeholder.Width = PlaceholderMaxSize;
                }
                else
                {
                    placeholder.Width = rightElement.Left - placeholder.X;
                }
                //TODO: should we handle case when placehoder.Width == 0, is it possible?

                //update placeholder Height
                Element bottomElement = GetBottomElement(placeholder.X, placeholder.Y);
                if (bottomElement == null)
                {
                    placeholder.Height = PlaceholderMaxSize;
                }
                else
                {
                    placeholder.Height = bottomElement.Top - placeholder.Y;
                }
                //TODO: should we handle case when placehoder.Height == 0, is it possible?
                _placeholders[i] = placeholder;
            }
        }

        private Element GetBottomElement(double x, double y)
        {
            Element bottomElement = null;
            double minDistance = int.MaxValue;
            foreach (Element element in Items)
            {
                if (element.Left < x && element.Right > x)
                {
                    double distance = element.Top - y;
                    if (distance > 0 && distance < minDistance)
                    {
                        bottomElement = element;
                        minDistance = distance;
                    }
                }
            }
            return bottomElement;
        }

        private Element GetRightElement(double x, double y)
        {
            Element rightElement = null;
            double minDistance = double.MaxValue;
            foreach (Element element in Items)
            {

                if ((element.Top < y && element.Bottom > y))
                {
                    double distance = element.Left - x;
                    if (distance > 0 && distance < minDistance)
                    {
                        rightElement = element;
                        minDistance = distance;
                    }
                }
            }
            return rightElement;
        }

        private double CalculateFreeArea(Element candidateElement)
        {
            //add candidate element to the list of elements
            Items.Add(candidateElement);
            try
            {
                //calculate actual width and height of elements and elements area
                double elementsWidth = Items.Max(e => e.Right);
                double elementsHeight = Items.Max(e => e.Bottom);

                double areaWidth;
                double areaHeight;
                //should we match aspectReatio or not?
                if (_aspectRatio > 0)
                {
                    //calculate area size pro rata to aspect ratio
                    areaWidth = Math.Max(elementsWidth, _aspectRatio * elementsHeight);
                    areaHeight = Math.Max(elementsHeight, elementsWidth / _aspectRatio);
                }
                else
                {
                    areaWidth = elementsWidth;
                    areaHeight = elementsHeight;
                }

                //calculate areas: elements, total area and free
                double elementsArea = Items.Sum(e => e.Area);
                double area = areaWidth * areaHeight;
                double freeArea = area - elementsArea;
                return freeArea;
            }
            finally
            {
                //remove candidate element from the list of elements
                Items.Remove(candidateElement);
            }
        }

        private static bool HaveMinDifference(double value1, double value2)
        {
            return Math.Abs(value1 - value2) <= double.Epsilon;
        }
    }
}
