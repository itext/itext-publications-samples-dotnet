/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/
using System;
using System.IO;
using iText.IO.Font;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Hyphenation;
using iText.Layout.Properties;

namespace iText.Highlevel.Notused.Appendix {
    /// <author>Bruno Lowagie (iText Software)</author>
    public class ParagraphProperties {
        public const String DEST = "results/appendix/paragraph_properties.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new ParagraphProperties().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            // Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document document = new Document(pdf);
            Paragraph p;
            PdfFont font = PdfFontFactory.CreateFont(FontConstants.TIMES_ROMAN);
            Style style = new Style();
            style.SetBackgroundColor(ColorConstants.YELLOW);
            p = GetNewParagraphInstance().AddStyle(style).SetBorder(new SolidBorder(0.5f)).SetDestination("Top");
            document.Add(p);
            p = GetNewParagraphInstance();
            p.SetBackgroundColor(ColorConstants.GRAY).SetWidth(150).SetHorizontalAlignment(HorizontalAlignment.CENTER).SetTextAlignment
                (TextAlignment.CENTER);
            document.Add(p);
            document.Add(GetNewParagraphInstance().SetRotationAngle(Math.PI / 18));
            document.Add(GetNewParagraphInstance().SetWidth(150).SetHyphenation(new HyphenationConfig("en", "uk", 3, 3
                )));
            document.Add(GetNewParagraphInstance().SetHeight(120).SetVerticalAlignment(VerticalAlignment.BOTTOM).SetBackgroundColor
                (ColorConstants.YELLOW).SetRelativePosition(10, 10, 50, 10));
            document.Add(GetNewParagraphInstance().SetWidth(UnitValue.CreatePercentValue(80)).SetFont(font).SetFontSize(8).SetFontColor(ColorConstants
                .RED));
            document.Add(new AreaBreak());
            document.Add(GetNewParagraphInstance().SetFixedPosition(100, 400, 350).SetAction(PdfAction.CreateGoTo("Top"
                )));
            document.Add(new AreaBreak());
            document.Add(GetNewParagraphInstance().SetBackgroundColor(ColorConstants.YELLOW).SetMarginBottom(10));
            document.Add(GetNewParagraphInstance().SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetPaddingLeft(20).SetPaddingRight
                (50));
            document.Add(GetNewParagraphInstance().SetBackgroundColor(ColorConstants.YELLOW));
            document.Add(GetNewParagraphInstance().SetBackgroundColor(ColorConstants.LIGHT_GRAY));
            document.Add(GetNewParagraphInstance().SetBackgroundColor(ColorConstants.YELLOW));
            document.Add(GetNewParagraphInstance().SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetMargin(50).SetPadding(30));
            document.Add(GetNewParagraphInstance().SetBackgroundColor(ColorConstants.YELLOW));
            document.Add(GetNewParagraphInstance().SetBackgroundColor(ColorConstants.LIGHT_GRAY));
            document.Close();
        }

        public static Paragraph GetNewParagraphInstance() {
            return new Paragraph("This is a long paragraph that " + "will be used and reused to test paragraph " + "properties. This paragraph should take "
                 + "more than one line. We'll change different " + "properties and then look at the effect " + "when we add the paragraph to the document."
                );
        }
    }
}
