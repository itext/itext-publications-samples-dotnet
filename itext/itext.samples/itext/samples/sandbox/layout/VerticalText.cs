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
using iText.Layout.Renderer;

namespace iText.Samples.Sandbox.Layout {

    // VerticalText.cs
    //
    // Example showing how to render text vertically using VerticalParagraph.
    // Demonstrates both VERTICAL_LR (left-to-right columns) and VERTICAL_RL
    // (right-to-left columns), mixing English and CJK characters.

    public class VerticalText
    {
        public static readonly string DEST = "results/sandbox/layout/verticalText.pdf";
        public static readonly string FONT = "../../../resources/font/NotoSansCJKjp-Regular.otf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new VerticalText().ManipulatePdf(DEST);
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

            doc.Add(new Paragraph("Vertical Text Demo")
                    .SetFontSize(20)
                    .SetFontColor(new DeviceRgb(60, 60, 150))
                    .SetMarginBottom(20));

            doc.Add(new Paragraph(
                    "VerticalParagraph supports two column directions: "
                    + "VERTICAL_LR (false) adds new columns left-to-right, "
                    + "VERTICAL_RL (true) adds new columns right-to-left — standard for CJK.")
                    .SetMarginBottom(20));

            // VERTICAL_LR (false): columns flow left-to-right.
            VerticalParagraph ltr = new VerticalParagraph(false);
            ltr.Add("VERTICAL_LR\n");
            ltr.Add("左から右へ\n");
            ltr.Add("Columns: left to right.\n");
            ltr.Add("日本語・中文・한국어\n");
            ltr.SetFontSize(14);
            ltr.SetBackgroundColor(new DeviceRgb(235, 245, 255));
            ltr.SetFontColor(new DeviceRgb(40, 60, 120));
            ltr.SetHeight(400);

            // VERTICAL_RL (true): columns flow right-to-left — standard for CJK vertical typography.
            VerticalParagraph rtl = new VerticalParagraph(true);
            rtl.Add("VERTICAL_RL\n");
            rtl.Add("右から左へ\n");
            rtl.Add("Columns: right to left.\n");
            rtl.Add("日本語・中文・한국어\n");
            rtl.SetFontSize(14);
            rtl.SetBackgroundColor(new DeviceRgb(235, 250, 238));
            rtl.SetFontColor(new DeviceRgb(20, 100, 60));
            rtl.SetHeight(400);

            Div columns = new Div()
                    .Add(new Div()
                            .Add(new Paragraph("VERTICAL_LR (false)")
                                    .SetFontSize(10).SetFontColor(ColorConstants.GRAY))
                            .Add(ltr)
                            .SetMarginRight(30))
                    .Add(new Div()
                            .Add(new Paragraph("VERTICAL_RL (true)")
                                    .SetFontSize(10).SetFontColor(ColorConstants.GRAY))
                            .Add(rtl))
                    .SetBorder(new SolidBorder(ColorConstants.LIGHT_GRAY, 1))
                    .SetPadding(10);
            columns.SetNextRenderer(new FlexContainerRenderer(columns));

            doc.Add(columns);

            doc.Close();
        }
    }
}