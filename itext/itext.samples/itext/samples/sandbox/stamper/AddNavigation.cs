/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
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
using iText.Kernel.Pdf.Navigation;

namespace iText.Samples.Sandbox.Stamper 
{
    public class AddNavigation 
    {
        public static readonly String DEST = "results/sandbox/stamper/add_navigation.pdf";
        public static readonly String SRC = "../../../resources/pdfs/primes.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new AddNavigation().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            
            int[] borders = new int[] { 0, 0, 1 };
            PdfDestination pdfDestination = PdfExplicitDestination.CreateFit(pdfDoc.GetPage(10));
            Rectangle rect = new Rectangle(0, 806, 595, 36);
            PdfAnnotation a10 = new PdfLinkAnnotation(rect)
                    .SetAction(PdfAction.CreateGoTo(pdfDestination))
                    .SetHighlightMode(PdfAnnotation.HIGHLIGHT_INVERT)
                    .SetPage(pdfDoc.GetPage(10)).SetBorder(new PdfArray(borders))
                    .SetColor(new PdfArray(new float[] { 0, 1, 0 }));
            pdfDoc.GetPage(1).AddAnnotation(a10);
            PdfDestination d2 = PdfExplicitDestination.CreateFit(pdfDoc.GetPage(1));
            PdfAnnotation a1 = new PdfLinkAnnotation(rect)
                    .SetAction(PdfAction.CreateGoTo(d2))
                    .SetHighlightMode(PdfAnnotation.HIGHLIGHT_PUSH)
                    .SetPage(pdfDoc.GetPage(1))
                    .SetBorder(new PdfArray(borders))
                    .SetColor(new PdfArray(new float[] { 0, 1, 0 }));
            pdfDoc.GetPage(10).AddAnnotation(a1);
            
            pdfDoc.Close();
        }
    }
}
