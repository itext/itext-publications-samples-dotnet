/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/
using System;
using System.IO;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;

namespace iText.Highlevel.Notused.Appendix {
    /// <author>Bruno Lowagie (iText Software)</author>
    public class ImageProperties {
        public const String TEST1 = "../../resources/img/test/amb.jb2";

        public const String TEST2 = "../../resources/img/test/butterfly.bmp";

        public const String TEST3 = "../../resources/img/test/hitchcock.gif";

        public const String TEST4 = "../../resources/img/test/hitchcock.png";

        public const String TEST5 = "../../resources/img/test/info.png";

        public const String TEST6 = "../../resources/img/test/marbles.tif";

        public const String DEST = "results/appendix/image_properties.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new ImageProperties().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            // Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdf);
            iText.Layout.Element.Image img1 = new Image(ImageDataFactory.Create(TEST1));
            img1.ScaleToFit(100, 100).SetDestination("Top");
            document.Add(img1);
            iText.Layout.Element.Image img2 = new iText.Layout.Element.Image(ImageDataFactory.Create(TEST2));
            img2.SetHeight(300);
            document.Add(img2);
            iText.Layout.Element.Image img3 = new iText.Layout.Element.Image(ImageDataFactory.Create(TEST3));
            img3.ScaleToFit(100, 100);
            img3.SetBackgroundColor(ColorConstants.BLUE);
            document.Add(img3);
            iText.Layout.Element.Image img4 = new iText.Layout.Element.Image(ImageDataFactory.Create(TEST4));
            img4.ScaleToFit(100, 100);
            img4.SetBackgroundColor(ColorConstants.RED);
            document.Add(img4);
            iText.Layout.Element.Image img5 = new iText.Layout.Element.Image(ImageDataFactory.Create(TEST5));
            img5.ScaleToFit(50, 50);
            Style style = new Style();
            style.SetBorderRight(new SolidBorder(2));
            img5.AddStyle(style);
            document.Add(img5);
            iText.Layout.Element.Image img6 = new iText.Layout.Element.Image(ImageDataFactory.Create(TEST6));
            PdfAction top = PdfAction.CreateGoTo("Top");
            img6.ScaleToFit(100, 100).SetAction(top);
            document.Add(img6);
            document.Close();
        }
    }
}
