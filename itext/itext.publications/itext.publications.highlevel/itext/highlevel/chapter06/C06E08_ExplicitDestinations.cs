/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks.
*/
using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Navigation;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Test.Attributes;

namespace iText.Highlevel.Chapter06 {
    /// <author>iText</author>
    [WrapToTest]
    public class C06E08_ExplicitDestinations {
        public const String DEST = "../../results/chapter06/jekyll_hyde_explicit.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C06E08_ExplicitDestinations().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdf);
            PdfDestination jekyll = PdfExplicitDestination.CreateFitH(1, 416);
            PdfDestination hyde = PdfExplicitDestination.CreateXYZ(1, 150, 516, 2);
            PdfDestination jekyll2 = PdfExplicitDestination.CreateFitR(2, 50, 380, 130, 440);
            document.Add(new Paragraph().Add(new Link("Link to Dr. Jekyll", jekyll)));
            document.Add(new Paragraph().Add(new Link("Link to Mr. Hyde", hyde)));
            document.Add(new Paragraph().Add(new Link("Link to Dr. Jekyll on page 2", jekyll2)));
            document.Add(new Paragraph().SetFixedPosition(50, 400, 80).Add("Dr. Jekyll"));
            document.Add(new Paragraph().SetFixedPosition(150, 500, 80).Add("Mr. Hyde"));
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            document.Add(new Paragraph().SetFixedPosition(50, 400, 80).Add("Dr. Jekyll on page 2"));
            document.Close();
        }
    }
}
