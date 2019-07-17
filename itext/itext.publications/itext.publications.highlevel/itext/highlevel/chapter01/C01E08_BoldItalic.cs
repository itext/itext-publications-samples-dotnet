/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/

using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Highlevel.Chapter01 {
    /// <author>Bruno Lowagie (iText Software)</author>
    public class C01E08_BoldItalic {
        public const String DEST = "../../results/chapter01/bold_italic.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C01E08_BoldItalic().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            // Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document document = new Document(pdf);
            // Add content
            Text title1 = new Text("The Strange Case of ").SetItalic();
            Text title2 = new Text("Dr. Jekyll and Mr. Hyde").SetBold();
            Text author = new Text("Robert Louis Stevenson").SetItalic().SetBold();
            Paragraph p = new Paragraph().Add(title1).Add(title2).Add(" by ").Add(author);
            document.Add(p);
            //Close document
            document.Close();
        }
    }
}
