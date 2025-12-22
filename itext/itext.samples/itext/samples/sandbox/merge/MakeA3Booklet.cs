using System;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;

namespace iText.Samples.Sandbox.Merge
{
   
    // MakeA3Booklet.cs
    //
    // Example showing how to create an A3 booklet from A4 pages.
    // Demonstrates placing two A4 pages side-by-side on A3 landscape sheets.
 
    public class MakeA3Booklet
    {
        public static readonly String DEST = "results/sandbox/merge/make_a3_booklet.pdf";

        public static readonly String SRC = "../../../resources/pdfs/primes.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new MakeA3Booklet().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument srcDoc = new PdfDocument(new PdfReader(SRC));
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            pdfDoc.SetDefaultPageSize(PageSize.A3.Rotate());

            PdfCanvas canvas = new PdfCanvas(pdfDoc.AddNewPage());
            float a4Width = PageSize.A4.GetWidth();
            int numberOfPages = srcDoc.GetNumberOfPages();
            int i = 0;
            while (i++ < numberOfPages)
            {
                PdfFormXObject page = srcDoc.GetPage(i).CopyAsFormXObject(pdfDoc);
                if (i % 2 == 1)
                {
                    canvas.AddXObjectAt(page, 0, 0);
                }
                else
                {
                    canvas.AddXObjectAt(page, a4Width, 0);
                    canvas = new PdfCanvas(pdfDoc.AddNewPage());
                }
            }

            pdfDoc.Close();
            srcDoc.Close();
        }
    }
}