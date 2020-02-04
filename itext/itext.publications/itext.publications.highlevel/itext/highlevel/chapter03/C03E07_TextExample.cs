/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/
using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Highlevel.Chapter03 {
    public class C03E07_TextExample {
        public const String DEST = "../../../results/chapter03/jekyll_hyde_text.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C03E07_TextExample().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdf);
            Text t1 = new Text("The Strange Case of ");
            Text t2 = new Text("Dr. Jekyll").SetTextRise(5);
            Text t3 = new Text(" and ").SetHorizontalScaling(2);
            Text t4 = new Text("Mr. Hyde").SetSkew(10, 45);
            document.Add(new Paragraph(t1).Add(t2).Add(t3).Add(t4));
            document.Close();
        }
    }
}
