using System;
using System.IO;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Fonts
{
    public class LiberationSans
    {
        public static readonly String DEST = "results/sandbox/fonts/liberation_sans.pdf";

        public static readonly String FONT = "../../../resources/font/LiberationSans-Regular.ttf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new LiberationSans().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            
            String fontName = "Greek-Regular";
            PdfFontFactory.Register(FONT, fontName);
            PdfFont f = PdfFontFactory.CreateRegisteredFont(fontName, PdfEncodings.CP1253, true);

            // "Νύφες"
            Paragraph p = new Paragraph("\u039d\u03cd\u03c6\u03b5\u03c2").SetFont(f);
            doc.Add(p);

            doc.Close();
        }
    }
}