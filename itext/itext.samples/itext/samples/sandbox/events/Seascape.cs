using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Event;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Events
{
    public class Seascape
    {
        public static readonly String DEST = "results/sandbox/events/seascape.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new Seascape().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            pdfDoc.AddEventHandler(PdfDocumentEvent.START_PAGE, new SeascapeEventHandler());

            for (int i = 0; i < 50; i++)
            {
                doc.Add(new Paragraph("Hello World!"));
            }

            doc.Add(new AreaBreak());
            doc.Add(new Paragraph("Hello World!"));
            doc.Add(new AreaBreak());
            doc.Add(new Paragraph("Hello World!"));

            doc.Close();
        }

        private class SeascapeEventHandler : AbstractPdfDocumentEventHandler      
        {
            protected override void OnAcceptedEvent(AbstractPdfDocumentEvent currentEvent)
            {
                PdfDocumentEvent documentEvent = (PdfDocumentEvent) currentEvent;
                documentEvent.GetPage().SetRotation(270);
            }
        }
    }
}