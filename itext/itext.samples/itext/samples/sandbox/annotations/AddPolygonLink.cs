/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2019 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Annot;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Navigation;
using iText.Layout;

namespace iText.Samples.Sandbox.Annotations
{
    public class AddPolygonLink
    {
        public static readonly String DEST = "results/sandbox/annotations/add_polygon_link.pdf";

        public static readonly String SRC = "../../resources/pdfs/hello.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new AddPolygonLink().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            PdfPage firstPage = pdfDoc.GetFirstPage();

            PdfCanvas canvas = new PdfCanvas(firstPage);
            canvas
                .MoveTo(36, 700)
                .LineTo(72, 760)
                .LineTo(144, 720)
                .LineTo(72, 730)
                .ClosePathStroke();

            Rectangle linkLocation = new Rectangle(36, 700, 144, 760);

            // Make the link destination page fit to the display
            PdfExplicitDestination destination = PdfExplicitDestination.CreateFit(firstPage);
            PdfLinkAnnotation linkAnnotation = new PdfLinkAnnotation(linkLocation)

                // Set highlighting type which is enabled after a click on the annotation
                .SetHighlightMode(PdfAnnotation.HIGHLIGHT_INVERT)

                // Create a link to the first page of the document.
                .SetAction(PdfAction.CreateGoTo(destination));
            PdfArray arrayOfQuadPoints = new PdfArray(new int[] {72, 730, 144, 720, 72, 760, 36, 700});
            linkAnnotation.Put(PdfName.QuadPoints, arrayOfQuadPoints);
            
            firstPage.AddAnnotation(linkAnnotation);

            doc.Close();
        }
    }
}