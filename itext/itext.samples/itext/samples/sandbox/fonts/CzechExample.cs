using System;
using System.IO;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Fonts
{
   
    // CzechExample.cs
    //
    // Example showing Czech special characters with different encodings.
    // Demonstrates CP1250 vs Unicode encoding for diacritical marks.
 
    public class CzechExample
    {
        public static readonly String DEST = "results/sandbox/fonts/czech_example.pdf";
        public static readonly String FONT = "../../../resources/font/FreeSans.ttf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new CzechExample().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            PdfFont f1 = PdfFontFactory.CreateFont(FONT, PdfEncodings.CP1250, PdfFontFactory.EmbeddingStrategy.PREFER_EMBEDDED);

            // "Č,Ć,Š,Ž,Đ"
            Paragraph p1 = new Paragraph("Testing of letters \u010c,\u0106,\u0160,\u017d,\u0110").SetFont(f1);
            doc.Add(p1);

            PdfFont f2 = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H);

            // "Č,Ć,Š,Ž,Đ"
            Paragraph p2 = new Paragraph("Testing of letters \u010c,\u0106,\u0160,\u017d,\u0110").SetFont(f2);
            doc.Add(p2);

            doc.Close();
        }
    }
}