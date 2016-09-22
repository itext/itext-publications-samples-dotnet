/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/
using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;

namespace iText.Highlevel.Notused.Appendix {
    /// <author>iText</author>
    public class TextWithColoredBorder {
        public const String DEST = "results/appendix/jekyll_hyde_text.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new TextWithColoredBorder().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdf);
            document.Add(new Paragraph("Paragraph with orange border").SetBorder(new SolidBorder(Color.ORANGE, 5)));
            Text text = new Text("Text with orange border").SetBorder(new SolidBorder(Color.ORANGE, 5));
            document.Add(new Paragraph(text));
            Link link = new Link("Link with orange border", PdfAction.CreateURI("http://itextpdf.com"));
            link.SetBorder(new SolidBorder(Color.ORANGE, 5));
            document.Add(new Paragraph(link));
            document.Close();
        }
    }
}
