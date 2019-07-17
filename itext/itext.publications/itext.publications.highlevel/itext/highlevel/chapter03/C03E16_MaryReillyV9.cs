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

namespace iText.Highlevel.Chapter03 {
    public class C03E16_MaryReillyV9 {
        public const String MARY = "../../resources/img/0117002.jpg";

        public const String DEST = "../../results/chapter03/mary_reilly_V9.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C03E16_MaryReillyV9().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdf);
            Paragraph p = new Paragraph("Mary Reilly is a maid in the household of Dr. Jekyll: ");
            iText.Layout.Element.Image img = new Image(ImageDataFactory.Create(MARY));
            p.Add(img);
            document.Add(p);
            document.Close();
        }
    }
}
