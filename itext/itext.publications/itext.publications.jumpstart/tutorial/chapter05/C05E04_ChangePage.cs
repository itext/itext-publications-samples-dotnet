using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;

namespace Tutorial.Chapter05 {
    /// <summary>Simple changing page properties example.</summary>
    public class C05E04_ChangePage {
        public const String SRC = "../../../resources/pdf/ufo.pdf";

        public const String DEST = "../../../results/chapter05/change_page.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C05E04_ChangePage().ManipulatePdf(SRC, DEST);
        }

        public virtual void ManipulatePdf(String src, String dest) {
            //Initialize PDF document
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(src), new PdfWriter(dest));
            float margin = 72;
            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++) {
                PdfPage page = pdfDoc.GetPage(i);
                // change page size
                Rectangle mediaBox = page.GetMediaBox();
                Rectangle newMediaBox = new Rectangle(mediaBox.GetLeft() - margin, mediaBox.GetBottom() - margin, mediaBox
                    .GetWidth() + margin * 2, mediaBox.GetHeight() + margin * 2);
                page.SetMediaBox(newMediaBox);
                // add border
                PdfCanvas over = new PdfCanvas(page);
                over.SetStrokeColor(ColorConstants.GRAY);
                over.Rectangle(mediaBox.GetLeft(), mediaBox.GetBottom(), mediaBox.GetWidth(), mediaBox.GetHeight());
                over.Stroke();
                // change rotation of the even pages
                if (i % 2 == 0) {
                    page.SetRotation(180);
                }
            }
            pdfDoc.Close();
        }
    }
}
