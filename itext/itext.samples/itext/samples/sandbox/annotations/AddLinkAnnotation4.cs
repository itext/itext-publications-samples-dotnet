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
   
    // AddLinkAnnotation4.cs
    // 
    // This class demonstrates how to create a vertically oriented hyperlink in a PDF document.
    // The code opens an existing PDF file and adds a paragraph containing a clickable
    // external link with bold formatting. The entire paragraph, including the link,
    // is rotated 90 degrees (vertically) and positioned at specific coordinates on the page.
    // This example shows how to create text with hyperlinks in non-standard orientations.
 
    public class AddLinkAnnotation4
    {
        public static readonly String DEST = "results/sandbox/annotations/add_link_annotation4.pdf";

        public static readonly String SRC = "../../../resources/pdfs/hello.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new AddLinkAnnotation4().ManipulatePdf(DEST);
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
                .Add(" and discover more than 200 questions and answers.");

            // Rotate the paragraph on 90 degrees and add it to the document.
            doc.ShowTextAligned(p, 30, 100, 1, TextAlignment.LEFT,
                VerticalAlignment.TOP, (float) Math.PI / 2);

            doc.Close();
        }
    }
}