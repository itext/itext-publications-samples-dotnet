using iText.Forms.Form.Element;
using iText.Html2pdf.Attach;
using iText.Html2pdf.Attach.Impl.Tags;
using iText.Kernel.Colors;
using iText.Kernel.Pdf.Tagging;
using iText.Layout;
using iText.StyledXmlParser.Node;

namespace iText.Samples.Sandbox.Pdfhtml.Formtagging
{
    public class CustomTextAreaTagWorker : TextAreaTagWorker
    {
        public CustomTextAreaTagWorker(IElementNode element, ProcessorContext context) : base(element, context)
        {
        }
        
        public override IPropertyContainer GetElementResult()
        {
            FormField<TextArea> formField = (FormField<TextArea>) base.GetElementResult();
            formField.SetInteractive(false);
            formField.SetBackgroundColor(ColorConstants.BLUE);
            formField.GetAccessibilityProperties().SetRole(StandardRoles.H);
            return formField;
        }
    }
}
