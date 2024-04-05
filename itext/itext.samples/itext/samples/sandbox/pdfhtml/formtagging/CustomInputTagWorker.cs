using iText.Forms.Form.Element;
using iText.Html2pdf.Attach;
using iText.Html2pdf.Attach.Impl.Tags;
using iText.Kernel.Colors;
using iText.Kernel.Pdf.Tagging;
using iText.Layout;
using iText.Layout.Properties;
using iText.Layout.Tagging;
using iText.StyledXmlParser.Node;

namespace iText.Samples.Sandbox.Pdfhtml.Formtagging
{
    public class CustomInputTagWorker : InputTagWorker
    {
        public CustomInputTagWorker(IElementNode element, ProcessorContext context) : base(element, context)
        {
        }
        
        public override IPropertyContainer GetElementResult()
        {
            IFormField formField = (IFormField) base.GetElementResult();
            formField.SetInteractive(false);
            formField.SetProperty(Property.BACKGROUND, new Background(new DeviceRgb(255, 255, 0)));
            IAccessibleElement accessibleElement = (IAccessibleElement)formField;
            accessibleElement.GetAccessibilityProperties().SetRole(StandardRoles.LBL);
            return formField;
        }
    }
}
