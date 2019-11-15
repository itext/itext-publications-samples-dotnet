/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/
using System;
using System.IO;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Highlevel.Notused {
    public class C04E10_MaryReillyV3 {
        public const String SRC = "../../resources/pdfs/jekyll_hyde.pdf";

        public const String MARY = "../../resources/img/0117002.jpg";

        public const String DEST = "../../results/chapter03/mary_reilly_V3.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C04E10_MaryReillyV3().ManipulatePdf(SRC, DEST);
        }

        public virtual void ManipulatePdf(String src, String dest) {
            PdfReader reader = new PdfReader(src);
            PdfWriter writer = new PdfWriter(dest);
            PdfDocument pdfDoc = new PdfDocument(reader, writer);
            Document document = new Document(pdfDoc);
            iText.Layout.Element.Image img = new Image(ImageDataFactory.Create(MARY), 400, 36, 1);
            document.Add(img);
            document.Close();
        }
    }
}
