﻿using Layex.ViewModels;
using System;

namespace Layex.Layouts
{
    internal sealed class JsonLayoutReader : ILayoutReader
    {
        public bool SupportsContentType(string layoutContent)
        {
            return false;
        }

        public Layout ReadLayout(string layoutContent)
        {
            throw new NotImplementedException();
        }

        public Attachment ReadAttachment(string attachmentContent)
        {
            throw new NotImplementedException();
        }
    }
}