using System;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Event;
using iText.Layout.Element;

namespace iText.Highlevel.Chapter07 {
    public class C07E05_AddRemovePages {
        public const String SRC = "../../../resources/pdfs/jekyll_hyde_bookmarked.pdf";

        public const String DEST = "../../../results/chapter07/jekyll_hyde_updated.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C07E05_AddRemovePages().ManipulatePdf(SRC, DEST);
        }

        public virtual void ManipulatePdf(String src, String dest) {
            PdfReader reader = new PdfReader(src);
            PdfWriter writer = new PdfWriter(dest);
            PdfDocument pdf = new PdfDocument(reader, writer);
            pdf.AddEventHandler(PdfDocumentEvent.INSERT_PAGE, new AddPageHandler());
            pdf.AddEventHandler(PdfDocumentEvent.REMOVE_PAGE, new RemovePageHandler());
            pdf.AddNewPage(1, PageSize.A4);
            int total = pdf.GetNumberOfPages();
            for (int i = 9; i <= total; i++) {
                pdf.RemovePage(9);
                if (i == 12) {
                    pdf.RemoveAllHandlers();
                }
            }
            pdf.Close();
        }

        protected internal class AddPageHandler : AbstractPdfDocumentEventHandler {
            protected override void OnAcceptedEvent(AbstractPdfDocumentEvent @event) {
                PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
                PdfDocument pdf = docEvent.GetDocument();
                PdfPage page = docEvent.GetPage();
                PdfCanvas pdfCanvas = new PdfCanvas(page);
                iText.Layout.Canvas canvas = new iText.Layout.Canvas(pdfCanvas, page.GetPageSize());
                canvas.Add(new Paragraph().Add(docEvent.GetType()));
            }

            internal AddPageHandler() {
            }

        }

        protected internal class RemovePageHandler : AbstractPdfDocumentEventHandler {
            protected override void OnAcceptedEvent(AbstractPdfDocumentEvent @event) {
                PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
                System.Console.Out.WriteLine(docEvent.GetType());
            }

            internal RemovePageHandler() {
            }
        }
    }
}
