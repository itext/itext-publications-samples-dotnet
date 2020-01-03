/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;

namespace iText.Samples.Sandbox.Objects
{
    public class DrawRectangle
    {
        public static readonly string DEST = "results/sandbox/objects/draw_rectangle.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new DrawRectangle().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));

            PdfCanvas canvas = new PdfCanvas(pdfDoc.AddNewPage());
            Rectangle rect = new Rectangle(36, 36, 523, 770);
            canvas.SetLineWidth(2);
            canvas.Rectangle(rect);
            canvas.Stroke();

            pdfDoc.Close();
        }
    }
}