using System;
using System.IO;
using iText.Html2pdf;
using iText.Html2pdf.Attach;
using iText.Html2pdf.Attach.Impl;
using iText.Html2pdf.Attach.Impl.Tags;
using iText.Samples.Sandbox.columncontainer;
using iText.StyledXmlParser.Node;

namespace iText.Samples.Sandbox.Pdfhtml.ColumnCount {
    public class CustomColumnRendererDivTagWorker {
        public static readonly String SRC = "../../../resources/pdfhtml/CustomColumnRendererDivTagWorker"
                                            + "/CustomColumnRendererDivTagWorker.html";

        public static readonly String DEST = "results/sandbox/pdfhtml/CustomColumnRendererDivTagWorker.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new CustomColumnRendererDivTagWorker().ManipulatePdf(SRC, DEST);
        }

        private void ManipulatePdf(String src, String dest) {
            ConverterProperties converterProperties = new ConverterProperties();
            converterProperties.SetTagWorkerFactory(new CustomTagWorkerFactory());
            HtmlConverter.ConvertToPdf(new FileInfo(src), new FileInfo(dest), converterProperties);
        }
        
        class CustomTagWorkerFactory : DefaultTagWorkerFactory {
            public override ITagWorker GetCustomTagWorker(IElementNode tag, ProcessorContext context) {
                if (tag.Name().Equals("div")) {
                    return new ColumnDivTagWorker(tag, context);
                }

                return base.GetCustomTagWorker(tag, context);
            }
        }
        
        class ColumnDivTagWorker : DivTagWorker {
            public ColumnDivTagWorker(IElementNode element, ProcessorContext context) : base(element, context) {
                this.multicolContainer.SetNextRenderer(
                    new ColumnAllowMoreRelayouts.MultiColRendererAllow10RetriesRenderer(
                        this.multicolContainer));
            }
        }
    }
}
