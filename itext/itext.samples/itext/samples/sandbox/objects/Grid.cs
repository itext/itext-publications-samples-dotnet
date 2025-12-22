using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;

namespace iText.Samples.Sandbox.Objects
{
   
    // Grid.cs
    //
    // Example showing how to create a grid of dots on a PDF page.
    // Demonstrates drawing regularly spaced circle markers using PdfCanvas.
 
    public class Grid
    {
        public static readonly string DEST = "results/sandbox/objects/grid.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new Grid().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));

            PageSize pageSize = new PageSize(612, 792);
            pdfDoc.SetDefaultPageSize(pageSize);

            PdfCanvas canvas = new PdfCanvas(pdfDoc.AddNewPage());
            for (float x = 0; x < pageSize.GetWidth(); x += 72f)
            {
                for (float y = 0; y < pageSize.GetHeight(); y += 72f)
                {
                    canvas.Circle(x, y, 1f);
                }
            }

            canvas.Fill();

            pdfDoc.Close();
        }
    }
}