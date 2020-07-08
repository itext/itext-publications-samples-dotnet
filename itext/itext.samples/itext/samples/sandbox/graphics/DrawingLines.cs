using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;

namespace iText.Samples.Sandbox.Graphics
{
    public class DrawingLines
    {
        public static readonly string DEST = "results/sandbox/graphics/drawing_lines.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new DrawingLines().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            PdfCanvas canvas = new PdfCanvas(pdfDoc.AddNewPage());

            // Create a 100% Magenta color
            Color magentaColor = new DeviceCmyk(0f, 1f, 0f, 0f);
            canvas
                .SetStrokeColor(magentaColor)
                .MoveTo(36, 36)
                .LineTo(36, 806)
                .LineTo(559, 36)
                .LineTo(559, 806)
                .ClosePathStroke();

            pdfDoc.Close();
        }
    }
}