using System;
using System.IO;
using iText.IO.Font;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Font;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Layout {

    // VerticalTextWithCombineUpright.cs
    //
    // Example showing vertical text combined with CombineUpright (tate-chu-yoko).
    // CombineUpright rotates short runs of Latin/numeric characters to display
    // upright within a vertical text flow, which is standard in CJK typography
    // for inline numbers, abbreviations and short Latin words.

    public class VerticalTextWithCombineUpright
    {
        public static readonly string DEST = "results/sandbox/layout/verticalTextWithCombineUpright.pdf";
        public static readonly string FONT = "../../../resources/font/NotoSansCJKjp-Regular.otf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new VerticalTextWithCombineUpright().ManipulatePdf(DEST);
        }

        public void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            PdfFont font = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H);
            FontProvider fontProvider = new FontProvider();
            fontProvider.AddFont(FONT, PdfEncodings.IDENTITY_H);
            doc.SetFontProvider(fontProvider);
            doc.SetFont(font);

            doc.Add(new Paragraph("Vertical Text with Combine Upright (縦中横)")
                    .SetFontSize(20)
                    .SetFontColor(new DeviceRgb(60, 60, 150))
                    .SetMarginBottom(20));

            doc.Add(new Paragraph(
                    "CombineUpright (tate-chu-yoko / 縦中横) keeps short Latin or numeric runs "
                    + "upright within a vertical text flow. "
                    + "Compare the two columns below: first page uses plain vertical text, "
                    + "second page wraps numbers and abbreviations in CombineUpright.")
                    .SetMarginBottom(20));

            // Left column: plain vertical text — numbers and Latin rotate sideways.
            VerticalParagraph plainVertical = new VerticalParagraph(true);
            plainVertical.Add("発売日：");
            plainVertical.Add("2024");
            plainVertical.Add("年");
            plainVertical.Add("10");
            plainVertical.Add("月");
            plainVertical.Add("PDF\n");
            plainVertical.Add("ページ数：");
            plainVertical.Add("128");
            plainVertical.Add("ページ");
            plainVertical.SetFontSize(14);
            plainVertical.SetBackgroundColor(new DeviceRgb(255, 240, 240));
            plainVertical.SetFontColor(new DeviceRgb(120, 40, 40));
            plainVertical.SetWidth(40);
            plainVertical.SetHeight(400);

            doc.Add(new Div()
                    .Add(new Paragraph("Without CombineUpright")
                            .SetFontSize(10).SetFontColor(ColorConstants.GRAY))
                    .Add(plainVertical)
                    .SetBorder(new SolidBorder(ColorConstants.LIGHT_GRAY, 1))
                    .SetPadding(10));

            // Page 2: same content with TEXT_COMBINE_UPRIGHT applied — numbers and
            // Latin abbreviations now display upright within the vertical flow.
            doc.Add(new AreaBreak());

            doc.Add(new Paragraph("With CombineUpright (縦中横)")
                    .SetFontSize(20)
                    .SetFontColor(new DeviceRgb(60, 60, 150))
                    .SetMarginBottom(20));

            // Right column: same content but short runs have TEXT_COMBINE_UPRIGHT applied —
            // numbers and Latin abbreviations display upright within the vertical flow.
            VerticalParagraph uprightVertical = new VerticalParagraph(true);
            uprightVertical.Add("発売日：");
            Text year = new Text("2024");
            year.SetProperty(Property.TEXT_COMBINE_UPRIGHT, TextCombineUpright.ALL);
            uprightVertical.Add(year);
            uprightVertical.Add("年");
            Text month = new Text("10");
            month.SetProperty(Property.TEXT_COMBINE_UPRIGHT, TextCombineUpright.ALL);
            uprightVertical.Add(month);
            uprightVertical.Add("月");
            Text version = new Text("PDF");
            version.SetProperty(Property.TEXT_COMBINE_UPRIGHT, TextCombineUpright.ALL);
            uprightVertical.Add(version);
            uprightVertical.Add("\nページ数：");
            Text pages = new Text("128");
            pages.SetProperty(Property.TEXT_COMBINE_UPRIGHT, TextCombineUpright.ALL);
            uprightVertical.Add(pages);
            uprightVertical.Add("ページ");
            uprightVertical.SetFontSize(14);
            uprightVertical.SetBackgroundColor(new DeviceRgb(235, 245, 255));
            uprightVertical.SetFontColor(new DeviceRgb(40, 60, 120));
            uprightVertical.SetWidth(40);
            uprightVertical.SetHeight(400);

            doc.Add(new Div()
                    .Add(new Paragraph("With CombineUpright")
                            .SetFontSize(10).SetFontColor(ColorConstants.GRAY))
                    .Add(uprightVertical)
                    .SetBorder(new SolidBorder(ColorConstants.LIGHT_GRAY, 1))
                    .SetPadding(10));

            doc.Close();
        }
    }
}