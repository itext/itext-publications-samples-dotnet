using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;

namespace Tutorial.Chapter02 {
    /// <summary>Simple changing text state example.</summary>
    public class C02E04_StarWarsCrawl {
        public const String DEST = "../../../results/chapter02/star_wars_crawl.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C02E04_StarWarsCrawl().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            IList<String> text = new List<String>();
            text.Add("            Episode V      ");
            text.Add("    THE EMPIRE STRIKES BACK  ");
            text.Add("It is a dark time for the");
            text.Add("Rebellion. Although the Death");
            text.Add("Star has been destroyed,");
            text.Add("Imperial troops have driven the");
            text.Add("Rebel forces from their hidden");
            text.Add("base and pursued them across");
            text.Add("the galaxy.");
            text.Add("Evading the dreaded Imperial");
            text.Add("Starfleet, a group of freedom");
            text.Add("fighters led by Luke Skywalker");
            text.Add("has established a new secret");
            text.Add("base on the remote ice world");
            text.Add("of Hoth...");
            int maxStringWidth = 0;
            foreach (String fragment in text) {
                if (fragment.Length > maxStringWidth) {
                    maxStringWidth = fragment.Length;
                }
            }
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            //Add new page
            PageSize ps = PageSize.A4;
            PdfPage page = pdf.AddNewPage(ps);
            PdfCanvas canvas = new PdfCanvas(page);
            //Set black background
            canvas.Rectangle(0, 0, ps.GetWidth(), ps.GetHeight()).SetColor(ColorConstants.BLACK, true).Fill();
            //Replace the origin of the coordinate system to the top left corner
            canvas.ConcatMatrix(1, 0, 0, 1, 0, ps.GetHeight());
            Color yellowColor = new DeviceCmyk(0f, 0.0537f, 0.769f, 0.051f);
            float lineHeight = 5;
            float yOffset = -40;
            canvas.BeginText().SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.COURIER_BOLD), 1).SetColor(yellowColor
                , true);
            for (int j = 0; j < text.Count; j++) {
                String line = text[j];
                float xOffset = ps.GetWidth() / 2 - 45 - 8 * j;
                float fontSizeCoeff = 6 + j;
                float lineSpacing = (lineHeight + j) * j / 1.5f;
                int stringWidth = line.Length;
                for (int i = 0; i < stringWidth; i++) {
                    float angle = (maxStringWidth / 2 - i) / 2f;
                    float charXOffset = (4 + (float)j / 2) * i;
                    canvas.SetTextMatrix(fontSizeCoeff, 0, angle, fontSizeCoeff / 1.5f, xOffset + charXOffset, yOffset - lineSpacing
                        ).ShowText(line[i].ToString());
                }
            }
            canvas.EndText();
            //Close document
            pdf.Close();
        }
    }
}
