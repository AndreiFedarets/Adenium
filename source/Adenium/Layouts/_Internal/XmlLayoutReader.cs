using Adenium.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Adenium.Layouts
{
    internal sealed class XmlLayoutReader : ILayoutReader
    {
        private const string LayoutElementName = "Layout";
        private const string ItemElementName = "Item";
        private const string AttachmentElementName = "Attachment";

        private const string DisplayModeAttributeName = "Display";
        private const string ViewModelTypeAttributeName = "Type";
        private const string ActivationModeAttributeName = "Activation";
        private const string InstanceModeAttributeName = "Instance";
        private const string OrderAttributeName = "Order";

        public bool SupportsContentType(string layoutContent)
        {
            //Dummy check - we just make sure content starts with "<" that's most probably xml
            return !string.IsNullOrEmpty(layoutContent) && layoutContent.TrimStart().StartsWith("<");
        }

        public Layout ReadLayout(string layoutContent)
        {
            using (StringReader stringReader = new StringReader(layoutContent))
            {
                return ReadLayout(stringReader);
            }
        }

        public Attachment ReadAttachment(string attachmentContent)
        {
            using (StringReader stringReader = new StringReader(attachmentContent))
            {
                return ReadAttachment(stringReader);
            }
        }

        private Layout ReadLayout(TextReader reader)
        {
            using (XmlReader xmlReader = new XmlTextReader(reader))
            {
                return ReadLayout(xmlReader);
            }
        }

        private Attachment ReadAttachment(TextReader reader)
        {
            using (XmlReader xmlReader = new XmlTextReader(reader))
            {
                return ReadAttachment(xmlReader);
            }
        }

        private Layout ReadLayout(XmlReader reader)
        {
            //Move to <Layout> element
            MoveToElement(reader, LayoutElementName);
            //Define defaults
            List<LayoutItem> layoutItems = new List<LayoutItem>();
            DisplayMode displayMode = DisplayMode.Tab;
            //Read <Layout> attributes
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case DisplayModeAttributeName:
                        displayMode = reader.ReadContentAsEnum<DisplayMode>();
                        break;
                }
            }
            //Read <Layout> element content
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement && string.Equals(LayoutElementName, reader.Name))
                {
                    break;
                }
                if (reader.NodeType != XmlNodeType.Element)
                {
                    continue;
                }
                switch (reader.Name)
                {
                    case ItemElementName:
                        LayoutItem layoutItem = ReadLayoutItem(reader);
                        layoutItems.Add(layoutItem);
                        break;
                }
            }
            Layout layout = new Layout(displayMode, layoutItems);
            return layout;
        }

        private Attachment ReadAttachment(XmlReader reader)
        {
            //Move to <Attachment> element
            MoveToElement(reader, AttachmentElementName);
            //Define defaults
            List<LayoutItem> layoutItems = new List<LayoutItem>();
            //Read <Attachment> element content
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement && string.Equals(AttachmentElementName, reader.Name))
                {
                    break;
                }
                if (reader.NodeType != XmlNodeType.Element)
                {
                    continue;
                }
                switch (reader.Name)
                {
                    case ItemElementName:
                        LayoutItem layoutItem = ReadLayoutItem(reader);
                        layoutItems.Add(layoutItem);
                        break;
                }
            }
            Attachment attachment = new Attachment(layoutItems);
            return attachment;
        }

        private LayoutItem ReadLayoutItem(XmlReader reader)
        {
            //Move to <Item> element
            MoveToElement(reader, ItemElementName);
            //Define defaults
            string viewModelTypeName = string.Empty;
            InstanceMode instanceMode = InstanceMode.Multiple;
            ActivationMode activationMode = ActivationMode.OnStartup;
            int order = 0;
            //Read <Item> attributes
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case ViewModelTypeAttributeName:
                        viewModelTypeName = reader.ReadContentAsString();
                        break;
                    case ActivationModeAttributeName:
                        activationMode = reader.ReadContentAsEnum<ActivationMode>();
                        break;
                    case InstanceModeAttributeName:
                        instanceMode = reader.ReadContentAsEnum<InstanceMode>();
                        break;
                    case OrderAttributeName:
                        order = reader.ReadContentAsInt();
                        break;
                }
            }
            LayoutItem layoutItem = new LayoutItem(viewModelTypeName, activationMode, instanceMode, order);
            return layoutItem;
        }

        private void MoveToElement(XmlReader reader, string elementName)
        {
            if (string.Equals(reader.Name, elementName, StringComparison.InvariantCulture))
            {
                return;
            }
            if (reader.ReadToFollowing(elementName))
            {
                return;
            }
            throw new Exception();
        }
    }
}
