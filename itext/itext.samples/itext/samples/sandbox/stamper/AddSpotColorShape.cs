/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Colorspace;
using iText.Kernel.Pdf.Function;

namespace iText.Samples.Sandbox.Stamper 
{
    public class AddSpotColorShape 
    {
        public static readonly String DEST = "results/sandbox/stamper/add_spot_color_shape.pdf";
        public static readonly String SRC = "../../../resources/pdfs/image.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new AddSpotColorShape().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            PdfPage pdfPage = pdfDoc.GetFirstPage();
            
            pdfPage.SetIgnorePageRotationForContent(true);
            PdfCanvas canvas = new PdfCanvas(pdfPage);
            canvas.Arc(0, 0, 842, 595, 0, 360);
            canvas.Arc(25, 25, 817, 570, 0, 360);
            canvas.Arc(50, 50, 792, 545, 0, 360);
            canvas.Arc(75, 75, 767, 520, 0, 360);
            canvas.EoClip();
            canvas.EndPath();
            canvas.SetFillColor(new Separation(CreateCmykColorSpace(0.8f, 0.3f, 0.3f, 0.1f), 0.4f));
            canvas.Rectangle(0, 0, 842, 595);
            canvas.Fill();
            
            pdfDoc.Close();
        }

        private PdfSpecialCs.Separation CreateCmykColorSpace(float c, float m, float y, float k) 
        {
            float[] c0 = new float[] { 0, 0, 0, 0 };
            float[] c1 = new float[] { c, m, y, k };
            PdfFunction pdfFunction = new PdfFunction.Type2(new PdfArray(new float[] { 0, 1 }), 
                    null, new PdfArray(c0), new PdfArray(c1), new PdfNumber(1));
            PdfSpecialCs.Separation cs = new PdfSpecialCs.Separation("iTextSpotColorCMYK", 
                    new DeviceCmyk(c, m, y, k).GetColorSpace(), pdfFunction);
            
            return cs;
        }
    }
}
