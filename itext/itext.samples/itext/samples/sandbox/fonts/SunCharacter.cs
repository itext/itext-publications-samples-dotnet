using System;
using System.IO;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Fonts
{
   
    // SunCharacter.cs
    //
    // This example demonstrates rendering a special Unicode character (sun symbol ☉) in a PDF document using the Cardo font.
    // The sample shows how to embed a TrueType font that supports specific Unicode glyphs not available in standard fonts.
 
    public class SunCharacter
    {
        public static readonly String DEST = "results/sandbox/fonts/sun_character.pdf";

        public static readonly String FONT = "../../../resources/font/Cardo-Regular.ttf";

        // "The Cardo family of fonts supports this character: ☉"
        public static readonly String TEXT = "The Cardo family of fonts supports this character: \u2609";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new SunCharacter().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            PdfFont font = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H);
            Paragraph p = new Paragraph(TEXT).SetFont(font);
            doc.Add(p);

            doc.Close();
        }
    }
}