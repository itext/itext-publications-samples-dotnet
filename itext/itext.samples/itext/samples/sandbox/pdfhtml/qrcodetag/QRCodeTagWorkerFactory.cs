using iText.Html2pdf.Attach;
using iText.Html2pdf.Attach.Impl;
using iText.StyledXmlParser.Node;

namespace iText.Samples.Sandbox.Pdfhtml.Qrcodetag
{
    /// <summary>
    /// Example of a custom tagworkerfactory for pdfHTML
    /// </summary>
    /// <remarks>
    /// The tag <bold>qr</bold> is mapped on a QRCode tagworker. Every other tag is mapped to the default.
    /// </remarks>
    public class QRCodeTagWorkerFactory : DefaultTagWorkerFactory
    {
        public override ITagWorker GetCustomTagWorker(IElementNode tag, ProcessorContext context)
        {
            if (tag.Name().Equals("qr"))
            {
                return new QRCodeTagWorker(tag, context);
            }
            return null;
        }
    }
}