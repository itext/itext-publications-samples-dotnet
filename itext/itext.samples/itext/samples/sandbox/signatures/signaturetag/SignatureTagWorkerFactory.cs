using iText.Html2pdf.Attach;
using iText.Html2pdf.Attach.Impl;
using iText.StyledXmlParser.Node;

namespace iText.Samples.Sandbox.Signatures.Signaturetag
{
    public class SignatureTagWorkerFactory : DefaultTagWorkerFactory
    {
        public override ITagWorker GetCustomTagWorker(IElementNode tag, ProcessorContext context)
        {
            if (tag.Name().Equals("signature-field"))
            {
                return new CustomSignatureTagWorker(tag, context);
            }
            return base.GetCustomTagWorker(tag, context);
        }
    }
}
