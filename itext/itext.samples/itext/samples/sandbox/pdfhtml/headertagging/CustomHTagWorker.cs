using iText.Html2pdf.Attach;
using iText.Html2pdf.Attach.Impl.Tags;
using iText.Layout;
using iText.Layout.Element;
using iText.StyledXmlParser.Node;

namespace iText.Samples.Sandbox.Pdfhtml.Headertagging
{
    public class CustomHTagWorker : DivTagWorker
    {
        private int i;
        public CustomHTagWorker(IElementNode element, ProcessorContext context, int i) : base(element, context)
        {
            this.i = i;
        }

        public override IPropertyContainer GetElementResult()
        {
            Div div = (Div) base.GetElementResult();
            div.GetAccessibilityProperties().SetRole("H" + i);
            return base.GetElementResult();
        }
    }
}