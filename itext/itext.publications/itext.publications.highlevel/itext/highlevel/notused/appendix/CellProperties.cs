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
    public class CellProperties {
        public const String DEST = "results/appendix/cell_properties.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new CellProperties().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            // Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document document = new Document(pdf);
            Table table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
            table.AddCell(new Cell().Add(new Paragraph("Test 1")).SetHeight(50).SetDestination("Top"));
            Style style = new Style();
            style.SetBackgroundColor(ColorConstants.YELLOW);
            table.AddCell(new Cell().SetBorder(new DottedBorder(5)).Add(new Paragraph("Test 2")).AddStyle(style).SetRelativePosition(
                10, 10, 50, 10));
            table.AddCell(new Cell().Add(new Paragraph("Test 3")).SetVerticalAlignment(VerticalAlignment.BOTTOM));
            table.AddCell(new Cell().Add(ParagraphProperties.GetNewParagraphInstance()).SetHyphenation(new HyphenationConfig
                ("en", "uk", 3, 3)));
            table.AddCell(new Cell().Add(new Paragraph("Rotated")).SetRotationAngle(Math.PI / 18).SetFont(font).SetFontSize(8).SetFontColor
                (ColorConstants.RED));
            table.AddCell(new Cell().Add(new Paragraph("Centered")).SetTextAlignment(TextAlignment.CENTER).SetAction(PdfAction.CreateGoTo
                ("Top")));
            table.AddCell(new Cell().Add(new Paragraph("Test 5")).SetBackgroundColor(ColorConstants.BLUE));
            table.AddCell(new Cell().Add(ParagraphProperties.GetNewParagraphInstance()).SetBackgroundColor(ColorConstants.RED).
                SetPaddingLeft(20).SetPaddingRight(50));
            table.AddCell(new Cell().Add(new Paragraph("Test 7")).SetBackgroundColor(ColorConstants.RED));
            table.AddCell(new Cell().Add(new Paragraph("Test 8")).SetBackgroundColor(ColorConstants.BLUE).SetMarginBottom(10));
            table.AddCell(new Cell().Add(new Paragraph("Test 9")).SetBackgroundColor(ColorConstants.BLUE));
            table.AddCell(new Cell().Add(new Paragraph("Test 10")).SetBackgroundColor(ColorConstants.RED));
            table.AddCell(new Cell().Add(ParagraphProperties.GetNewParagraphInstance()).SetBackgroundColor(ColorConstants.RED).
                SetMargin(50).SetPadding(30));
            table.AddCell(new Cell().Add(new Paragraph("Test 12")).SetBackgroundColor(ColorConstants.BLUE));
            document.Add(table);
            SolidBorder border = new SolidBorder(ColorConstants.RED, 2);
            Cell cell = new Cell().Add(new Paragraph("Test")).SetFixedPosition(100, 400, 350).SetBorder(border).SetBackgroundColor(ColorConstants
                .BLUE).SetHeight(100).SetHorizontalAlignment(HorizontalAlignment.CENTER);
            document.Add(cell);
            document.Close();
        }
    }
}
