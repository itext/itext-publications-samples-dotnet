using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Annot;
using iText.Kernel.Pdf.Navigation;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Annotations
{
   
    // AddLinkAnnotation5.cs
    // 
    // This class demonstrates how to create an internal navigation link in a PDF document.
    // The code opens an existing PDF file and adds a descriptive text link that navigates
    // to the third page when clicked. It combines high-level layout elements with low-level
    // annotation properties to create a positioned link with custom highlight behavior.
    // The example shows how to create internal document navigation with precise control
    // over link appearance and positioning.
 
    public class AddLinkAnnotation5
    {
        public static readonly String SRC = "../../../resources/pdfs/primes.pdf";

        public static readonly String DEST = "results/sandbox/annotations/add_link_annotation5.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new AddLinkAnnotation5().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            // Make the link destination page fit to the display
            PdfExplicitDestination destination = PdfExplicitDestination.CreateFit(pdfDoc.GetPage(3));
            Link link = new Link(
                "This is a link. Click it and you'll be forwarded to another page in this document.",

                // Add link to the 3rd page.
                PdfAction.CreateGoTo(destination));

            // Set highlighting type which is enabled after a click on the annotation
            link.GetLinkAnnotation().SetHighlightMode(PdfAnnotation.HIGHLIGHT_INVERT);
            Paragraph p = new Paragraph(link).SetWidth(240);
            doc.ShowTextAligned(p, 320, 695, 1, TextAlignment.LEFT,
                VerticalAlignment.TOP, 0);

            doc.Close();
        }
    }
}