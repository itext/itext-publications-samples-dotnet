/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/

using System;
using System.IO;
using iText.IO.Font;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Test.Attributes;

namespace itext.publications.highlevel.itext.highlevel.chapter01 {
    /// <author>Bruno Lowagie (iText Software)</author>
    [WrapToTest]
    public class C01E10_ReusingStyles {
        public const String DEST = "results/chapter01/style_example.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C01E10_ReusingStyles().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdf);
            Style normal = new Style();
            PdfFont font = PdfFontFactory.CreateFont(FontConstants.TIMES_ROMAN);
            normal.SetFont(font).SetFontSize(14);
            Style code = new Style();
            PdfFont monospace = PdfFontFactory.CreateFont(FontConstants.COURIER);
            code.SetFont(monospace).SetFontColor(Color.RED).SetBackgroundColor(Color.LIGHT_GRAY);
            Paragraph p = new Paragraph();
            p.Add(new Text("The Strange Case of ").AddStyle(normal));
            p.Add(new Text("Dr. Jekyll").AddStyle(code));
            p.Add(new Text(" and ").AddStyle(normal));
            p.Add(new Text("Mr. Hyde").AddStyle(code));
            p.Add(new Text(".").AddStyle(normal));
            document.Add(p);
            document.Close();
        }
    }
}
