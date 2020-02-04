/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2020 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Samples.Sandbox.Merge.Densemerger;

namespace iText.Samples.Sandbox.Merge
{
    public class PdfDenseMergeExample
    {
        public static readonly String DEST = "results/sandbox/merge/denseMergeExample.pdf";

        public static readonly String SRC1 = "../../../resources/pdfs/merge1.pdf";
        public static readonly String SRC2 = "../../../resources/pdfs/merge2.pdf";
        public static readonly String SRC3 = "../../../resources/pdfs/merge3.pdf";
        public static readonly String SRC4 = "../../../resources/pdfs/merge4.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new PdfDenseMergeExample().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            List<PdfDocument> mergeList = InitSourceDocuments();

            PdfDenseMerger merger = new PdfDenseMerger(pdfDoc);
            merger
                .SetTopMargin(doc.GetTopMargin())
                .SetBottomMargin(doc.GetBottomMargin())
                .SetGap(10);
            foreach (PdfDocument src in mergeList)
            {
                merger.AddPages(src, 1, src.GetNumberOfPages());
            }

            pdfDoc.Close();
            foreach (PdfDocument src in mergeList)
            {
                src.Close();
            }
        }

        private static List<PdfDocument> InitSourceDocuments()
        {
            List<PdfDocument> list = new List<PdfDocument>();
            list.Add(new PdfDocument(new PdfReader(SRC1)));
            list.Add(new PdfDocument(new PdfReader(SRC2)));
            list.Add(new PdfDocument(new PdfReader(SRC3)));
            list.Add(new PdfDocument(new PdfReader(SRC4)));
            return list;
        }
    }
}