/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;

namespace iText.Samples.Sandbox.Objects
{
    public class Rectangles
    {
        public static readonly string DEST = "results/sandbox/objects/rectangles.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new Rectangles().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));

            float llx = 36;
            float lly = 700;
            float urx = 200;
            float ury = 806;

            PdfCanvas canvas = new PdfCanvas(pdfDoc.AddNewPage());
            Rectangle rect1 = new Rectangle(llx, lly, urx - llx, ury - lly);
            canvas
                .SetStrokeColor(ColorConstants.BLACK)
                .SetLineWidth(1)
                .SetFillColor(new DeviceGray(0.9f))
                .Rectangle(rect1)
                .FillStroke();

            Rectangle rect2 = new Rectangle(llx + 60, lly, urx - llx - 60, ury - 40 - lly);
            canvas
                .SetStrokeColor(ColorConstants.WHITE)
                .SetLineWidth(0.5f)
                .SetFillColor(new DeviceGray(0.1f))
                .Rectangle(rect2)
                .FillStroke();

            pdfDoc.Close();
        }
    }
}