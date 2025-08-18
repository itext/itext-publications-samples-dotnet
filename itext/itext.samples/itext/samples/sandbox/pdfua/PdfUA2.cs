using System;
using System.IO;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.XMP;
using iText.Layout.Element;
using iText.Layout;
using iText.Pdfua;

namespace iText.Samples.Sandbox.Pdfua {
    public class PdfUA2 {
        public const String DEST = "results/sandbox/pdfua2/pdf_ua.pdf";

        public const String FONT = "../../../resources/font/FreeSans.ttf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new PdfUA2().ManipulatePdf(DEST);
        }

        public virtual void ManipulatePdf(String dest) {
            try {
                using (PdfDocument pdfDocument = new PdfUADocument(
                           new PdfWriter(dest, new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0)),
                           new PdfUAConfig(PdfUAConformance.PDF_UA_2, "PdfUA2 Title", "en-US")
                       )) {
                    Document document = new Document(pdfDocument);
                    PdfFont font = PdfFontFactory.CreateFont(FONT, "WinAnsi",
                        PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED);
                    document.SetFont(font);

                    Paragraph paragraph = new Paragraph("Hello PdfUA2");
                    document.Add(paragraph);
                }
            }
            catch (XMPException e) {
                //process xmp exception
            }
        }
    }
}