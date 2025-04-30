using System;
using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Annotations
{
   
    // AddLinkAnnotation2.cs
    // 
    // This class demonstrates how to create a hyperlink in a PDF document using iText's high-level API.
    // The code opens an existing PDF file and adds a paragraph containing a clickable external link
    // with custom formatting (bold text). The link points to an external URL and is positioned at 
    // specific coordinates on the page. This example shows a more user-friendly approach to creating
    // links compared to working directly with PDF annotations.
 
    public class AddLinkAnnotation2
    {
        public static readonly String DEST = "results/sandbox/annotations/add_link_annotation2.pdf";

        public static readonly String SRC = "../../../resources/pdfs/hello.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new AddLinkAnnotation2().ManipulatePdf(DEST);
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

            doc.Add(p.SetFixedPosition(36, 700, 500));

            doc.Close();
        }
    }
}