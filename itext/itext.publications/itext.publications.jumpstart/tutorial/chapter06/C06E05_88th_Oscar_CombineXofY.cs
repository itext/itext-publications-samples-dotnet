/*
* This example is part of the iText 7 tutorial.
*/
using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;

namespace Tutorial.Chapter06 {
    public class C06E05_88th_Oscar_CombineXofY {
        public const String SRC1 = "../../../resources/pdf/88th_reminder_list.pdf";

        public const String SRC2 = "../../../resources/pdf/88th_noms_announcement.pdf";

        public const String DEST = "../../../results/chapter06/88th_oscar_combined_documents_xy_pages.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C06E05_88th_Oscar_CombineXofY().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            //Initialize PDF document with output intent
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            PdfMerger merger = new PdfMerger(pdf);
            //Add pages from the first document
            PdfDocument firstSourcePdf = new PdfDocument(new PdfReader(SRC1));
            merger.Merge(firstSourcePdf, iText.IO.Util.JavaUtil.ArraysAsList(1, 5, 7, 1));
            //Add pages from the second pdf document
            PdfDocument secondSourcePdf = new PdfDocument(new PdfReader(SRC2));
            merger.Merge(secondSourcePdf, iText.IO.Util.JavaUtil.ArraysAsList(1, 15));
            firstSourcePdf.Close();
            secondSourcePdf.Close();
            pdf.Close();
        }
    }
}
