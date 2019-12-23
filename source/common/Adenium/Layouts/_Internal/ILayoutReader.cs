using Adenium.ViewModels;

namespace Adenium.Layouts
{
    internal interface ILayoutReader
    {
        bool SupportsContentType(string layoutContent);

        Layout ReadLayout(string layoutContent);

        Attachment ReadAttachment(string attachmentContent);
    }
}
