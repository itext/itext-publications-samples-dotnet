/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/
using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Wmf;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;
using iText.Test.Attributes;

namespace iText.Highlevel.Chapter03 {
    /// <author>iText</author>
    [WrapToTest]
    public class C03E20_XObjectTypes {
        public const String WMF = "../../resources/img/test/butterfly.wmf";

        public const String SRC = "../../resources/pdfs/jekyll_hyde.pdf";

        public const String DEST = "results/chapter03/xobject_types.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C03E20_XObjectTypes().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdf);
            PdfFormXObject xObject1 = new PdfFormXObject(new WmfImageData(WMF), pdf);
            Image img1 = new Image(xObject1);
            document.Add(img1);
            PdfReader reader = new PdfReader(SRC);
            PdfDocument existing = new PdfDocument(reader);
            PdfPage page = existing.GetPage(1);
            PdfFormXObject xObject2 = page.CopyAsFormXObject(pdf);
            Image img2 = new Image(xObject2);
            img2.ScaleToFit(400, 400);
            document.Add(img2);
            document.Close();
        }
    }
}
