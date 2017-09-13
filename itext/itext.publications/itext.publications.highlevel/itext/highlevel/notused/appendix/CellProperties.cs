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
    /// <author>iText</author>
    public class CellProperties {
        public const String DEST = "results/appendix/cell_properties.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new CellProperties().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            // Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document document = new Document(pdf);
            Table table = new Table(2);
            PdfFont font = PdfFontFactory.CreateFont(FontConstants.TIMES_ROMAN);
            table.AddCell(new Cell().Add(new Paragraph("Test 1")).SetHeight(50).SetDestination("Top"));
            Style style = new Style();
            style.SetBackgroundColor(Color.YELLOW);
            table.AddCell(new Cell().SetBorder(new DottedBorder(5)).Add(new Paragraph("Test 2")).AddStyle(style).SetRelativePosition(
                10, 10, 50, 10));
            table.AddCell(new Cell().Add(new Paragraph("Test 3")).SetVerticalAlignment(VerticalAlignment.BOTTOM));
            table.AddCell(new Cell().Add(ParagraphProperties.GetNewParagraphInstance()).SetHyphenation(new HyphenationConfig
                ("en", "uk", 3, 3)));
            table.AddCell(new Cell().Add(new Paragraph("Rotated")).SetRotationAngle(Math.PI / 18).SetFont(font).SetFontSize(8).SetFontColor
                (Color.RED));
            table.AddCell(new Cell().Add(new Paragraph("Centered")).SetTextAlignment(TextAlignment.CENTER).SetAction(PdfAction.CreateGoTo
                ("Top")));
            table.AddCell(new Cell().Add(new Paragraph("Test 5")).SetBackgroundColor(Color.BLUE));
            table.AddCell(new Cell().Add(ParagraphProperties.GetNewParagraphInstance()).SetBackgroundColor(Color.RED).
                SetPaddingLeft(20).SetPaddingRight(50));
            table.AddCell(new Cell().Add(new Paragraph("Test 7")).SetBackgroundColor(Color.RED));
            table.AddCell(new Cell().Add(new Paragraph("Test 8")).SetBackgroundColor(Color.BLUE).SetMarginBottom(10));
            table.AddCell(new Cell().Add(new Paragraph("Test 9")).SetBackgroundColor(Color.BLUE));
            table.AddCell(new Cell().Add(new Paragraph("Test 10")).SetBackgroundColor(Color.RED));
            table.AddCell(new Cell().Add(ParagraphProperties.GetNewParagraphInstance()).SetBackgroundColor(Color.RED).
                SetMargin(50).SetPadding(30));
            table.AddCell(new Cell().Add(new Paragraph("Test 12")).SetBackgroundColor(Color.BLUE));
            document.Add(table);
            SolidBorder border = new SolidBorder(Color.RED, 2);
            Cell cell = new Cell().Add(new Paragraph("Test")).SetFixedPosition(100, 400, 350).SetBorder(border).SetBackgroundColor(Color
                .BLUE).SetHeight(100).SetHorizontalAlignment(HorizontalAlignment.CENTER);
            document.Add(cell);
            document.Close();
        }
    }
}
