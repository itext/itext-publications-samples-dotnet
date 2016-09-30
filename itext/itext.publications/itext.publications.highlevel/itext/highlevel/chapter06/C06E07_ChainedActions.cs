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
using iText.Layout.Element;
using iText.Test.Attributes;

namespace iText.Highlevel.Chapter06 {
    /// <author>iText</author>
    [WrapToTest]
    public class C06E07_ChainedActions {
        public const String DEST = "results/chapter06/jekyll_hyde_chained.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C06E07_ChainedActions().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdf);
            PdfAction action = PdfAction.CreateJavaScript("app.alert('Boo');");
            action.Next(PdfAction.CreateGoToR(new FileInfo(C06E04_TOC_GoToNamed.DEST).Name, 1, true));
            Link link = new Link("here", action);
            Paragraph p = new Paragraph().Add("Click ").Add(link.SetFontColor(Color.BLUE)).Add(" if you want to be scared."
                );
            document.Add(p);
            document.Close();
        }
    }
}
