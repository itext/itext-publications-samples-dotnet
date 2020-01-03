/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2020 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.Collections.Generic;
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
    public class PageBorder
    {
        public static readonly String DEST = "results/sandbox/events/page_border.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new PageBorder().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            pdfDoc.AddEventHandler(PdfDocumentEvent.START_PAGE, new PageBorderEventHandler());
            Document doc = new Document(pdfDoc);

            for (int i = 2; i < 301; i++)
            {
                IList<int> factors = GetFactors(i);
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

        private static IList<int> GetFactors(int n)
        {
            IList<int> factors = new List<int>();
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

        private class PageBorderEventHandler : IEventHandler
        {
            public void HandleEvent(Event currentEvent)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent) currentEvent;
                PdfCanvas canvas = new PdfCanvas(docEvent.GetPage());
                Rectangle rect = docEvent.GetPage().GetPageSize();

                canvas
                    .SetLineWidth(5)
                    .SetStrokeColor(ColorConstants.RED)
                    .Rectangle(rect)
                    .Stroke();
            }
        }
    }
}