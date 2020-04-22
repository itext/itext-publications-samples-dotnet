using System;
using System.IO;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Highlevel.Chapter03 {
    public class C03E12_MaryReillyV5 {
        public const String MARY = "../../../resources/img/0117002.jpg";

        public const String DEST = "../../../results/chapter03/mary_reilly_V5.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C03E12_MaryReillyV5().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdf);
            Paragraph p = new Paragraph("Mary Reilly is a maid in the household of Dr. Jekyll: ");
            document.Add(p);
            iText.Layout.Element.Image img = new Image(ImageDataFactory.Create(MARY));
            img.SetFixedPosition(320, 750, UnitValue.CreatePointValue(50));
            document.Add(img);
            document.Close();
        }
    }
}
