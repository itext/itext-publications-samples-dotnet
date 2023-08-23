using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Highlevel.Chapter06 {
    public class C06E06_JavaScript {
        public const String DEST = "../../../results/chapter06/jekyll_hyde_javascript.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C06E06_JavaScript().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdf);
            Link link = new Link("here", PdfAction.CreateJavaScript("app.alert('Boo!');"));
            Paragraph p = new Paragraph().Add("Click ").Add(link.SetFontColor(ColorConstants.BLUE)).Add(" if you want to be scared."
                );
            document.Add(p);
            document.Close();
        }
    }
}
