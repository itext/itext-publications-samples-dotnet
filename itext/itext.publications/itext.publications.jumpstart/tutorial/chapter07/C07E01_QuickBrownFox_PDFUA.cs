using System;
using System.IO;
using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace Tutorial.Chapter07 {
    public class C07E01_QuickBrownFox_PDFUA {
        public const String DOG = "../../../resources/img/dog.bmp";

        public const String FOX = "../../../resources/img/fox.bmp";

        public const String FONT = "../../../resources/font/FreeSans.ttf";

        public const String DEST = "../../../results/chapter07/quick_brown_fox_PDFUA.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C07E01_QuickBrownFox_PDFUA().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest, new WriterProperties()
                .AddPdfUaXmpMetadata(PdfUAConformance.PDF_UA_1)));
            Document document = new Document(pdf);
            //Setting some required parameters
            pdf.SetTagged();
            pdf.GetCatalog().SetLang(new PdfString("en-US"));
            pdf.GetCatalog().SetViewerPreferences(new PdfViewerPreferences().SetDisplayDocTitle(true));
            PdfDocumentInfo info = pdf.GetDocumentInfo();
            info.SetTitle("iText7 PDF/UA example");
            //Fonts need to be embedded
            PdfFont font = PdfFontFactory.CreateFont(FONT, PdfEncodings.WINANSI, 
                PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED);
            Paragraph p = new Paragraph();
            p.SetFont(font);
            p.Add(new Text("The quick brown "));
            iText.Layout.Element.Image foxImage = new Image(ImageDataFactory.Create(FOX));
            //PDF/UA: Set alt text
            foxImage.GetAccessibilityProperties().SetAlternateDescription("Fox");
            p.Add(foxImage);
            p.Add(" jumps over the lazy ");
            iText.Layout.Element.Image dogImage = new iText.Layout.Element.Image(ImageDataFactory.Create(DOG));
            //PDF/UA: Set alt text
            dogImage.GetAccessibilityProperties().SetAlternateDescription("Dog");
            p.Add(dogImage);
            document.Add(p);
            document.Close();
        }
    }
}
