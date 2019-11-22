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
    public class ClipPdf 
    {
        public static readonly String DEST = "results/sandbox/stamper/clip_pdf.pdf";
        public static readonly String SRC = "../../resources/pdfs/hero.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new ClipPdf().ManipulatePdf(DEST);
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
                float llx = media.GetX() + 200;
                float lly = media.GetY() + 200;
                float w = media.GetWidth() - 400;
                float h = media.GetHeight() - 400;
                
                // It's important to write explicit Locale settings, because decimal separator differs in
                // different regions and in PDF only dot is respected
                String command = String.Format(System.Globalization.CultureInfo.InvariantCulture, 
                        
                                // re operator constructs a rectangle
                                // W operator - sets the clipping path
                                // n operator - starts a new path
                                // q, Q - operators save and restore the graphics state stack
                                "\nq {0:f} {1:f} {2:f} {3:f} re W n\nq\n", llx, lly, w, h);
                
                //q 200.00 200.00 195.00 442.00 re W n q
                
                // The content, placed on a content stream before, will be rendered before the other content
                // and, therefore, could be understood as a background (bottom "layer")
                PdfPage pdfPage = pdfDoc.GetPage(p);
                new PdfCanvas(pdfPage.NewContentStreamBefore(), pdfPage.GetResources(), pdfDoc)
                        .WriteLiteral(command);
                
                // The content, placed on a content stream after, will be rendered after the other content
                // and, therefore, could be understood as a foreground (top "layer")
                new PdfCanvas(pdfPage.NewContentStreamAfter(), pdfPage.GetResources(), pdfDoc)
                        .WriteLiteral("\nQ\nQ\n");
            }
            
            pdfDoc.Close();
        }
    }
}
