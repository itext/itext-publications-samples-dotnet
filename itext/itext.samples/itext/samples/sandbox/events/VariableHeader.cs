/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2019 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.Collections.Generic;
using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Events
{
    public class VariableHeader
    {
        public static readonly String DEST = "results/sandbox/events/variable_header.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new VariableHeader().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            VariableHeaderEventHandler handler = new VariableHeaderEventHandler();
            pdfDoc.AddEventHandler(PdfDocumentEvent.END_PAGE, handler);

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

                handler.SetHeader(String.Format("THE FACTORS OF {0}", i));

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

        private class VariableHeaderEventHandler : IEventHandler
        {
            protected String header;

            public void SetHeader(String header)
            {
                this.header = header;
            }

            public void HandleEvent(Event currentEvent)
            {
                PdfDocumentEvent documentEvent = (PdfDocumentEvent) currentEvent;
                PdfPage page = documentEvent.GetPage();
                new Canvas(page, page.GetPageSize())
                    .ShowTextAligned(header, 490, 806, TextAlignment.CENTER)
                    .Close();
            }
        }
    }
}