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
using iText.Layout.Properties;
using iText.Test.Attributes;

namespace iText.Highlevel.Chapter03 {
    /// <author>Bruno Lowagie (iText Software)</author>
    [WrapToTest]
    public class C03E14_MaryReillyV7 {
        public const String SRC = "../../resources/pdfs/jekyll_hyde.pdf";

        public const String MARY = "../../resources/img/0117002.jpg";

        public const String DEST = "../../results/chapter03/mary_reilly_V7.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C03E14_MaryReillyV7().ManipulatePdf(SRC, DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void ManipulatePdf(String src, String dest) {
            PdfReader reader = new PdfReader(src);
            PdfWriter writer = new PdfWriter(dest);
            PdfDocument pdfDoc = new PdfDocument(reader, writer);
            Document document = new Document(pdfDoc);
            iText.Layout.Element.Image img = new Image(ImageDataFactory.Create(MARY));
            img.SetFixedPosition(1, 350, 750, UnitValue.CreatePointValue(50));
            document.Add(img);
            document.Close();
        }
    }
}
