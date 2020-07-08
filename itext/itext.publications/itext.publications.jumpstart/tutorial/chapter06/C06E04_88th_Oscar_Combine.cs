using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;

namespace Tutorial.Chapter06 {
    public class C06E04_88th_Oscar_Combine {
        public const String SRC1 = "../../../resources/pdf/88th_reminder_list.pdf";

        public const String SRC2 = "../../../resources/pdf/88th_noms_announcement.pdf";

        public const String DEST = "../../../results/chapter06/88th_oscar_combined_documents.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C06E04_88th_Oscar_Combine().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            //Initialize PDF document with output intent
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            PdfMerger merger = new PdfMerger(pdf);
            //Add pages from the first document
            PdfDocument firstSourcePdf = new PdfDocument(new PdfReader(SRC1));
            merger.Merge(firstSourcePdf, 1, firstSourcePdf.GetNumberOfPages());
            //Add pages from the second pdf document
            PdfDocument secondSourcePdf = new PdfDocument(new PdfReader(SRC2));
            merger.Merge(secondSourcePdf, 1, secondSourcePdf.GetNumberOfPages());
            firstSourcePdf.Close();
            secondSourcePdf.Close();
            pdf.Close();
        }
    }
}
