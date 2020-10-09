using System;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;

namespace iText.Samples.Sandbox.Merge
{
    public class MakeBookletA3
    {
        public static readonly String DEST = "results/sandbox/merge/make_booklet_a3.pdf";

        public static readonly String SRC = "../../../resources/pdfs/primes.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new MakeBookletA3().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument srcDoc = new PdfDocument(new PdfReader(SRC));
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));

            float a4_width = PageSize.A4.GetWidth();
            PageSize pageSize = new PageSize(a4_width * 2, PageSize.A4.GetHeight());
            pdfDoc.SetDefaultPageSize(pageSize);

            int numberOfPages = srcDoc.GetNumberOfPages();
            int p = 1;
            PdfCanvas canvas = new PdfCanvas(pdfDoc.AddNewPage());
            while ((p - 1) <= numberOfPages)
            {
                CopyPage(canvas, srcDoc, pdfDoc, p + 3, 0);
                CopyPage(canvas, srcDoc, pdfDoc, p, a4_width);

                canvas = new PdfCanvas(pdfDoc.AddNewPage());
                CopyPage(canvas, srcDoc, pdfDoc, p + 1, 0);
                CopyPage(canvas, srcDoc, pdfDoc, p + 2, a4_width);

                if ((p - 1) / 4 < numberOfPages / 4)
                {
                    canvas = new PdfCanvas(pdfDoc.AddNewPage());
                }

                p += 4;
            }

            pdfDoc.Close();
            srcDoc.Close();
        }

        private static void CopyPage(PdfCanvas canvas, PdfDocument srcDoc, PdfDocument pdfDoc,
            int pageNumber, float offsetX)
        {
            if (pageNumber > srcDoc.GetNumberOfPages())
            {
                return;
            }

            PdfFormXObject page = srcDoc.GetPage(pageNumber).CopyAsFormXObject(pdfDoc);
            canvas.AddXObjectAt(page, offsetX, 0);
        }
    }
}