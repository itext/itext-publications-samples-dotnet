using System;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;

namespace iText.Samples.Sandbox.Merge
{
   
    // MakeBooklet.cs
    //
    // Example showing how to create a booklet with 16-page signatures.
    // Demonstrates arranging pages for printing and folding into booklets.
 
    public class MakeBooklet
    {
        public static readonly String DEST = "results/sandbox/merge/make_booklet.pdf";

        public static readonly String SRC = "../../../resources/pdfs/primes.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new MakeBooklet().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument srcDoc = new PdfDocument(new PdfReader(SRC));
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));

            float a4Width = PageSize.A4.GetWidth();
            float a4Height = PageSize.A4.GetHeight();
            PageSize pagesize = new PageSize(a4Width * 4, a4Height * 2);
            pdfDoc.SetDefaultPageSize(pagesize);

            int numberOfPages = srcDoc.GetNumberOfPages();
            int p = 1;
            PdfCanvas canvas = new PdfCanvas(pdfDoc.AddNewPage());
            while ((p - 1) <= numberOfPages)
            {
                CopyPageToDoc(canvas, srcDoc, pdfDoc, p + 3, 0);
                CopyPageToDoc(canvas, srcDoc, pdfDoc, p + 12, a4Width);
                CopyPageToDoc(canvas, srcDoc, pdfDoc, p + 15, a4Width * 2);
                CopyPageToDoc(canvas, srcDoc, pdfDoc, p, a4Width * 3);
                canvas.SaveState();

                // Rotate on 180 degrees and copy pages to the top row.
                AffineTransform at = AffineTransform.GetRotateInstance((float) -Math.PI);
                at.Concatenate(AffineTransform.GetTranslateInstance(0, -a4Height * 2));
                canvas.ConcatMatrix(at);
                CopyPageToDoc(canvas, srcDoc, pdfDoc, p + 4, -a4Width);
                CopyPageToDoc(canvas, srcDoc, pdfDoc, p + 11, -a4Width * 2);
                CopyPageToDoc(canvas, srcDoc, pdfDoc, p + 8, -a4Width * 3);
                CopyPageToDoc(canvas, srcDoc, pdfDoc, p + 7, -a4Width * 4);
                canvas.RestoreState();

                canvas = new PdfCanvas(pdfDoc.AddNewPage());
                CopyPageToDoc(canvas, srcDoc, pdfDoc, p + 1, 0);
                CopyPageToDoc(canvas, srcDoc, pdfDoc, p + 14, a4Width);
                CopyPageToDoc(canvas, srcDoc, pdfDoc, p + 13, a4Width * 2);
                CopyPageToDoc(canvas, srcDoc, pdfDoc, p + 2, a4Width * 3);
                canvas.SaveState();

                // Rotate on 180 degrees and copy pages to the top row.
                canvas.ConcatMatrix(at);
                CopyPageToDoc(canvas, srcDoc, pdfDoc, p + 6, -a4Width);
                CopyPageToDoc(canvas, srcDoc, pdfDoc, p + 9, -a4Width * 2);
                CopyPageToDoc(canvas, srcDoc, pdfDoc, p + 10, -a4Width * 3);
                CopyPageToDoc(canvas, srcDoc, pdfDoc, p + 5, -a4Width * 4);
                canvas.RestoreState();

                if ((p - 1) / 16 < numberOfPages / 16)
                {
                    canvas = new PdfCanvas(pdfDoc.AddNewPage());
                }

                p += 16;
            }

            pdfDoc.Close();
            srcDoc.Close();
        }

        private static void CopyPageToDoc(PdfCanvas canvas, PdfDocument srcDoc, PdfDocument pdfDoc,
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