/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2020 iText Group NV
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
    public class TileInTwo2
    {
        public static readonly String DEST = "results/sandbox/merge/tile_in_two2.pdf";

        public static readonly String SRC = "../../../resources/pdfs/united_states.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new TileInTwo2().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument srcDoc = new PdfDocument(new PdfReader(SRC));
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));

            int numberOfPages = srcDoc.GetNumberOfPages();
            for (int i = 1; i <= numberOfPages; i++)
            {
                PageSize mediaBox = GetHalfWidthPageSize(srcDoc.GetPage(i).GetPageSizeWithRotation());
                pdfDoc.SetDefaultPageSize(mediaBox);
                PdfFormXObject page = srcDoc.GetPage(i).CopyAsFormXObject(pdfDoc);

                PdfCanvas canvas = new PdfCanvas(pdfDoc.AddNewPage());
                canvas.AddXObject(page, 0, 0);

                canvas = new PdfCanvas(pdfDoc.AddNewPage());
                canvas.AddXObject(page, -mediaBox.GetWidth(), 0);
            }

            pdfDoc.Close();
            srcDoc.Close();
        }

        private static PageSize GetHalfWidthPageSize(Rectangle pageSize)
        {
            float width = pageSize.GetWidth();
            float height = pageSize.GetHeight();
            return new PageSize(width / 2, height);
        }
    }
}