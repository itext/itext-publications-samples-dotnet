using iText.Html2pdf.Attach;
using iText.Html2pdf.Attach.Impl;
using iText.StyledXmlParser.Node;

namespace iText.Samples.Sandbox.Pdfhtml.Formtagging
{
    public class FormTagWorkerFactory : DefaultTagWorkerFactory
    {
        public override ITagWorker GetCustomTagWorker(IElementNode tag, ProcessorContext context)
        {
            switch (tag.Name())
            {
                case "button":
                    return new CustomButtonTagWorker(tag, context);
                case "input":
                    return new CustomInputTagWorker(tag, context);
                case "select":
                    return new CustomSelectTagWorker(tag, context);
                case "textarea":
                    return new CustomTextAreaTagWorker(tag, context);
                default:
                    return null;
            }
        }
    }
}
