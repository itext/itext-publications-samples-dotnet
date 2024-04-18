using iText.Html2pdf.Attach;
using iText.Html2pdf.Attach.Impl.Tags;
using iText.Layout;
using iText.StyledXmlParser.Node;
using iText.Forms.Form.Element;
using iText.Kernel.Colors;
using iText.Kernel.Pdf.Tagging;

namespace iText.Samples.Sandbox.Pdfhtml.Formtagging
{
    public class CustomButtonTagWorker : ButtonTagWorker
    {
        public CustomButtonTagWorker(IElementNode element, ProcessorContext context) : base(element, context)
        {
        }
        
        public override IPropertyContainer GetElementResult()
        {
            FormField<Button> formField = (FormField<Button>) base.GetElementResult();
            formField.SetInteractive(false);
            formField.SetBackgroundColor(ColorConstants.GREEN);
            formField.GetAccessibilityProperties().SetRole(StandardRoles.ARTIFACT);
            return formField;
        }
    }
}
