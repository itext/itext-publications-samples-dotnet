using System;
using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Annotations
{
   
    // AddLinkAnnotation3.cs
    // 
    // This class demonstrates how to create a rotated hyperlink in a PDF document.
    // The code opens an existing PDF file and adds a paragraph containing a clickable
    // external link with custom formatting (bold text). The entire paragraph, including
    // the link, is then rotated 30 degrees and positioned at specific coordinates on the page.
    // This example showcases advanced text positioning and formatting with hyperlinks.
 
    public class AddLinkAnnotation3
    {
        public static readonly String DEST = "results/sandbox/annotations/add_link_annotation3.pdf";

        public static readonly String SRC = "../../../resources/pdfs/hello.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new AddLinkAnnotation3().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

            Link link = new Link("The Best iText Questions on StackOverflow",
                PdfAction.CreateURI("https://kb.itextpdf.com/home/it7kb/ebooks/best-itext-7-questions-on-stackoverflow"));
            link.SetFont(bold);
            Paragraph p = new Paragraph("Download ")
                .Add(link)
                .Add(" and discover \nmore than 200 questions and answers.");

            // Rotate the paragraph on 30 degrees and add it to the document.
            doc.ShowTextAligned(p, 30, 600, 1, TextAlignment.LEFT,
                VerticalAlignment.TOP, (float) Math.PI / 6);

            doc.Close();
        }
    }
}