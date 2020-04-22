using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Colorspace;
using iText.Layout;

namespace iText.Samples.Sandbox.Objects
{
    public class TextPattern
    {
        public static readonly string DEST = "results/sandbox/objects/text_pattern.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new TextPattern().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            PdfCanvas canvas = new PdfCanvas(pdfDoc.AddNewPage());
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            string fillText = "this is the fill text! ";
            float fillTextWidth = font.GetWidth(fillText, 6);

            PdfPattern.Tiling tilingPattern = new PdfPattern.Tiling(fillTextWidth, 60, fillTextWidth, 60);
            PdfPatternCanvas patternCanvas = new PdfPatternCanvas(tilingPattern, pdfDoc);
            patternCanvas.BeginText().SetFontAndSize(font, 6f);
            float x = 0;
            for (float y = 0; y < 60; y += 10)
            {
                patternCanvas.SetTextMatrix(x - fillTextWidth, y);
                patternCanvas.ShowText(fillText);
                patternCanvas.SetTextMatrix(x, y);
                patternCanvas.ShowText(fillText);
                x += (fillTextWidth / 6);
            }

            patternCanvas.EndText();

            canvas.Rectangle(10, 10, 575, 822);
            canvas.SetFillColor(new PatternColor(tilingPattern));
            canvas.Fill();

            doc.Close();
        }
    }
}