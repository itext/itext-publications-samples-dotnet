/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/
using System;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Test.Attributes;

namespace iText.Highlevel.Chapter07 {
    /// <author>Bruno Lowagie (iText Software)</author>
    [WrapToTest]
    public class C07E08_FullScreen {
        public const String DEST = "results/chapter07/fullscreen.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C07E08_FullScreen().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            pdf.GetCatalog().SetPageMode(PdfName.FullScreen);
            PdfViewerPreferences preferences = new PdfViewerPreferences();
            preferences.SetNonFullScreenPageMode(PdfViewerPreferences.PdfViewerPreferencesConstants.USE_THUMBS);
            pdf.GetCatalog().SetViewerPreferences(preferences);
            Document document = new Document(pdf, PageSize.A8);
            document.Add(new Paragraph("Mr. Jekyl"));
            document.Add(new AreaBreak());
            document.Add(new Paragraph("Mr. Hyde"));
            document.Close();
        }
    }
}
