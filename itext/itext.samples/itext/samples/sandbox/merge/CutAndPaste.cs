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
    public class CutAndPaste
    {
        public static readonly String DEST = "results/sandbox/merge/page229_cut_paste.pdf";

        public static readonly String SRC = "../../../resources/pdfs/page229.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new CutAndPaste().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument srcDoc = new PdfDocument(new PdfReader(SRC));
            Rectangle pageSize = srcDoc.GetFirstPage().GetPageSize();
            PdfDocument resultPdfDoc = new PdfDocument(new PdfWriter(dest));
            resultPdfDoc.SetDefaultPageSize(new PageSize(pageSize));
            resultPdfDoc.AddNewPage();

            PdfFormXObject pageXObject = srcDoc.GetFirstPage().CopyAsFormXObject(resultPdfDoc);
            Rectangle toMove = new Rectangle(100, 500, 100, 100);

            // Create a formXObject of a page content, in which the area to move is cut.
            PdfFormXObject formXObject1 = new PdfFormXObject(pageSize);
            PdfCanvas canvas1 = new PdfCanvas(formXObject1, resultPdfDoc);
            canvas1.Rectangle(0, 0, pageSize.GetWidth(), pageSize.GetHeight());
            canvas1.Rectangle(toMove);

            // This method uses the even-odd rule to determine which regions lie inside the clipping path.
            canvas1.EoClip();
            canvas1.EndPath();
            canvas1.AddXObject(pageXObject, 0, 0);

            // Create a formXObject of the area to move.
            PdfFormXObject formXObject2 = new PdfFormXObject(pageSize);
            PdfCanvas canvas2 = new PdfCanvas(formXObject2, resultPdfDoc);
            canvas2.Rectangle(toMove);

            // This method uses the nonzero winding rule to determine which regions lie inside the clipping path.
            canvas2.Clip();
            canvas2.EndPath();
            canvas2.AddXObject(pageXObject, 0, 0);

            PdfCanvas canvas = new PdfCanvas(resultPdfDoc.GetFirstPage());
            canvas.AddXObject(formXObject1, 0, 0);

            // Add the area to move content, shifted 10 points to the left and 2 points to the bottom.
            canvas.AddXObject(formXObject2, -20, -2);

            srcDoc.Close();
            resultPdfDoc.Close();
        }
    }
}