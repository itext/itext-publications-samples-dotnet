/*
* This example is part of the iText 7 tutorial.
*/
using System;
using System.IO;
using iText.Forms;
using iText.Kernel.Pdf;
using iText.Test.Attributes;

namespace Tutorial.Chapter06 {
    [WrapToTest]
    public class C06E07_Combine_Forms {
        public const String DEST = "../../results/chapter06/combined_forms.pdf";

        public const String SRC1 = "../../resources/pdf/subscribe.pdf";

        public const String SRC2 = "../../resources/pdf/state.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C06E07_Combine_Forms().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            PdfDocument destPdfDocument = new PdfDocument(new PdfWriter(dest));
            PdfDocument[] sources = new PdfDocument[] { new PdfDocument(new PdfReader(SRC1)), new PdfDocument(new PdfReader
                (SRC2)) };
            PdfPageFormCopier formCopier = new PdfPageFormCopier();
            foreach (PdfDocument sourcePdfDocument in sources) {
                sourcePdfDocument.CopyPagesTo(1, sourcePdfDocument.GetNumberOfPages(), destPdfDocument, formCopier);
                sourcePdfDocument.Close();
            }
            destPdfDocument.Close();
        }
    }
}
