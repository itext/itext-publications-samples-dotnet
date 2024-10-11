using System;
using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Event;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Events
{
    public class Text2PdfWithFooter1
    {
        public static readonly String TEXT = "../../../resources/txt/tree.txt";
        public static readonly String DEST = "results/sandbox/events/text2pdf_with_footer1.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new Text2PdfWithFooter1().ManipulatePdf(DEST);
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

        private static void ParseTextAndFillDocument(Document doc, String filePath)
        {
            using (StreamReader br = new StreamReader(filePath))
            {
                PdfFont normal = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
                PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);
                Border border = new SolidBorder(ColorConstants.BLUE, 1);
                bool title = true;
                String line;
                while ((line = br.ReadLine()) != null)
                {
                    Paragraph paragraph;
                    if (title)
                    {
                        
                        // If the text line is a title, then set a bold font and the created border
                        paragraph = new Paragraph(line)
                            .SetFont(bold)
                            .SetBorder(border);
                    }
                    else
                    {
                        
                        // If the text line is not a title, then set a normal font
                        paragraph = new Paragraph(line).SetFont(normal);
                    }

                    doc.Add(paragraph);
                    title = line.Equals("");
                }
            }
        }

        private class Footer : AbstractPdfDocumentEventHandler      
        {
            protected override void OnAcceptedEvent(AbstractPdfDocumentEvent @event)
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