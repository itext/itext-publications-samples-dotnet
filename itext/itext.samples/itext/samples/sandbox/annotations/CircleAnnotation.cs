/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2019 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;

namespace iText.Samples.Sandbox.Annotations
{
    public class CircleAnnotation
    {
        public static readonly String DEST = "../../results/sandbox/annotations/circle_annotation.pdf";

        public static readonly String SRC = "../../resources/pdfs/hello.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new CircleAnnotation().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));

            Rectangle rect = new Rectangle(150, 770, 50, 50);
            PdfAnnotation annotation = new PdfCircleAnnotation(rect)
                .SetBorderStyle(PdfAnnotation.STYLE_DASHED)
                .SetDashPattern(new PdfArray(new int[] {3, 2}))

                // This method sets the text that will be displayed for the annotation or the alternate description,
                // if this type of annotation does not display text.
                .SetContents("Circle")
                .SetTitle(new PdfString("Circle"))
                .SetColor(ColorConstants.BLUE)

                // Set to print the annotation when the page is printed
                .SetFlags(PdfAnnotation.PRINT)
                .SetBorder(new PdfArray(new float[] {0, 0, 2}))

                // Set the interior color
                .Put(PdfName.IC, new PdfArray(new int[] {1, 0, 0}));
            pdfDoc.GetFirstPage().AddAnnotation(annotation);

            pdfDoc.Close();
        }
    }
}