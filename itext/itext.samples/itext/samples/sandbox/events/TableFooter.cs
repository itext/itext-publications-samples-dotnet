using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Events
{
    public class TableFooter
    {
        public static readonly String DEST = "results/sandbox/events/table_footer.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new TableFooter().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc, PageSize.A4);
            doc.SetMargins(36, 36, 72, 36);

            Table table = new Table(1).UseAllAvailableWidth();

            Cell cell = new Cell().Add(new Paragraph("This is a test doc"));
            cell.SetBackgroundColor(ColorConstants.ORANGE);
            table.AddCell(cell);

            cell = new Cell().Add(new Paragraph("This is a copyright notice"));
            cell.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
            table.AddCell(cell);

            pdfDoc.AddEventHandler(PdfDocumentEvent.END_PAGE, new TableFooterEventHandler(table));

            for (int i = 0; i < 150; i++)
            {
                doc.Add(new Paragraph("Hello World!"));
            }

            doc.Add(new AreaBreak());
            doc.Add(new Paragraph("Hello World!"));
            doc.Add(new AreaBreak());
            doc.Add(new Paragraph("Hello World!"));

            doc.Close();
        }

        private class TableFooterEventHandler : IEventHandler
        {
            private Table table;

            public TableFooterEventHandler(Table table)
            {
                this.table = table;
            }

            public void HandleEvent(Event currentEvent)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent) currentEvent;
                PdfDocument pdfDoc = docEvent.GetDocument();
                PdfPage page = docEvent.GetPage();
                PdfCanvas canvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);

                new Canvas(canvas, new Rectangle(36, 20, page.GetPageSize().GetWidth() - 72, 50))
                    .Add(table)
                    .Close();
            }
        }
    }
}