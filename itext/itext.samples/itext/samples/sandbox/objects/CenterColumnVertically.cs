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
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Objects
{
    public class CenterColumnVertically
    {
        public static readonly string DEST = "results/sandbox/objects/center_column_vertically.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new CenterColumnVertically().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));

            float llx = 50;
            float lly = 650;
            float urx = 400;
            float ury = 800;

            Rectangle rect = new Rectangle(llx, lly, urx - llx, ury - lly);
            PdfCanvas canvas = new PdfCanvas(pdfDoc.AddNewPage());
            canvas.SetStrokeColor(ColorConstants.RED)
                .SetLineWidth(0.5f)
                .Rectangle(rect)
                .Stroke();

            Paragraph p = new Paragraph("This text is displayed above the vertical middle of the red rectangle.");
            new Canvas(canvas, pdfDoc, rect)
                .Add(p.SetFixedPosition(llx, (ury + lly) / 2, urx - llx).SetMargin(0));

            pdfDoc.Close();
        }
    }
}