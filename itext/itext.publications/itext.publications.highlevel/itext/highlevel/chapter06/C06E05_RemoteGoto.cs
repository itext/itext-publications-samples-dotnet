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
    public class C06E05_RemoteGoto {
        public const String DEST = "../../results/chapter06/jekyll_hyde_remote.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C06E05_RemoteGoto().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdf);
            Link link1 = new Link("Strange Case of Dr. Jekyll and Mr. Hyde", PdfAction.CreateGoToR(new FileInfo(C06E04_TOC_GoToNamed
                .DEST).Name, 1, true));
            Link link2 = new Link("table of contents", PdfAction.CreateGoToR(new FileInfo(C06E04_TOC_GoToNamed.DEST).Name
                , "toc", false));
            Paragraph p = new Paragraph().Add("Read the amazing horror story ").Add(link1.SetFontColor(Color.BLUE)).Add
                (" or, if you're too afraid to start reading the story, read the ").Add(link2.SetFontColor(Color.BLUE)
                ).Add(".");
            document.Add(p);
            document.Close();
        }
    }
}
