using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Navigation;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Annotations
{
   
    // XYZDestination.cs
    // 
    // This class demonstrates how to create internal navigation links in a PDF document 
    // using XYZ destinations. The code creates a multi-page document with test content,
    // then adds a set of links on the last page that navigate to specific coordinates
    // on each of the preceding pages. This example shows how to implement precise
    // internal navigation with custom zoom levels using explicit destinations.
 
    public class XYZDestination
    {
        public static readonly String DEST = "results/sandbox/annotations/xyz_destination.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new XYZDestination().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            for (int i = 0; i < 10; i++)
            {
                doc.Add(new Paragraph("Test"));
                doc.Add(new AreaBreak());
            }

            for (int i = 1; i < 11; i++)
            {
                // Create a link destination to the page, specified in the 1st argument.
                PdfDestination d = PdfExplicitDestination.CreateXYZ(pdfDoc.GetPage(i), 36, 806, 0);
                Paragraph c = new Paragraph(new Link("Goto page " + i, d));
                doc.Add(c);
            }

            doc.Close();
        }
    }
}