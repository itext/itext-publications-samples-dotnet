using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Objects
{
    public class ColoredLetters
    {
        public static readonly string DEST = "results/sandbox/objects/colored_letters.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new ColoredLetters().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            PdfFont helveticaFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            PdfFont helveticaBoldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

            Paragraph p = new Paragraph();
            string s = "all text is written in red, except the letters b and g; they are written in blue and green.";
            for (int i = 0; i < s.Length; i++)
            {
                p.Add(ReturnCorrectColor(s[i], helveticaFont, helveticaBoldFont));
            }

            doc.Add(p);

            doc.Close();
        }

        private static Text ReturnCorrectColor(char letter, PdfFont helveticaFont, PdfFont helveticaBoldFont)
        {
            if (letter == 'b')
            {
                return new Text("b")
                    .SetFontColor(ColorConstants.BLUE)
                    .SetFont(helveticaBoldFont);
            }
            else if (letter == 'g')
            {
                return new Text("g")
                    .SetFontColor(ColorConstants.GREEN)
                    .SetFont(helveticaFont)
                    .SetItalic();
            }
            else
            {
                return new Text(letter.ToString())
                    .SetFontColor(ColorConstants.RED)
                    .SetFont(helveticaFont);
            }
        }
    }
}