using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Annotations
{
   
    // RelativeLink.cs
    // 
    // This class demonstrates how to create a PDF document with a hyperlink that
    // references a relative file path. The code creates a simple PDF with a "Click me"
    // text link that, when clicked, attempts to open an XML file using a relative path.
    // This example shows how to create external URI actions that point to local resources
    // using relative paths rather than absolute URLs.
 
    public class RelativeLink
    {
        public static readonly String DEST = "results/sandbox/annotations/relative_link.pdf";

        public static readonly String XML = "../../../../../../resources/xml/data.xml";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new RelativeLink().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Paragraph chunk = new Paragraph(new Link("Click me", PdfAction.CreateURI(XML)));
            doc.Add(chunk);

            doc.Close();
        }
    }
}