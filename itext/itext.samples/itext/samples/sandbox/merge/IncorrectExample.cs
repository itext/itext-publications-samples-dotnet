/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2019 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;

namespace iText.Samples.Sandbox.Merge
{
    public class IncorrectExample
    {
        public static readonly String DEST = "results/sandbox/merge/incorrect_example.pdf";

        public static readonly String SOURCE = "../../resources/pdfs/pages.pdf";

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
                    canvas.AddXObject(page, 0, 0);
                }
                else
                {
                    // Add page content as formXObject, rotated counterclockwise.
                    canvas.AddXObject(page, 0, 1, -1, 0, pageSize.GetWidth(), 0);
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