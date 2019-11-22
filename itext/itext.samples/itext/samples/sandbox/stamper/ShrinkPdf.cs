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
    public class ShrinkPdf 
    {
        public static readonly String DEST = "results/sandbox/stamper/shrink_pdf.pdf";
        public static readonly String SRC = "../../resources/pdfs/hero.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new ShrinkPdf().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            
            for (int p = 1; p <= pdfDoc.GetNumberOfPages(); p++) 
            {
                PdfPage page = pdfDoc.GetPage(p);
                Rectangle media = page.GetCropBox();
                if (media == null) 
                {
                    media = page.GetMediaBox();
                }
                
                // Shrink the page to 50%
                Rectangle crop = new Rectangle(0, 0, media.GetWidth() / 2, media.GetHeight() / 2);
                page.SetMediaBox(crop);
                page.SetCropBox(crop);
                
                // The content, placed on a content stream before, will be rendered before the other content
                // and, therefore, could be understood as a background (bottom "layer")
                new PdfCanvas(page.NewContentStreamBefore(), 
                        page.GetResources(), pdfDoc).WriteLiteral("\nq 0.5 0 0 0.5 0 0 cm\nq\n");
                
                // The content, placed on a content stream after, will be rendered after the other content
                // and, therefore, could be understood as a foreground (top "layer")
                new PdfCanvas(page.NewContentStreamAfter(),
                        page.GetResources(), pdfDoc).WriteLiteral("\nQ\nQ\n");
            }
            
            pdfDoc.Close();
        }
    }
}
