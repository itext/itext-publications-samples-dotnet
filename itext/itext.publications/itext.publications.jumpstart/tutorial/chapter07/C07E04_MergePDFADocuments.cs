using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using iText.Pdfa;
using iText.Test.Attributes;

namespace Tutorial.Chapter07 {
    [WrapToTest]
    public class C07E04_MergePDFADocuments {
        public const String INTENT = "../../resources/color/sRGB_CS_profile.icm";

        public const String SRC1 = "../../resources/pdf/quick_brown_fox_PDFA-1a.pdf";

        public const String SRC2 = "../../resources/pdf/united_states_PDFA-1a.pdf";

        public const String DEST = "../../results/chapter07/merged_PDFA-1a_documents.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C07E04_MergePDFADocuments().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            //Initialize PDFA document with output intent
            PdfADocument pdf = new PdfADocument(new PdfWriter(dest), PdfAConformanceLevel.PDF_A_1A, new PdfOutputIntent
                ("Custom", "", "http://www.color.org", "sRGB IEC61966-2.1", new FileStream(INTENT, FileMode.Open, FileAccess.Read
                )));
            //Setting some required parameters
            pdf.SetTagged();
            pdf.GetCatalog().SetLang(new PdfString("en-US"));
            pdf.GetCatalog().SetViewerPreferences(new PdfViewerPreferences().SetDisplayDocTitle(true));
            PdfDocumentInfo info = pdf.GetDocumentInfo();
            info.SetTitle("iText7 PDF/A-1a example");
            //Create PdfMerger instance
            PdfMerger merger = new PdfMerger(pdf);
            //Add pages from the first document
            PdfDocument firstSourcePdf = new PdfDocument(new PdfReader(SRC1));
            merger.Merge(firstSourcePdf, 1, firstSourcePdf.GetNumberOfPages());
            //Add pages from the second pdf document
            PdfDocument secondSourcePdf = new PdfDocument(new PdfReader(SRC2));
            merger.Merge(secondSourcePdf, 1, secondSourcePdf.GetNumberOfPages());
            //Close the documents
            firstSourcePdf.Close();
            secondSourcePdf.Close();
            pdf.Close();
        }
    }
}
