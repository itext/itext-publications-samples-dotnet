using System;
using System.IO;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Fonts
{
   
    // SmallCapsExample.cs
    //
    // This example demonstrates rendering text in a PDF document using a Small Caps font variant.
    // The sample loads and applies the Delicious-SmallCaps OpenType font with Unicode encoding to display text in small capital letters.
 
    public class SmallCapsExample
    {
        public static readonly String DEST = "results/sandbox/fonts/small_caps_example.pdf";

        public static readonly String FONT = "../../../resources/font/Delicious-SmallCaps.otf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new SmallCapsExample().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            PdfFont font = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H);
            Paragraph p = new Paragraph("This is some text displayed using a Small Caps font.")
                .SetFont(font);
            doc.Add(p);

            doc.Close();
        }
    }
}