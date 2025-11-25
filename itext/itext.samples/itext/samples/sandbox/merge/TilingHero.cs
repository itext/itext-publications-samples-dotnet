using System;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;

namespace iText.Samples.Sandbox.Merge
{
   
    // TilingHero.cs
    //
    // Example showing how to create tiled pages from a scaled-up source page.
    // Demonstrates splitting a 4x scaled page into 16 individual tiles.
 
    public class TilingHero
    {
        public static readonly String DEST = "results/sandbox/merge/tiling_hero.pdf";

        public static readonly String RESOURCE = "../../../resources/pdfs/hero.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new TilingHero().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument srcDoc = new PdfDocument(new PdfReader(RESOURCE));
            PdfDocument resultDoc = new PdfDocument(new PdfWriter(dest));
            PdfPage srcFirstPage = srcDoc.GetFirstPage();

            Rectangle pageSize = srcFirstPage.GetPageSizeWithRotation();
            float width = pageSize.GetWidth();
            float height = pageSize.GetHeight();

            // The top left rectangle of the tiled pdf picture
            Rectangle mediaBox = new Rectangle(0, 3 * height, width, height);
            resultDoc.SetDefaultPageSize(new PageSize(mediaBox));

            PdfFormXObject page = srcFirstPage.CopyAsFormXObject(resultDoc);
            for (int i = 1; i <= 16; i++)
            {
                PdfCanvas canvas = new PdfCanvas(resultDoc.AddNewPage());
                canvas.AddXObjectWithTransformationMatrix(page, 4, 0, 0, 4, 0, 0);

                float xCoordinate = (i % 4) * width;
                float yCoordinate = (4 - (i / 4)) * height;
                mediaBox = new Rectangle(xCoordinate, yCoordinate, width, -height);
                resultDoc.SetDefaultPageSize(new PageSize(mediaBox));
            }

            srcDoc.Close();
            resultDoc.Close();
        }
    }
}