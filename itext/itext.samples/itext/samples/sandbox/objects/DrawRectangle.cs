using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;

namespace iText.Samples.Sandbox.Objects
{
   
    // DrawRectangle.cs
    //
    // Example showing how to draw a rectangle on a PDF page using PdfCanvas.
    // Demonstrates basic shape drawing with custom stroke width.
 
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