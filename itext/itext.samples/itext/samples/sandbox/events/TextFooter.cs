using System;
using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Event;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Events
{
    
    // TextFooter.cs
    // 
    // This class demonstrates how to add both header and footer text to PDF pages using event handlers.
    // It creates a simple three-page document with small Helvetica Oblique text at the top and bottom
    // of each page, positioned precisely based on the document's margin settings.
 
    public class TextFooter
    {
        public static readonly String DEST = "results/sandbox/events/text_footer.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new TextFooter().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            pdfDoc.AddEventHandler(PdfDocumentEvent.END_PAGE, new TextFooterEventHandler(doc));

            for (int i = 0; i < 3; i++)
            {
                doc.Add(new Paragraph("Test " + (i + 1)));
                if (i != 2)
                {
                    doc.Add(new AreaBreak());
                }
            }

            doc.Close();
        }

        private class TextFooterEventHandler : AbstractPdfDocumentEventHandler      
        {
            protected Document doc;

            public TextFooterEventHandler(Document doc)
            {
                this.doc = doc;
            }


            protected override void OnAcceptedEvent(AbstractPdfDocumentEvent currentEvent)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent) currentEvent;
                Rectangle pageSize = docEvent.GetPage().GetPageSize();
                PdfFont font = null;
                try {
                    font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_OBLIQUE);
                } catch (IOException e) 
                {

                    // Such an exception isn't expected to occur,
                    // because helvetica is one of standard fonts
                    Console.Error.WriteLine(e.Message);
                }
                
                float coordX = ((pageSize.GetLeft() + doc.GetLeftMargin())
                                 + (pageSize.GetRight() - doc.GetRightMargin())) / 2;
                float headerY = pageSize.GetTop() - doc.GetTopMargin() + 10;
                float footerY = doc.GetBottomMargin();
                Canvas canvas = new Canvas(docEvent.GetPage(), pageSize);
                canvas
                        
                    // If the exception has been thrown, the font variable is not initialized.
                    // Therefore null will be set and iText will use the default font - Helvetica
                    .SetFont(font)
                    .SetFontSize(5)
                    .ShowTextAligned("this is a header", coordX, headerY, TextAlignment.CENTER)
                    .ShowTextAligned("this is a footer", coordX, footerY, TextAlignment.CENTER)
                    .Close();
            }
        }
    }
}