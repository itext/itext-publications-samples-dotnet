using System;
using System.IO;
using iText.IO.Font.Constants;
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
    /// <author>iText</author>
    public class DivProperties {
        public const String DEST = "results/appendix/div_properties.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new DivProperties().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            // Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document document = new Document(pdf);
            Style style = new Style();
            style.SetBackgroundColor(ColorConstants.YELLOW).SetBorder(new SolidBorder(0.5f));
            document.Add(CreateNewDiv().AddStyle(style).SetWidth(350).SetHorizontalAlignment(HorizontalAlignment.CENTER
                ).SetTextAlignment(TextAlignment.CENTER).SetDestination("Top"));
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
            document.Add(CreateNewDiv().SetRotationAngle(Math.PI / 18).SetFont(font).SetFontSize(8).SetFontColor(ColorConstants
                .RED));
            document.Add(CreateNewDiv().SetWidth(350).SetHyphenation(new HyphenationConfig("en", "uk", 3, 3)).SetKeepWithNext
                (true));
            document.Add(CreateNewDiv().SetWidth(UnitValue.CreatePercentValue(70)).SetKeepTogether(true));
            document.Add(CreateNewDiv().SetHeight(350).SetBackgroundColor(ColorConstants.YELLOW).SetAction(PdfAction.CreateGoTo
                ("Top")).SetRelativePosition(10, 10, 50, 10));
            document.Add(new AreaBreak());
            document.Add(CreateNewDiv().SetFixedPosition(100, 400, 350));
            document.Add(new AreaBreak());
            document.Add(CreateNewDiv().SetBackgroundColor(ColorConstants.YELLOW).SetMarginBottom(10));
            document.Add(CreateNewDiv().SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetPaddingLeft(20).SetPaddingRight(50));
            document.Add(CreateNewDiv().SetBackgroundColor(ColorConstants.YELLOW));
            document.Add(CreateNewDiv().SetBackgroundColor(ColorConstants.LIGHT_GRAY));
            document.Add(CreateNewDiv().SetBackgroundColor(ColorConstants.YELLOW));
            document.Add(CreateNewDiv().SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetMargin(50).SetPadding(30));
            document.Add(CreateNewDiv().SetBackgroundColor(ColorConstants.YELLOW));
            document.Add(CreateNewDiv().SetBackgroundColor(ColorConstants.LIGHT_GRAY));
            document.Close();
        }

        public static Div CreateNewDiv() {
            Div div = new Div();
            div.Add(ParagraphProperties.GetNewParagraphInstance());
            div.Add(ListSeparatorProperties.CreateNewSeparator());
            div.Add(ListProperties.CreateNewList());
            return div;
        }
    }
}
