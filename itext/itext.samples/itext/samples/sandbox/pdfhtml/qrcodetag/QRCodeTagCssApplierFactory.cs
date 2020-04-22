using iText.Html2pdf.Css.Apply;
using iText.Html2pdf.Css.Apply.Impl;
using iText.StyledXmlParser.Node;

namespace iText.Samples.Sandbox.Pdfhtml.Qrcodetag
{
    /// <summary>
    /// Example of a custom CssApplier factory for pdfHTML
    /// </summary>
    /// <remarks>
    /// The tag <bold>qr</bold> is mapped on a BlockCssApplier. Every other tag is mapped to the default.
    /// </remarks>
    public class QRCodeTagCssApplierFactory : DefaultCssApplierFactory
    {
        public override ICssApplier GetCustomCssApplier(IElementNode tag)
        {
            if (tag.Name().Equals("qr"))
            {
                return new BlockCssApplier();
            }
            return null;
        }
    }
}