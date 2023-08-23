using System;
using System.IO;
using iText.Kernel.Events;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Highlevel.Chapter07 {
    public class C07E01_EventHandlers {
        public const String DEST = "../../../results/chapter07/jekyll_hyde_page_orientation.pdf";

        public static readonly PdfNumber PORTRAIT = new PdfNumber(0);

        public static readonly PdfNumber LANDSCAPE = new PdfNumber(90);

        public static readonly PdfNumber INVERTEDPORTRAIT = new PdfNumber(180);

        public static readonly PdfNumber SEASCAPE = new PdfNumber(270);

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C07E01_EventHandlers().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            pdf.GetCatalog().SetPageLayout(PdfName.TwoColumnLeft);
            C07E01_EventHandlers.PageRotationEventHandler eventHandler = new PageRotationEventHandler();
            pdf.AddEventHandler(PdfDocumentEvent.START_PAGE, eventHandler);
            Document document = new Document(pdf, PageSize.A8);
            document.Add(new Paragraph("Dr. Jekyll"));
            eventHandler.SetRotation(INVERTEDPORTRAIT);
            document.Add(new AreaBreak());
            document.Add(new Paragraph("Mr. Hyde"));
            eventHandler.SetRotation(LANDSCAPE);
            document.Add(new AreaBreak());
            document.Add(new Paragraph("Dr. Jekyll"));
            eventHandler.SetRotation(SEASCAPE);
            document.Add(new AreaBreak());
            document.Add(new Paragraph("Mr. Hyde"));
            document.Close();
        }

        protected internal class PageRotationEventHandler : IEventHandler {
            protected internal PdfNumber rotation = C07E01_EventHandlers.PORTRAIT;

            public virtual void SetRotation(PdfNumber orientation) {
                this.rotation = orientation;
            }

            public virtual void HandleEvent(Event @event) {
                PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
                docEvent.GetPage().Put(PdfName.Rotate, this.rotation);
            }

            internal PageRotationEventHandler() {
            }
        }
    }
}
