using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adenium.Controls
{
    class Area
    {

        private void AddRightPlaceholder(Element element, Placehoder elementPlaceholder)
        {
            if (elementPlaceholder.Width == element.Width)
            {
                //this means that our element fills placeholder and there is not space for right placeholder
                return;
            }
            //calculate placeholder width
            int width;
            if (elementPlaceholder.Width == int.MaxValue)
            {
                width = int.MaxValue;
            }
            else if (elementPlaceholder.Width > element.Width)
            {
                width = elementPlaceholder.Width - element.Width;
            }
            else
            {
                //elementPlaceholder.Width < element.Width - is not possible, otherwise we have a bug
                throw new InvalidOperationException();
            }

            //calculate placeholder height
            int height;
            if (elementPlaceholder.Height == int.MaxValue)
            {
                height = int.MaxValue;
            }
            else
            {
                Element bottomElement = GetBottomElement(element.Right, element.Bottom);
                if (bottomElement == null)
                {
                    height = int.MaxValue;
                }
                else
                {
                    height = bottomElement.Top - element.Bottom;
                }
            }
            Placehoder placeholder = new Placehoder(element.Right, element.Top, width, height);
            _placeholders.Add(placeholder);
        }

        private void AddBottomPlaceholder(Element element, Placehoder elementPlaceholder)
        {
            if (elementPlaceholder.Height == element.Height)
            {
                //this means that our element fills placeholder and there is not space for bottom placeholder
                return;
            }
            //calculate placeholder height
            int height;
            if (elementPlaceholder.Height == int.MaxValue)
            {
                height = int.MaxValue;
            }
            else if (elementPlaceholder.Height > element.Height)
            {
                height = elementPlaceholder.Height - element.Height;
            }
            else
            {
                //elementPlaceholder.Height < element.Height - is not possible, otherwise we have a bug
                throw new InvalidOperationException();
            }

            //calculate placeholder width
            int width = 0;
            if (elementPlaceholder.Width == int.MaxValue)
            {
                width = int.MaxValue;
            }
            else
            {
                Element rightElement = GetRightElement(element.Right, element.Bottom);
                if (rightElement == null)
                {
                    width = int.MaxValue;
                }
                else
                {
                    width = rightElement.Left - element.Right;
                }
            }
            Placehoder placeholder = new Placehoder(element.Right, element.Top, width, height);
            _placeholders.Add(placeholder);
        }

    }
}
