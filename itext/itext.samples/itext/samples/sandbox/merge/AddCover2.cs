/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2020 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.IO;
using iText.Forms;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Merge
{
    public class AddCover2
    {
        public static readonly String DEST = "results/sandbox/merge/add_cover2.pdf";

        public static readonly String COVER = "../../resources/pdfs/hero.pdf";
        public static readonly String RESOURCE = "../../resources/pdfs/pages.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new AddCover2().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(RESOURCE), new PdfWriter(dest));
            PdfDocument cover = new PdfDocument(new PdfReader(COVER));

            // Copier contains the additional logic to copy acroform fields to a new page.
            // PdfPageFormCopier uses some caching logic which can potentially improve performance
            // in case of the reusing of the same instance.
            PdfPageFormCopier formCopier = new PdfPageFormCopier();
            cover.CopyPagesTo(1, 1, pdfDoc, 1, formCopier);

            cover.Close();
            pdfDoc.Close();
        }
    }
}