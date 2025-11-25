using System;
using System.IO;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Fonts
{
   
    // ArabicExample2.cs
    //
    // Example showing Arabic text in table cells with RTL base direction.
    // Demonstrates right-to-left text alignment for bidirectional content.
 
    public class ArabicExample2
    {
        public static readonly String DEST = "results/sandbox/fonts/arabic_example2.pdf";
        public static readonly String FONT = "../../../resources/font/NotoNaskhArabic-Regular.ttf";

        // السعر الاجمالي
        public static readonly String ARABIC =
            "\u0627\u0644\u0633\u0639\u0631 \u0627\u0644\u0627\u062c\u0645\u0627\u0644\u064a";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ArabicExample2().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            Table table = new Table(1).UseAllAvailableWidth();
            PdfFont f = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H);

            Paragraph p = new Paragraph("test value");
            p.Add(new Text(ARABIC).SetFont(f));

            Cell cell = new Cell().Add(p);
            cell
                .SetBaseDirection(BaseDirection.RIGHT_TO_LEFT)
                .SetTextAlignment(TextAlignment.RIGHT);
            table.AddCell(cell);

            doc.Add(table);

            doc.Close();
        }
    }
}