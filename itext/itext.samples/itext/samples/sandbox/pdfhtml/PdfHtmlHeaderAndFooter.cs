/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.Html2pdf;
using iText.Kernel.Events;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Pdfhtml
{
    public class PdfHtmlHeaderAndFooter
    {
        public static readonly string SRC = "../../resources/pdfhtml/";
        public static readonly string DEST = "results/sandbox/pdfhtml/ipsum.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            string htmlSource = SRC + "ipsum.html";

            new PdfHtmlHeaderAndFooter().ManipulatePdf(htmlSource, DEST);
        }

        public void ManipulatePdf(string htmlSource, string pdfDest)
        {
            PdfWriter writer = new PdfWriter(pdfDest);
            PdfDocument pdfDocument = new PdfDocument(writer);
            string header = "pdfHtml Header and footer example using page-events";
            Header headerHandler = new Header(header);
            Footer footerHandler = new Footer();

            pdfDocument.AddEventHandler(PdfDocumentEvent.START_PAGE, headerHandler);
            pdfDocument.AddEventHandler(PdfDocumentEvent.END_PAGE, footerHandler);

            // Base URI is required to resolve the path to source files
            ConverterProperties converterProperties = new ConverterProperties().SetBaseUri(SRC);
            HtmlConverter.ConvertToDocument(new FileStream(htmlSource, FileMode.Open), pdfDocument, converterProperties);

            // Write the total number of pages to the placeholder
            footerHandler.WriteTotal(pdfDocument);
            pdfDocument.Close();
        }

        // Header event handler
        protected class Header : IEventHandler
        {
            private string header;

            public Header(string header)
            {
                this.header = header;
            }

            public virtual void HandleEvent(Event @event)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent) @event;
                PdfDocument pdf = docEvent.GetDocument();

                PdfPage page = docEvent.GetPage();
                Rectangle pageSize = page.GetPageSize();

                Canvas canvas = new Canvas(new PdfCanvas(page), pdf, pageSize);
                canvas.SetFontSize(18);

                // Write text at position
                canvas.ShowTextAligned(header,
                    pageSize.GetWidth() / 2,
                    pageSize.GetTop() - 30, TextAlignment.CENTER);
                canvas.Close();
            }
        }

        // Footer event handler
        protected class Footer : IEventHandler
        {
            protected PdfFormXObject placeholder;
            protected float side = 20;
            protected float x = 300;
            protected float y = 25;
            protected float space = 4.5f;
            protected float descent = 3;

            public Footer()
            {
                placeholder = new PdfFormXObject(new Rectangle(0, 0, side, side));
            }

            public virtual void HandleEvent(Event @event)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent) @event;
                PdfDocument pdf = docEvent.GetDocument();
                PdfPage page = docEvent.GetPage();
                int pageNumber = pdf.GetPageNumber(page);
                Rectangle pageSize = page.GetPageSize();

                // Creates drawing canvas
                PdfCanvas pdfCanvas = new PdfCanvas(page);
                Canvas canvas = new Canvas(pdfCanvas, pdf, pageSize);

                Paragraph p = new Paragraph()
                    .Add("Page ")
                    .Add(pageNumber.ToString())
                    .Add(" of");

                canvas.ShowTextAligned(p, x, y, TextAlignment.RIGHT);
                canvas.Close();

                // Create placeholder object to write number of pages
                pdfCanvas.AddXObject(placeholder, x + space, y - descent);
                pdfCanvas.Release();
            }

            public void WriteTotal(PdfDocument pdf)
            {
                Canvas canvas = new Canvas(placeholder, pdf);
                canvas.ShowTextAligned(pdf.GetNumberOfPages().ToString(),
                    0, descent, TextAlignment.LEFT);
                canvas.Close();
            }
        }
    }
}