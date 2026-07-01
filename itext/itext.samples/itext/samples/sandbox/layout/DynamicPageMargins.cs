using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Properties.Margins;

namespace iText.Samples.Sandbox.Layout {

    // DynamicPageMargins.cs
    //
    // Example showing how to set page margins with dynamic content.
    // Demonstrates applying different margin box content to specific pages,
    // to pages selected by a predicate, and via a SectionBreak with margin
    // boxes on all four sides.

    public class DynamicPageMargins
    {
        public static readonly string DEST = "results/sandbox/layout/dynamicPageMargins.pdf";

        private static readonly DeviceRgb PAGE2_COLOR = new DeviceRgb(255, 196, 140);
        private static readonly DeviceRgb EVEN_COLOR = new DeviceRgb(140, 196, 255);

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new DynamicPageMargins().ManipulatePdf(DEST);
        }

        public void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            doc.Add(BodyText("Page 1", "No margin box has been set yet."));

            // Margin boxes for a single, specific page number.
            // This will only affect page 2, regardless of how many pages the document ends up having.
            doc.SetPageMargins(2, CreateTopMarginBoxes("PAGE 2 ONLY", PAGE2_COLOR));

            doc.Add(new AreaBreak());
            doc.Add(BodyText("Page 2", "A margin box was set for this specific page only."));

            doc.Add(new AreaBreak());
            doc.Add(BodyText("Page 3", "The page-2-only margin box no longer applies here."));

            // Margin boxes driven by a predicate.
            // This will affect every even page added after this call, for example pages 4, 6, 8...
            doc.SetPageMargins((pageNum) => pageNum % 2 == 0, CreateTopMarginBoxes("EVEN PAGE", EVEN_COLOR));

            doc.Add(new AreaBreak());
            doc.Add(BodyText("Page 4", "An even page, so the predicate-based margin box applies."));

            doc.Add(new AreaBreak());
            doc.Add(BodyText("Page 5", "An odd page, so the predicate-based margin box does not apply."));

            // A SectionBreak can also carry its own margin boxes.
            // Here all four sides (top, bottom, left, right) get content,
            // and these margin boxes apply from this point on, replacing the predicate-based ones.
            doc.Add(new SectionBreak(CreateAllSidesMarginBoxes()));
            doc.Add(BodyText("Page 6", "A SectionBreak introduced margin boxes on all four sides."));

            doc.Add(new AreaBreak());
            doc.Add(BodyText("Page 7", "The all-four-sides margin boxes from the SectionBreak persist."));

            doc.Close();
        }

        // Builds a single, centered, color-coded top margin box with a bold label.
        private PageMarginBoxes CreateTopMarginBoxes(String label, DeviceRgb color)
        {
            Div header = MarginBoxContent(label, color);

            IList<PageMarginContent> elements = new List<PageMarginContent>();
            elements.Add(new PageMarginContent(MarginBoxName.TOP, header));
            return new PageMarginBoxes(elements);
        }

        // Builds a basic set of margin boxes for all four sides: top, bottom, left and right.
        // Each side has a distinct shape and style so they're easy to tell apart visually.
        private PageMarginBoxes CreateAllSidesMarginBoxes()
        {
            IList<PageMarginContent> elements = new List<PageMarginContent>();
            elements.Add(new PageMarginContent(MarginBoxName.TOP, TopMarginContent()));
            elements.Add(new PageMarginContent(MarginBoxName.BOTTOM, BottomMarginContent()));
            elements.Add(new PageMarginContent(MarginBoxName.LEFT, LeftMarginContent()));
            elements.Add(new PageMarginContent(MarginBoxName.RIGHT, RightMarginContent()));
            return new PageMarginBoxes(elements);
        }

        // Top margin: a tall, bold banner with a thick bottom border, like a page header.
        private Div TopMarginContent()
        {
            return new Div()
                    .Add(new Paragraph("TOP MARGIN")
                            .SetFontColor(ColorConstants.WHITE)
                            .SetFontSize(16)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetMargin(0))
                    .SetBackgroundColor(new DeviceRgb(90, 60, 160))
                    .SetHeight(50)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                    .SetBorderBottom(new SolidBorder(new DeviceRgb(60, 30, 120), 4));
        }

        // Bottom margin: a short, light strip with small, letter-spaced uppercase text,
        // like a footer caption rather than a heading.
        private Div BottomMarginContent()
        {
            return new Div()
                    .Add(new Paragraph("Bottom Margin \u2022 Page Footer")
                            .SetFontColor(new DeviceRgb(255, 140, 200))
                            .SetFontSize(9)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetMargin(0))
                    .SetBackgroundColor(new DeviceRgb(255, 235, 245))
                    .SetHeight(20)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                    .SetBorderTop(new SolidBorder(new DeviceRgb(255, 140, 200), 1));
        }

        // Left margin: a narrow vertical sidebar, text aligned to the left rather than centered,
        // with a colored left edge bar.
        private Div LeftMarginContent()
        {
            return new Div()
                    .Add(new Paragraph("LEFT")
                            .SetFontColor(new DeviceRgb(20, 130, 100))
                            .SetFontSize(11)
                            .SetTextAlignment(TextAlignment.LEFT)
                            .SetMargin(0))
                    .SetBackgroundColor(new DeviceRgb(225, 250, 240))
                    .SetPaddingLeft(6)
                    .SetBorderLeft(new SolidBorder(new DeviceRgb(140, 255, 200), 5));
        }

        // Right margin: a narrow vertical sidebar, mirrored from the left, text aligned right,
        // with a colored right edge bar.
        private Div RightMarginContent()
        {
            return new Div()
                    .Add(new Paragraph("RIGHT")
                            .SetFontColor(new DeviceRgb(180, 110, 0))
                            .SetFontSize(11)
                            .SetTextAlignment(TextAlignment.RIGHT)
                            .SetMargin(0))
                    .SetBackgroundColor(new DeviceRgb(255, 245, 225))
                    .SetPaddingRight(6)
                    .SetBorderRight(new SolidBorder(new DeviceRgb(255, 220, 140), 5));
        }

        // Builds a single, centered, color-coded labelled Div used as top margin box content
        // for the page-specific and predicate-based examples.
        private Div MarginBoxContent(String label, DeviceRgb color)
        {
            return new Div()
                    .Add(new Paragraph(label)
                            .SetFontColor(ColorConstants.WHITE)
                            .SetFontSize(14)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetMargin(0))
                    .SetBackgroundColor(color)
                    .SetHeight(40)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE);
        }

        // Builds the main page content: a page-number badge plus an explanatory line.
        private Div BodyText(String pageLabel, String description)
        {
            Paragraph badge = new Paragraph(pageLabel)
                    .SetFontSize(22)
                    .SetFontColor(ColorConstants.DARK_GRAY)
                    .SetMargin(0);

            Paragraph text = new Paragraph(description)
                    .SetFontSize(12)
                    .SetFontColor(ColorConstants.GRAY)
                    .SetMarginTop(8);

            return new Div()
                    .Add(badge)
                    .Add(text)
                    .SetWidth(UnitValue.CreatePercentValue(80))
                    .SetHorizontalAlignment(HorizontalAlignment.CENTER)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetPadding(20)
                    .SetBorder(new SolidBorder(ColorConstants.LIGHT_GRAY, 1))
                    .SetMarginTop(60);
        }
    }
}