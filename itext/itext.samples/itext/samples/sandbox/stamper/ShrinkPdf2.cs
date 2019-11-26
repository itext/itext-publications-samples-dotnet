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
using iText.Kernel.Pdf.Canvas;

namespace iText.Samples.Sandbox.Stamper 
{
    public class ShrinkPdf2 
    {
        public static readonly String DEST = "results/sandbox/stamper/shrink_pdf2.pdf";
        public static readonly String SRC = "../../resources/pdfs/hero.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new ShrinkPdf2().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            
            // Please note that we don't change the page size in this example, but only shrink the content (in this case to 80%)
            // and the content is shrunk to center of the page, leaving bigger margins to the top, bottom, left and right
            float percentage = 0.8f;
            for (int p = 1; p <= pdfDoc.GetNumberOfPages(); p++) 
            {
                PdfPage pdfPage = pdfDoc.GetPage(p);
                Rectangle pageSize = pdfPage.GetPageSize();
                
                // Applying the scaling in both X, Y direction to preserve the aspect ratio.
                float offsetX = (pageSize.GetWidth() * (1 - percentage)) / 2;
                float offsetY = (pageSize.GetHeight() * (1 - percentage)) / 2;
                
                // The content, placed on a content stream before, will be rendered before the other content
                // and, therefore, could be understood as a background (bottom "layer")
                new PdfCanvas(pdfPage.NewContentStreamBefore(), pdfPage.GetResources(), pdfDoc)
                        .WriteLiteral(String.Format(System.Globalization.CultureInfo.InvariantCulture, 
                                "\nq {0:f} 0 0 {1:f} {2:f} {3:f} cm\nq\n", percentage, percentage, offsetX, offsetY));
                
                // The content, placed on a content stream after, will be rendered after the other content
                // and, therefore, could be understood as a foreground (top "layer")
                new PdfCanvas(pdfPage.NewContentStreamAfter(), pdfPage.GetResources(), pdfDoc).WriteLiteral("\nQ\nQ\n");
            }
            
            pdfDoc.Close();
        }
    }
}
