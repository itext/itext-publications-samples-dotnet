/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/

using System;
using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Highlevel.Chapter01 {
    /// <author>Bruno Lowagie (iText Software)</author>
    public class C01E10_ReusingStyles {
        public const String DEST = "../../../results/chapter01/style_example.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C01E10_ReusingStyles().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdf);
            Style normal = new Style();
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
            normal.SetFont(font).SetFontSize(14);
            Style code = new Style();
            PdfFont monospace = PdfFontFactory.CreateFont(StandardFonts.COURIER);
            code.SetFont(monospace).SetFontColor(ColorConstants.RED).SetBackgroundColor(ColorConstants.LIGHT_GRAY);
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
