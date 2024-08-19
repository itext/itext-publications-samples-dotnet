using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;

namespace iText.Highlevel.Notused.Appendix {
    /// <author>Bruno Lowagie (iText Software)</author>
    public class TextProperties {
        public const String DEST = "results/appendix/text_properties.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new TextProperties().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            // Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document document = new Document(pdf);
            Style style = new Style().SetDestination("Top");
            Text t1 = new Text("Test").AddStyle(style);
            document.Add(new Paragraph(t1));
            Text t2 = new Text("Test").SetBorder(new SolidBorder(0.5f));
            document.Add(new Paragraph(t2));
            Text t3 = new Text("Test").SetBorderLeft(new SolidBorder(0.5f)).SetBackgroundColor(ColorConstants.LIGHT_GRAY);
            document.Add(new Paragraph(t3));
            Text t4 = new Text("AWAY AGAIN").SetCharacterSpacing(10);
            document.Add(new Paragraph(t4));
            Text t5 = new Text("AWAY AGAIN").SetWordSpacing(10);
            document.Add(new Paragraph(t5));
            Text t6 = new Text("AWAY AGAIN").SetRelativePosition(-10, 50, 0, 0);
            document.Add(new Paragraph(t6));
            PdfAction top = PdfAction.CreateGoTo("Top");
            Text t7 = new Text("go to top").SetAction(top);
            document.Add(new Paragraph(t7));
            document.Close();
        }
    }
}
