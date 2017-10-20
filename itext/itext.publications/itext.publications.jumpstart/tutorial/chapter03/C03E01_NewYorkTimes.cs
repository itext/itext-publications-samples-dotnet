/*
* This example is part of the iText 7 tutorial.
*/
using System;
using System.IO;
using System.Text;
using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Test.Attributes;

namespace Tutorial.Chapter03 {
    /// <summary>Simple column renderer example.</summary>
    [WrapToTest]
    public class C03E01_NewYorkTimes {
        public const String DEST = "../../results/chapter03/new_york_times.pdf";

        public const String APPLE_IMG = "../../resources/img/ny_times_apple.jpg";

        public const String APPLE_TXT = "../../resources/data/ny_times_apple.txt";

        public const String FACEBOOK_IMG = "../../resources/img/ny_times_fb.jpg";

        public const String FACEBOOK_TXT = "../../resources/data/ny_times_fb.txt";

        public const String INST_IMG = "../../resources/img/ny_times_inst.jpg";

        public const String INST_TXT = "../../resources/data/ny_times_inst.txt";

        internal static PdfFont timesNewRoman = null;

        internal static PdfFont timesNewRomanBold = null;

        /// <exception cref="System.Exception"/>
        public static void Main(String[] args) {
            timesNewRoman = PdfFontFactory.CreateFont(FontConstants.TIMES_ROMAN);
            timesNewRomanBold = PdfFontFactory.CreateFont(FontConstants.TIMES_BOLD);
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C03E01_NewYorkTimes().CreatePdf(DEST);
        }

        /// <exception cref="System.Exception"/>
        protected internal virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            PageSize ps = PageSize.A5;
            // Initialize document
            Document document = new Document(pdf, ps);
            //Set column parameters
            float offSet = 36;
            float columnWidth = (ps.GetWidth() - offSet * 2 + 10) / 3;
            float columnHeight = ps.GetHeight() - offSet * 2;
            //Define column areas
            Rectangle[] columns = new Rectangle[] {
                new Rectangle(offSet - 5, offSet, columnWidth, columnHeight),
                new Rectangle(offSet + columnWidth, offSet, columnWidth, columnHeight),
                new Rectangle(offSet + columnWidth * 2 + 5, offSet, columnWidth, columnHeight)
            };
            //
            document.SetRenderer(new ColumnDocumentRenderer(document, columns));
            Image apple = new Image(ImageDataFactory.Create(APPLE_IMG)).SetWidth(columnWidth);
            String articleApple = File.ReadAllText(System.IO.Path.Combine(APPLE_TXT), Encoding.UTF8);
            C03E01_NewYorkTimes.AddArticle(document, "Apple Encryption Engineers, if Ordered to Unlock iPhone, Might Resist"
                , "By JOHN MARKOFF MARCH 18, 2016", apple, articleApple);

            Image facebook = new Image(ImageDataFactory.Create(FACEBOOK_IMG)
                ).SetWidth(columnWidth);
            String articleFB = File.ReadAllText(System.IO.Path.Combine(FACEBOOK_TXT), Encoding.UTF8);
            C03E01_NewYorkTimes.AddArticle(document, "With \"Smog Jog\" Through Beijing, Zuckerberg Stirs Debate on Air Pollution"
                , "By PAUL MOZUR MARCH 18, 2016", facebook, articleFB);

            Image inst = new Image(ImageDataFactory.Create(INST_IMG)).SetWidth(columnWidth);
            String articleInstagram = File.ReadAllText(System.IO.Path.Combine(INST_TXT), Encoding.UTF8);
            C03E01_NewYorkTimes.AddArticle(document, "Instagram May Change Your Feed, Personalizing It With an Algorithm"
                , "By MIKE ISAAC MARCH 15, 2016", inst, articleInstagram);

            document.Close();
        }

        /// <exception cref="System.IO.IOException"/>
        public static void AddArticle(Document doc, String title, String author, iText.Layout.Element.Image img, String text) {
            Paragraph p1 = new Paragraph(title).SetFont(timesNewRomanBold).SetFontSize(14);
            doc.Add(p1);
            doc.Add(img);
            Paragraph p2 = new Paragraph().SetFont(timesNewRoman).SetFontSize(7).SetFontColor(ColorConstants.GRAY).Add(author);
            doc.Add(p2);
            Paragraph p3 = new Paragraph().SetFont(timesNewRoman).SetFontSize(10).Add(text);
            doc.Add(p3);
        }
    }
}
