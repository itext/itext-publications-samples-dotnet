using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Event;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Events
{
    public class PageBackgrounds
    {
        public static readonly String DEST = "results/sandbox/events/page_backgrounds.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new PageBackgrounds().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            pdfDoc.AddEventHandler(PdfDocumentEvent.START_PAGE, new PageBackgroundsEventHandler());
            Document doc = new Document(pdfDoc);

            doc.Add(new Paragraph("Prime Numbers"));
            doc.Add(new AreaBreak());
            doc.Add(new Paragraph("An overview"));
            doc.Add(new AreaBreak());
            for (int i = 2; i < 301; i++)
            {
                List<int> factors = GetFactors(i);
                if (factors.Count == 1)
                {
                    doc.Add(new Paragraph("This is a prime number!"));
                }

                foreach (int factor in factors)
                {
                    doc.Add(new Paragraph("Factor: " + factor));
                }

                if (300 != i)
                {
                    doc.Add(new AreaBreak());
                }
            }

            doc.Close();
        }

        private static List<int> GetFactors(int n)
        {
            List<int> factors = new List<int>();
            for (int i = 2; i <= n; i++)
            {
                while (n % i == 0)
                {
                    factors.Add(i);
                    n /= i;
                }
            }

            return factors;
        }

        private class PageBackgroundsEventHandler : AbstractPdfDocumentEventHandler      
        {
            protected override void OnAcceptedEvent(AbstractPdfDocumentEvent currentEvent)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent) currentEvent;
                PdfPage page = docEvent.GetPage();

                int pageNumber = docEvent.GetDocument().GetNumberOfPages();

                // Background color will be applied to the first page and all even pages
                if (pageNumber % 2 == 1 && pageNumber != 1)
                {
                    return;
                }

                PdfCanvas canvas = new PdfCanvas(page);
                Rectangle rect = page.GetPageSize();
                canvas
                    .SaveState()
                    .SetFillColor(pageNumber < 3 ? ColorConstants.BLUE : ColorConstants.LIGHT_GRAY)
                    .Rectangle(rect)
                    .FillStroke()
                    .RestoreState();
            }
        }
    }
}