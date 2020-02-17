/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2020 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Events
{
    public class Text2PdfWithFooter2
    {
        public static readonly String TEXT = "../../../resources/txt/tree.txt";
        public static readonly String DEST = "results/sandbox/events/text2pdf_with_footer2.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new Text2PdfWithFooter2().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));

            // Add a custom event handler, that draws a page number at the bottom of the page
            pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, new Footer());
            Document document = new Document(pdf).SetTextAlignment(TextAlignment.JUSTIFIED);

            ParseTextAndFillDocument(document, TEXT);

            document.Close();
        }

        private void ParseTextAndFillDocument(Document doc, String filePath)
        {
            using (StreamReader br = new StreamReader(filePath))
            {
                PdfFont normal = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
                PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);
                bool title = true;
                String line;
                while ((line = br.ReadLine()) != null)
                {
                    Paragraph paragraph;
                    if (title)
                    {
                        
                        // If the text line is a title,
                        // then create a paragraph with bold font and rounded borders
                        paragraph = new Paragraph(line)
                            .SetFont(bold)
                            .SetBorder(new SolidBorder(1))
                            .SetBorderRadius(new BorderRadius(5));
                    }
                    else
                    {
                        
                        // If the text line is not a title, then set a normal font
                        paragraph = new Paragraph(line)
                            .SetFont(normal);
                    }

                    doc.Add(paragraph);
                    title = line.Equals("");
                }
            }
        }

        private class Footer : IEventHandler
        {
            public void HandleEvent(Event @event)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent) @event;
                PdfDocument pdf = docEvent.GetDocument();
                PdfPage page = docEvent.GetPage();
                Rectangle pageSize = page.GetPageSize();

                float x = (pageSize.GetLeft() + pageSize.GetRight()) / 2;
                float y = pageSize.GetBottom() + 15;
                new Canvas(page, pageSize)
                    .ShowTextAligned(pdf.GetPageNumber(page).ToString(), x, y, TextAlignment.CENTER)
                    .Close();
            }
        }
    }
}