/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/

using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Test.Attributes;

namespace itext.publications.highlevel.itext.highlevel.chapter01 {
    /// <author>Bruno Lowagie (iText Software)</author>
    [WrapToTest]
    public class C01E07_FontSize {
        public const String DEST = "results/chapter01/font_size.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C01E07_FontSize().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            // Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document document = new Document(pdf);
            // Add content
            Text title1 = new Text("The Strange Case of ").SetFontSize(12);
            Text title2 = new Text("Dr. Jekyll and Mr. Hyde").SetFontSize(16);
            Text author = new Text("Robert Louis Stevenson");
            Paragraph p = new Paragraph().SetFontSize(8).Add(title1).Add(title2).Add(" by ").Add(author);
            document.Add(p);
            //Close document
            document.Close();
        }
    }
}
