using System;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;

namespace iText.Samples.Sandbox.Merge
{
   
    // IncorrectExample.cs
    //
    // Example showing non-standard approach to rotating pages during merge.
    // Demonstrates specific use case; not typical best practice solution.
    // This example is named IncorrectExample because this is not how the problem of rotating pages
    // or of merging documents typically should be solved.
 
    public class IncorrectExample
    {
        public static readonly String DEST = "results/sandbox/merge/incorrect_example.pdf";

        public static readonly String SOURCE = "../../../resources/pdfs/pages.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new IncorrectExample().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument srcDoc = new PdfDocument(new PdfReader(SOURCE));
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));

            for (int i = 1; i <= srcDoc.GetNumberOfPages(); i++)
            {
                PageSize pageSize = GetPageSize(srcDoc, i);
                pdfDoc.SetDefaultPageSize(pageSize);
                PdfCanvas canvas = new PdfCanvas(pdfDoc.AddNewPage());
                PdfFormXObject page = srcDoc.GetPage(i).CopyAsFormXObject(pdfDoc);

                if (IsPortrait(srcDoc, i))
                {
                    canvas.AddXObjectAt(page, 0, 0);
                }
                else
                {
                    // Add page content as formXObject, rotated counterclockwise.
                    canvas.AddXObjectWithTransformationMatrix(page, 0, 1, -1, 0, pageSize.GetWidth(), 0);
                }
            }

            pdfDoc.Close();
            srcDoc.Close();
        }

        private static PageSize GetPageSize(PdfDocument pdfDoc, int pageNumber)
        {
            PdfPage page = pdfDoc.GetPage(pageNumber);
            Rectangle pageSize = page.GetPageSize();

            // Returns a page size with the lowest value of the dimensions of the existing page as the width
            // and the highest value as the height. This way, the page will always be in portrait.
            return new PageSize
            (
                Math.Min(pageSize.GetWidth(), pageSize.GetHeight()),
                Math.Max(pageSize.GetWidth(), pageSize.GetHeight())
            );
        }

        private static bool IsPortrait(PdfDocument pdfDoc, int pageNumber)
        {
            PdfPage page = pdfDoc.GetPage(pageNumber);

            // This method doesn't take page rotation into account.
            Rectangle pageSize = page.GetPageSize();
            return pageSize.GetHeight() > pageSize.GetWidth();
        }
    }
}