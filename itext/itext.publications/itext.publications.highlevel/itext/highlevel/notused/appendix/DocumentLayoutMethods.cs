using System;
using System.IO;
using iText.IO.Font.Otf;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Hyphenation;
using iText.Layout.Properties;
using iText.Layout.Splitting;

namespace iText.Highlevel.Notused.Appendix {
    /// <author>Bruno Lowagie (iText Software)</author>
    public class DocumentLayoutMethods {
        public const String DEST = "results/appendix/document_layout_methods.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new DocumentLayoutMethods().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            // Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document document = new Document(pdf);
            Paragraph p;
            p = new Paragraph("Testing layout methods");
            document.Add(p);
            document.SetTextAlignment(TextAlignment.CENTER);
            p = new Paragraph("Testing layout methods");
            document.Add(p);
            p = new Paragraph();
            for (int i = 0; i < 6; i++) {
                p.Add("singing supercalifragilisticexpialidocious ");
            }
            document.Add(p);
            document.SetHyphenation(new HyphenationConfig("en", "uk", 3, 3));
            document.Add(p);
            document.SetTextAlignment(TextAlignment.JUSTIFIED);
            document.Add(p);
            document.SetHyphenation(null);
            document.SetSplitCharacters(new _ISplitCharacters_54());
            document.Add(p);
            document.SetSplitCharacters(new DefaultSplitCharacters());
            document.SetTextAlignment(TextAlignment.LEFT);
            document.Add(p);
            document.SetWordSpacing(10);
            document.Add(p);
            document.SetCharacterSpacing(5);
            document.Add(p);
            //Close document
            document.Close();
        }

        private sealed class _ISplitCharacters_54 : ISplitCharacters {
            public _ISplitCharacters_54() {
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
