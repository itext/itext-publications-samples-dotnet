using System;
using System.IO;
using iText.IO.Font.Otf;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout.Element;
using iText.Layout.Hyphenation;
using iText.Layout.Properties;
using iText.Layout.Splitting;

namespace iText.Highlevel.Notused.Appendix {
    public class CanvasLayoutMethods {
        public const String DEST = "results/appendix/canvas_layout_methods.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new CanvasLayoutMethods().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            // Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            PdfPage page = pdf.AddNewPage();
            PdfCanvas pdfCanvas = new PdfCanvas(page);
            Rectangle rectangle = new Rectangle(36, 36, 523, 770);
            iText.Layout.Canvas canvas = new iText.Layout.Canvas(pdfCanvas, rectangle);
            Paragraph p;
            p = new Paragraph("Testing layout methods");
            canvas.Add(p);
            canvas.SetTextAlignment(TextAlignment.CENTER);
            p = new Paragraph("Testing layout methods");
            canvas.Add(p);
            p = new Paragraph();
            for (int i = 0; i < 6; i++) {
                p.Add("singing supercalifragilisticexpialidocious ");
            }
            canvas.Add(p);
            canvas.SetHyphenation(new HyphenationConfig("en", "uk", 3, 3));
            canvas.Add(p);
            canvas.SetTextAlignment(TextAlignment.JUSTIFIED);
            canvas.Add(p);
            canvas.SetHyphenation(null);
            canvas.SetSplitCharacters(new _ISplitCharacters_62());
            canvas.Add(p);
            canvas.SetSplitCharacters(new DefaultSplitCharacters());
            canvas.SetTextAlignment(TextAlignment.LEFT);
            canvas.Add(p);
            canvas.SetWordSpacing(10);
            canvas.Add(p);
            canvas.SetCharacterSpacing(5);
            canvas.Add(p);
            //Close document
            pdf.Close();
        }

        private sealed class _ISplitCharacters_62 : ISplitCharacters {
            public _ISplitCharacters_62() {
            }

            public bool IsSplitCharacter(GlyphLine text, int glyphPos) {
                if (!text.Get(glyphPos).HasValidUnicode()) {
                    return false;
                }
                int charCode = text.Get(glyphPos).GetUnicode();
                return (charCode < ' ' || charCode == 'i');
            }
        }
    }
}
