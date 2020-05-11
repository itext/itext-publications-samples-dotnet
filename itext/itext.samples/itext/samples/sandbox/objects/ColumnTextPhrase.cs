using System.IO;
using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Objects
{
    public class ColumnTextPhrase
    {
        public static readonly string DEST = "results/sandbox/objects/column_text_phrase.pdf";
        public static readonly string SRC = "../../../resources/pdfs/hello.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new ColumnTextPhrase().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));

            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD, PdfEncodings.CP1252, true);
            Paragraph pz = new Paragraph("Hello World!").SetFont(font).SetFontSize(13);
            new Canvas(new PdfCanvas(pdfDoc.GetFirstPage()), new Rectangle(120, 48, 80, 580))
                .Add(pz);

            // The Leading is used in this paragraph, the Leading is a spacing between lines of text
            font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            pz = new Paragraph("Hello World!").SetFont(font).SetFixedLeading(20);
            new Canvas(new PdfCanvas(pdfDoc.GetFirstPage()), new Rectangle(120, 48, 80, 480))
                .Add(pz);

            pdfDoc.Close();
        }
    }
}