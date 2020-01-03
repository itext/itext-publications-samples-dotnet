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
    public class TilingHero
    {
        public static readonly String DEST = "results/sandbox/merge/tiling_hero.pdf";

        public static readonly String RESOURCE = "../../resources/pdfs/hero.pdf";

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
                canvas.AddXObject(page, 4, 0, 0, 4, 0, 0);

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