using System;
using System.IO;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Fonts
{
    public class RupeeSymbol
    {
        public static readonly String DEST = "results/sandbox/fonts/rupee_symbol.pdf";

        public static readonly String FONT1 = "../../../resources/font/PlayfairDisplay-Regular.ttf";
        public static readonly String FONT2 = "../../../resources/font/PT_Sans-Web-Regular.ttf";
        public static readonly String FONT3 = "../../../resources/font/FreeSans.ttf";

        // "The Rupee character ₹ and the Rupee symbol ₨"
        public static readonly String RUPEE = "The Rupee character \u20B9 and the Rupee symbol \u20A8";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new RupeeSymbol().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            PdfFont font1 = PdfFontFactory.CreateFont(FONT1, PdfEncodings.IDENTITY_H);
            PdfFont font2 = PdfFontFactory.CreateFont(FONT2, PdfEncodings.IDENTITY_H);
            PdfFont font3 = PdfFontFactory.CreateFont(FONT3, PdfEncodings.IDENTITY_H);
            PdfFont font4 = PdfFontFactory.CreateFont(FONT3, PdfEncodings.WINANSI, true);

            doc.Add(new Paragraph(RUPEE).SetFont(font1));
            doc.Add(new Paragraph(RUPEE).SetFont(font2));
            doc.Add(new Paragraph(RUPEE).SetFont(font3));
            doc.Add(new Paragraph(RUPEE).SetFont(font4));

            doc.Close();
        }
    }
}