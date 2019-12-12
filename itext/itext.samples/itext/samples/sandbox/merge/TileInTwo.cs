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
    public class TileInTwo
    {
        public static readonly String DEST = "results/sandbox/merge/tile_in_two.pdf";

        public static readonly String SRC = "../../resources/pdfs/united_states.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new TileInTwo().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument srcDoc = new PdfDocument(new PdfReader(SRC));
            PdfDocument resultDoc = new PdfDocument(new PdfWriter(dest));

            int numberOfPages = srcDoc.GetNumberOfPages();
            for (int i = 1; i <= numberOfPages; i++)
            {
                PageSize mediaBox = GetHalfHeightPageSize(srcDoc.GetPage(i).GetPageSizeWithRotation());
                resultDoc.SetDefaultPageSize(mediaBox);
                PdfFormXObject page = srcDoc.GetPage(i).CopyAsFormXObject(resultDoc);

                PdfCanvas canvas = new PdfCanvas(resultDoc.AddNewPage());
                canvas.AddXObject(page, 0, -mediaBox.GetHeight());

                canvas = new PdfCanvas(resultDoc.AddNewPage());
                canvas.AddXObject(page, 0, 0);
            }

            resultDoc.Close();
            srcDoc.Close();
        }

        private static PageSize GetHalfHeightPageSize(Rectangle pageSize)
        {
            float width = pageSize.GetWidth();
            float height = pageSize.GetHeight();
            return new PageSize(width, height / 2);
        }
    }
}