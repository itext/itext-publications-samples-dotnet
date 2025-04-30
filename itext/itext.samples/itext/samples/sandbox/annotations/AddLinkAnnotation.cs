using System;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Annot;
using iText.Kernel.Pdf.Navigation;

namespace iText.Samples.Sandbox.Annotations
{
   
    // AddLinkAnnotation.cs
    // 
    // This class demonstrates how to add a link annotation to a PDF document.
    // The code opens an existing PDF file and adds a clickable link to the first page
    // that navigates to the third page when clicked. The link area is defined by a 
    // rectangle, and the annotation is configured with specific visual properties
    // including highlight mode and border appearance.
 
    public class AddLinkAnnotation
    {
        public static readonly String DEST = "results/sandbox/annotations/add_link_annotation.pdf";

        public static readonly String SRC = "../../../resources/pdfs/primes.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new AddLinkAnnotation().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            Rectangle linkLocation = new Rectangle(523, 770, 36, 36);
            int[] borders = {0, 0, 1};

            // Make the link destination page fit to the display
            PdfExplicitDestination destination = PdfExplicitDestination.CreateFit(pdfDoc.GetPage(3));
            PdfAnnotation annotation = new PdfLinkAnnotation(linkLocation)

                // Set highlighting type which is enabled after a click on the annotation
                .SetHighlightMode(PdfAnnotation.HIGHLIGHT_INVERT)

                // Add link to the 3rd page.
                .SetAction(PdfAction.CreateGoTo(destination))
                .SetBorder(new PdfArray(borders));
            pdfDoc.GetFirstPage().AddAnnotation(annotation);

            pdfDoc.Close();
        }
    }
}