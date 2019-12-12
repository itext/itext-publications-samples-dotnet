/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2019 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;

namespace iText.Samples.Sandbox.Merge
{
    public class AddCover1
    {
        public static readonly String DEST = "results/sandbox/merge/add_cover.pdf";

        public static readonly String COVER = "../../resources/pdfs/hero.pdf";
        public static readonly String RESOURCE = "../../resources/pdfs/pages.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new AddCover1().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            PdfDocument cover = new PdfDocument(new PdfReader(COVER));
            PdfDocument resource = new PdfDocument(new PdfReader(RESOURCE));

            PdfMerger merger = new PdfMerger(pdfDoc);
            merger.Merge(cover, 1, 1);
            merger.Merge(resource, 1, resource.GetNumberOfPages());

            // Source documents can be closed implicitly after merging,
            // by passing true to the setCloseSourceDocuments(boolean) method
            cover.Close();
            resource.Close();
            
            // The resultant pdf doc will be closed implicitly.
            merger.Close();
        }
    }
}