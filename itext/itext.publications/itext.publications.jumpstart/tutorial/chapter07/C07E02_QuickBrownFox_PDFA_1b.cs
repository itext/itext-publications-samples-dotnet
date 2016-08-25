/*
* PDF/A-1a example
*/
using System;
using System.IO;
using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Pdfa;

namespace Tutorial.Chapter07 {
    public class C07E02_QuickBrownFox_PDFA_1b {
        public const String DOG = "resources/img/dog.bmp";

        public const String FOX = "resources/img/fox.bmp";

        public const String FONT = "resources/font/FreeSans.ttf";

        public const String INTENT = "resources/color/sRGB_CS_profile.icm";

        public const String DEST = "results/chapter07/quick_brown_fox_PDFA-1b.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C07E02_QuickBrownFox_PDFA_1b().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            //Initialize PDFA document with output intent
            PdfADocument pdf = new PdfADocument(new PdfWriter(dest), PdfAConformanceLevel.PDF_A_1B, new PdfOutputIntent
                ("Custom", "", "http://www.color.org", "sRGB IEC61966-2.1", new FileStream(INTENT, FileMode.Open, FileAccess.Read
                )));
            Document document = new Document(pdf);
            //Fonts need to be embedded
            PdfFont font = PdfFontFactory.CreateFont(FONT, PdfEncodings.WINANSI, true);
            Paragraph p = new Paragraph();
            p.SetFont(font);
            p.Add(new Text("The quick brown "));
            iText.Layout.Element.Image foxImage = new Image(ImageDataFactory.Create(FOX));
            p.Add(foxImage);
            p.Add(" jumps over the lazy ");
            iText.Layout.Element.Image dogImage = new iText.Layout.Element.Image(ImageDataFactory.Create(DOG));
            p.Add(dogImage);
            document.Add(p);
            document.Close();
        }
    }
}
