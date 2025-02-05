using System;
using iText.Forms.Form.Element;
using iText.Html2pdf.Attach;
using iText.Kernel.Colors;
using iText.Layout;
using iText.Layout.Borders;
using iText.StyledXmlParser.Node;

namespace iText.Samples.Sandbox.Signatures.Signaturetag
{
    public class CustomSignatureTagWorker : ITagWorker
    {
        private SignatureFieldAppearance signatureFieldAppearance;
        
        public CustomSignatureTagWorker(IElementNode tag, ProcessorContext context) {
            String signatureFieldId = tag.GetAttribute("id");
            signatureFieldAppearance = new SignatureFieldAppearance(signatureFieldId);
            signatureFieldAppearance.SetContent("Signature field");
            signatureFieldAppearance.SetBorder(new SolidBorder(ColorConstants.GREEN, 1));
            String width = tag.GetAttribute("width");
            signatureFieldAppearance.SetWidth(float.Parse(width));
            String height = tag.GetAttribute("height");
            signatureFieldAppearance.SetHeight(float.Parse(height));
            signatureFieldAppearance.SetInteractive(true);
        }
        
        public void ProcessEnd(IElementNode element, ProcessorContext context)
        {
        }

        public bool ProcessContent(string content, ProcessorContext context)
        {
            return false;
        }

        public bool ProcessTagChild(ITagWorker childTagWorker, ProcessorContext context)
        {
            return false;
        }

        public IPropertyContainer GetElementResult()
        {
            return signatureFieldAppearance;
        }
    }
}
