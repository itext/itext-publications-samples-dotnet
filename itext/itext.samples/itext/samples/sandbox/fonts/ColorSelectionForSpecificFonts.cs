using System;
using System.Collections.Generic;
using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Font;
using iText.Layout.Layout;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Fonts
{
    public class ColorSelectionForSpecificFonts
    {
        public static readonly String DEST = "results/sandbox/fonts/color_selection_for_specific_fonts.pdf";
        public static readonly String FONTS_FOLDER = "../../../resources/font/";

        public static readonly  String TEXT = "Some arabic text in yellow - \u0644\u0640\u0647 \n"
            + "Some devanagari text in green - \u0915\u0941\u091B \n"
            + "Some latin text in blue - iText \n";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ColorSelectionForSpecificFonts().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest) {
            String timesRomanFont = StandardFonts.TIMES_ROMAN;
            String arabicFont = FONTS_FOLDER + "NotoNaskhArabic-Regular.ttf";
            String devanagariFont = FONTS_FOLDER + "NotoSansDevanagari-Regular.ttf";

            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(DEST));
            Document doc = new Document(pdfDoc);

            FontProvider provider = new FontProvider();
            provider.AddFont(timesRomanFont);
            provider.AddFont(arabicFont);
            provider.AddFont(devanagariFont);

            // Adding fonts to the font provider
            doc.SetFontProvider(provider);

            Paragraph paragraph = new Paragraph(TEXT);

            // Setting font family to the particular element will trigger iText's font selection mechanism:
            // font for the element will be picked up from the provider's fonts
            paragraph.SetFontFamily(timesRomanFont);

            IDictionary<String, Color> fontColorMap = new Dictionary<String, Color>();
            fontColorMap.Add("NotoNaskhArabic", ColorConstants.YELLOW);
            fontColorMap.Add("NotoSansDevanagari", ColorConstants.GREEN);
            fontColorMap.Add("Times-Roman", ColorConstants.BLUE);

            // Set custom renderer, which will change the color of text written within specific font
            paragraph.SetNextRenderer(new CustomParagraphRenderer(paragraph, fontColorMap));

            doc.Add(paragraph);

            doc.Close();
        }

        public class CustomParagraphRenderer : ParagraphRenderer {
            private IDictionary<String, Color> fontColorMap;

            public CustomParagraphRenderer(Paragraph modelElement, IDictionary<String, Color> fontColorMap)
                    : base(modelElement) {
                this.fontColorMap = new Dictionary<String, Color>(fontColorMap);
            }

            public override LayoutResult Layout(LayoutContext layoutContext) {
                LayoutResult result = base.Layout(layoutContext);
                // During layout() execution iText parses the Paragraph and splits it into a number of Text objects,
                // each of which have glyphs to be processed by the same font.
                // Lines, which are the result of layout(), will be drawn to the pdf on draw().
                // In order to change the color of text written in specific font, we could just go through all the lines
                // and update the color of Text objects, which have this font being set.
                IList<LineRenderer> lines = GetLines();
                UpdateColor(lines);
                return result;
            }

            private void UpdateColor(IList<LineRenderer> lines) {
                foreach (LineRenderer renderer in lines) {
                    IList<IRenderer> children = renderer.GetChildRenderers();
                    foreach (IRenderer child in children) {
                        if (child is TextRenderer) {
                            PdfFont pdfFont = ((TextRenderer)child).GetPropertyAsFont(Property.FONT);
                            if (null != pdfFont) {
                                Color updatedColor = fontColorMap[pdfFont.GetFontProgram().GetFontNames().GetFontName()];
                                if (null != updatedColor) {

                                    // Although setting a property via setProperty might be useful,
                                    // it's regarded as internal iText functionality. The properties are expected
                                    // to be set on elements via specific setters (setFont, for example).
                                    // iText doesn't guarantee that setProperty will work as you expect it,
                                    // so please be careful while calling this method.
                                    child.SetProperty(Property.FONT_COLOR, new TransparentColor(updatedColor));
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}