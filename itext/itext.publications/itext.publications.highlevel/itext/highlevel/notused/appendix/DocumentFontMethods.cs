using System;
using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Highlevel.Notused.Appendix {
    /// <author>Bruno Lowagie (iText Software)</author>
    public class DocumentFontMethods {
        public const String DEST = "results/appendix/document_font_methods.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new DocumentFontMethods().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            // Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document document = new Document(pdf);
            Paragraph p;
            p = new Paragraph("Testing font methods");
            document.Add(p);
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
            document.SetFont(font);
            p = new Paragraph("Testing font methods: changed font");
            document.Add(p);
            document.SetFontSize(18);
            p = new Paragraph("Testing font methods: changed font size");
            document.Add(p);
            document.SetFontColor(ColorConstants.BLUE);
            p = new Paragraph("Testing font methods: changed color");
            document.Add(p);
            // We don't suggest usage of SimulateBold() method to reach text thickness since the result is written with the usual
            // rather than the bold font: we only emulate "thickness". It's recommended to use an actual bold font instead.
            document.SimulateBold();
            p = new Paragraph("Testing font methods: simulated bold");
            document.Add(p);
            // We don't suggest usage of SimulateItalic() method to reach text obliquity since the result is written with the usual
            // rather than the italic font: we only emulate "obliquity". It's recommended to use an actual italic font instead.
            document.SimulateItalic();
            p = new Paragraph("Testing font methods: simulated italic");
            document.Add(p);
            document.SetProperty(Property.BOLD_SIMULATION, false);
            document.SetProperty(Property.ITALIC_SIMULATION, false);
            document.SetProperty(Property.FONT_COLOR, null);
            p = new Paragraph("Testing font methods: resetting style and color");
            document.Add(p);
            document.SetLineThrough();
            p = new Paragraph("Testing font methods: line through (default)");
            document.Add(p);
            document.SetProperty(Property.UNDERLINE, null);
            document.SetUnderline();
            p = new Paragraph("Testing font methods: underline (default)");
            document.Add(p);
            document.SetProperty(Property.UNDERLINE, null);
            document.SetUnderline(2, 4);
            document.SetUnderline(ColorConstants.BLUE, 5, 0.1f, 2, -0.5f, PdfCanvasConstants.LineCapStyle.ROUND);
            p = new Paragraph("Testing font methods: underline (custom)");
            document.Add(p);
            document.SetProperty(Property.UNDERLINE, null);
            document.SetTextRenderingMode(PdfCanvasConstants.TextRenderingMode.STROKE);
            p = new Paragraph("Testing font methods: change text rendering mode");
            document.Add(p);
            document.SetStrokeWidth(0.1f);
            document.SetStrokeColor(ColorConstants.BLUE);
            p = new Paragraph("Testing font methods: change stroke width and color");
            document.Add(p);
            //Close document
            document.Close();
        }
    }
}
