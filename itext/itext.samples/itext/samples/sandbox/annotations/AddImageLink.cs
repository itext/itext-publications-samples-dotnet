/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2019 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.IO;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Annot;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Navigation;

namespace iText.Samples.Sandbox.Annotations
{
    public class AddImageLink
    {
        public static readonly String DEST = "results/sandbox/annotations/add_image_link.pdf";

        public static readonly String IMG = "../../resources/img/info.png";
        public static readonly String SRC = "../../resources/pdfs/primes.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new AddImageLink().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            ImageData img = ImageDataFactory.Create(IMG);
            float x = 10;
            float y = 650;
            float width = img.GetWidth();
            float height = img.GetHeight();
            PdfPage firstPage = pdfDoc.GetFirstPage();

            PdfCanvas canvas = new PdfCanvas(firstPage);
            canvas.AddImage(img, x, y, false);

            Rectangle linkLocation = new Rectangle(x, y, width, height);

            // Make the link destination page fit to the display
            PdfExplicitDestination destination = PdfExplicitDestination.CreateFit(pdfDoc.GetLastPage());
            PdfAnnotation annotation = new PdfLinkAnnotation(linkLocation)

                // Set highlighting type which is enabled after a click on the annotation
                .SetHighlightMode(PdfAnnotation.HIGHLIGHT_INVERT)

                // Create a link to the last page of the document.
                .SetAction(PdfAction.CreateGoTo(destination))
                .SetBorder(new PdfArray(new float[] {0, 0, 0}));
            firstPage.AddAnnotation(annotation);

            pdfDoc.Close();
        }
    }
}