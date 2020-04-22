using System.IO;
using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Objects
{
    public class Bullet
    {
        public static readonly string DEST = "results/sandbox/objects/bullets.pdf";
        public static readonly string[] ITEMS =
        {
            "Insurance system", "Agent", "Agency", "Agent Enrollment", "Agent Settings",
            "Appointment", "Continuing Education", "Hierarchy", "Recruiting", "Contract",
            "Message", "Correspondence", "Licensing", "Party"
        };

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new Bullet().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            PdfFont zapfdingbatsFont = PdfFontFactory.CreateFont(StandardFonts.ZAPFDINGBATS);

            Text bullet = new Text(((char) 108).ToString()).SetFont(zapfdingbatsFont);
            PdfFont helveticaFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            
            // No-Break Space prevents breaking a text in this place
            char space = '\u00a0';

            Paragraph p = new Paragraph("Items can be split if they don't fit at the end: ").SetFont(helveticaFont);
            foreach (string item in ITEMS)
            {
                p.Add(bullet);
                p.Add(new Text(" " + item + " ")).SetFont(helveticaFont);
            }

            doc.Add(p);
            doc.Add(new Paragraph("\n"));

            p = new Paragraph("Items can't be split if they don't fit at the end: ").SetFont(helveticaFont);
            foreach (string item in ITEMS)
            {
                p.Add(bullet);
                p.Add(new Text(space + item.Replace(' ', space) + " ")).SetFont(helveticaFont);
            }

            doc.Add(p);
            doc.Add(new Paragraph("\n"));

            PdfFont freeSansFont =
                PdfFontFactory.CreateFont("../../../resources/font/FreeSans.ttf", PdfEncodings.IDENTITY_H);
            p = new Paragraph("Items can't be split if they don't fit at the end: ").SetFont(freeSansFont);
            foreach (string item in ITEMS)
            {
                // Paste unicode Bullet symbol
                p.Add(new Text('\u2022' + space + item.Replace(' ', space) + " "));
            }

            doc.Add(p);

            doc.Close();
        }
    }
}