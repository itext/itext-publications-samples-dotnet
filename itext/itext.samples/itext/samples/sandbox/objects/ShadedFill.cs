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
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Colorspace;

namespace iText.Samples.Sandbox.Objects
{
    public class ShadedFill
    {
        public static readonly string DEST = "results/sandbox/objects/shaded_fill.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new ShadedFill().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));

            float x = 36;
            float y = 740;
            
            // Side of an equilateral triangle
            float side = 70;

            PdfShading.Axial axialShading = new PdfShading.Axial(new PdfDeviceCs.Rgb(), x, y,
                ColorConstants.PINK.GetColorValue(),
                x + side, y, ColorConstants.BLUE.GetColorValue());
            PdfPattern.Shading shading = new PdfPattern.Shading(axialShading);

            PdfCanvas canvas = new PdfCanvas(pdfDoc.AddNewPage());
            canvas.SetFillColorShading(shading);
            canvas.MoveTo(x, y);
            canvas.LineTo(x + side, y);
            canvas.LineTo(x + (side / 2), (float) (y + (side * Math.Sin(Math.PI / 3))));
            canvas.ClosePathFillStroke();

            pdfDoc.Close();
        }
    }
}