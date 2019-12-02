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
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Colorspace;

namespace iText.Samples.Sandbox.Graphics
{
    public class GradientTopToBottom
    {
        public static readonly string DEST = "results/sandbox/graphics/gradient_top_to_bottom.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new GradientTopToBottom().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PageSize pageSize = new PageSize(150, 300);
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            pdfDoc.SetDefaultPageSize(pageSize);

            PdfCanvas canvas = new PdfCanvas(pdfDoc.AddNewPage());
            PdfShading.Axial axial = new PdfShading.Axial(new PdfDeviceCs.Rgb(), 0, pageSize.GetHeight(),
                ColorConstants.WHITE.GetColorValue(), 0, 0, ColorConstants.GREEN.GetColorValue());
            PdfPattern.Shading pattern = new PdfPattern.Shading(axial);
            canvas.SetFillColorShading(pattern);
            canvas.Rectangle(0, 0, pageSize.GetWidth(), pageSize.GetHeight());
            canvas.Fill();

            pdfDoc.Close();
        }
    }
}