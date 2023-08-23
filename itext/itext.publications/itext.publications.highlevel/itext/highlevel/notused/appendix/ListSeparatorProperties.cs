using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Highlevel.Notused.Appendix {
    public class ListSeparatorProperties {
        public const String DEST = "results/appendix/separator_properties.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new ListSeparatorProperties().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            // Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document document = new Document(pdf);
            Style style = new Style();
            style.SetBackgroundColor(ColorConstants.YELLOW);
            document.Add(CreateNewSeparator().AddStyle(style).SetDestination("Top"));
            document.Add(new Paragraph("test"));
            document.Add(CreateNewSeparator().SetWidth(300).SetHorizontalAlignment(HorizontalAlignment.CENTER));
            document.Add(CreateNewSeparator().SetMargin(10).SetVerticalAlignment(VerticalAlignment.BOTTOM).SetBorder(new 
                SolidBorder(0.5f)));
            document.Add(CreateNewSeparator().SetMargin(10).SetWidth(300));
            document.Add(CreateNewSeparator().SetMargin(10).SetRelativePosition(10, 10, 50, 10));
            document.Add(CreateNewSeparator().SetMargin(10).SetWidth(UnitValue.CreatePercentValue(50)));
            document.Add(CreateNewSeparator().SetMargin(10).SetWidth(50).SetAction(PdfAction.CreateGoTo("Top")));
            document.Add(CreateNewSeparator().SetFixedPosition(100, 200, 350));
            document.Add(new AreaBreak());
            document.Add(CreateNewSeparator().SetBackgroundColor(ColorConstants.YELLOW).SetMarginBottom(10));
            document.Add(CreateNewSeparator().SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetPaddingLeft(20).SetPaddingRight(
                50));
            document.Add(CreateNewSeparator().SetBackgroundColor(ColorConstants.YELLOW));
            document.Add(CreateNewSeparator().SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetMarginBottom(50));
            document.Add(CreateNewSeparator().SetBackgroundColor(ColorConstants.YELLOW));
            document.Add(CreateNewSeparator().SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetMargin(50).SetPadding(30));
            document.Add(CreateNewSeparator().SetBackgroundColor(ColorConstants.YELLOW));
            document.Add(CreateNewSeparator().SetBackgroundColor(ColorConstants.LIGHT_GRAY));
            document.Close();
        }

        public static LineSeparator CreateNewSeparator() {
            return new LineSeparator(new DottedLine());
        }
    }
}
