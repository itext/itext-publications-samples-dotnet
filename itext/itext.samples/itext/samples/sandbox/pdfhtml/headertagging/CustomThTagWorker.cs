using iText.Html2pdf.Attach;
using iText.Html2pdf.Attach.Impl.Tags;
using iText.Kernel.Pdf.Tagging;
using iText.Layout;
using iText.Layout.Element;
using iText.StyledXmlParser.Node;

namespace iText.Samples.Sandbox.Pdfhtml.Headertagging
{
    public class CustomThTagWorker : TdTagWorker
    {
        public CustomThTagWorker(IElementNode element, ProcessorContext context) : base(element, context) 
        {
        }
        
        public override IPropertyContainer GetElementResult() {
            Cell cell = (Cell) base.GetElementResult();
            cell.GetAccessibilityProperties().SetRole(StandardRoles.TH);
            return base.GetElementResult();
        }
    }
}