using System;
using System.Collections.Generic;
using System.Linq;

namespace Adenium.Controls
{
    internal sealed class Area
    {
        public const int PlaceholderMaxSize = 1000000;
        private static readonly Placehoder InitialPlacehoder;
        private readonly List<Placehoder> _placeholders;
        private readonly List<Element> _elements;
        private float _aspectRatio;

        static Area()
        {
            InitialPlacehoder = new Placehoder(0, 0, PlaceholderMaxSize, PlaceholderMaxSize);
        }

        public Area(float aspectRatio)
        {
            _aspectRatio = aspectRatio;
            _elements = new List<Element>();
            _placeholders = new List<Placehoder> { InitialPlacehoder };
        }

        public Area()
            : this(4f / 3f)
        {
        }

        public void Append(Element element)
        {
            int minimumFreeArea = int.MaxValue;
            Element targetElement = null;
            Placehoder targetPlaceholder = null;
            foreach (Placehoder placeholder in _placeholders)
            {
                Element candidateElement = element.Clone();
                if (!candidateElement.Apply(placeholder))
                {
                    continue;
                }
                //TODO: handle also best fit to placeholder
                int freeArea = CalculateFreeArea(candidateElement);
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

        private void InsertElement(Element element, Placehoder placehoder)
        {
            if (_placeholders.Remove(placehoder))
            {
                _elements.Add(element);
                AddRightPlaceholder(element, placehoder);
                AddBottomPlaceholder(element, placehoder);
                UpdatePlaceholdersSize();
                TrimPlaceholders();
            }
        }

        private void TrimPlaceholders()
        {
            List<Placehoder> placehodersToRemove = null;
            foreach (Placehoder placehoder in _placeholders)
            {
                Placehoder rightPlaceholder = GetRightPlaceholder(placehoder.X, placehoder.Y);
                if (rightPlaceholder != null)
                {
                    if (placehoder.X + placehoder.Width > rightPlaceholder.X)
                    {
                        placehodersToRemove = placehodersToRemove ?? new List<Placehoder>();
                        placehodersToRemove.Add(rightPlaceholder);
                    }
                }
                Placehoder bottomPlaceholder = GetBottomPlaceholder(placehoder.X, placehoder.Y);
                if (bottomPlaceholder != null)
                {
                    if (placehoder.Y + placehoder.Height > bottomPlaceholder.Y)
                    {
                        placehodersToRemove = placehodersToRemove ?? new List<Placehoder>();
                        placehodersToRemove.Add(bottomPlaceholder);
                    }
                }
            }
            if (placehodersToRemove != null)
            {
                foreach (Placehoder placehoder in placehodersToRemove)
                {
                    _placeholders.Remove(placehoder);
                }
            }
        }

        private Placehoder GetRightPlaceholder(int x, int y)
        {
            Placehoder rightPlacehoder = null;
            int minDistance = int.MaxValue;
            foreach (Placehoder placehoder in _placeholders)
            {
                if (placehoder.Y == y)
                {
                    int distance = placehoder.X - x;
                    if (distance > 0 && distance < minDistance)
                    {
                        rightPlacehoder = placehoder;
                        minDistance = distance;
                    }
                }
            }
            return rightPlacehoder;
        }

        private Placehoder GetBottomPlaceholder(int x, int y)
        {
            Placehoder bottomPlaceholder = null;
            int minDistance = int.MaxValue;
            foreach (Placehoder placehoder in _placeholders)
            {
                if (placehoder.X == x)
                {
                    int distance = placehoder.Y - y;
                    if (distance > 0 && distance < minDistance)
                    {
                        bottomPlaceholder = placehoder;
                        minDistance = distance;
                    }
                }
            }
            return bottomPlaceholder;
        }

        private void AddRightPlaceholder(Element element, Placehoder elementPlaceholder)
        {
            if (elementPlaceholder.Width == element.Width)
            {
                //this means that element fills placeholder width and there is no space for right placeholder
                return;
            }
            //We don't need to calculate placeholder width and height here as they will be recalculated later in UpdatePlaceholdersSize
            Placehoder placeholder = new Placehoder(element.Right, element.Top, PlaceholderMaxSize, PlaceholderMaxSize);
            _placeholders.Add(placeholder);
        }

        private void AddBottomPlaceholder(Element element, Placehoder elementPlaceholder)
        {
            if (elementPlaceholder.Height == element.Height)
            {
                //this means that element fills placeholder height and there is no space for bottom placeholder
                return;
            }
            //We don't need to calculate placeholder width and height here as they will be recalculated later in UpdatePlaceholdersSize
            Placehoder placeholder = new Placehoder(element.Left, element.Bottom, PlaceholderMaxSize, PlaceholderMaxSize);
            _placeholders.Add(placeholder);
        }

        private void UpdatePlaceholdersSize()
        {
            foreach (Placehoder placehoder in _placeholders)
            {
                //update placeholder Width
                Element rightElement = GetRightElement(placehoder.X, placehoder.Y);
                if (rightElement == null)
                {
                    placehoder.Width = PlaceholderMaxSize;
                }
                else
                {
                    placehoder.Width = rightElement.Left - placehoder.X;
                }
                //TODO: should we handle case when placehoder.Width == 0, is it possible?

                //update placeholder Height
                Element bottomElement = GetBottomElement(placehoder.X, placehoder.Y);
                if (bottomElement == null)
                {
                    placehoder.Height = PlaceholderMaxSize;
                }
                else
                {
                    placehoder.Height = bottomElement.Top - placehoder.Y;
                }
                //TODO: should we handle case when placehoder.Height == 0, is it possible?
            }
        }

        private Element GetBottomElement(int x, int y)
        {
            Element bottomElement = null;
            int minDistance = int.MaxValue;
            foreach (Element element in _elements)
            {
                if (element.Left < x && element.Right > x)
                {
                    int distance = element.Top - y;
                    if (distance > 0 && distance < minDistance)
                    {
                        bottomElement = element;
                        minDistance = distance;
                    }
                }
            }
            return bottomElement;
        }

        private Element GetRightElement(int x, int y)
        {
            Element rightElement = null;
            int minDistance = int.MaxValue;
            foreach (Element element in _elements)
            {

                if ((element.Top < y && element.Bottom > y))
                {
                    int distance = element.Left - x;
                    if (distance > 0 && distance < minDistance)
                    {
                        rightElement = element;
                        minDistance = distance;
                    }
                }
            }
            return rightElement;
        }

        private int CalculateFreeArea(Element candidateElement)
        {
            //add candidate element to the list of elements
            _elements.Add(candidateElement);
            try
            {
                //calculate actual width and height of elements
                int elementsWidth = _elements.Max(e => e.Right);
                int elementsHeight = _elements.Max(e => e.Bottom);

                //calculate area size pro rata to aspect ratio
                int areaWidth = (int)Math.Max(elementsWidth, _aspectRatio * elementsHeight);
                int areaHeight = (int)Math.Max(elementsHeight, elementsWidth / _aspectRatio);

                //calculate areas: elements, total area and free
                int elementsArea = _elements.Sum(e => e.Area);
                int area = areaWidth * areaHeight;
                int freeArea = area - elementsArea;
                return freeArea;
            }
            finally
            {
                //remove candidate element from the list of elements
                _elements.Remove(candidateElement);
            }
        }
    }
}
