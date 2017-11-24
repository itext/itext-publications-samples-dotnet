/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/
using System;
using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Highlevel.Notused.Appendix {
    /// <author>Bruno Lowagie (iText Software)</author>
    public class CanvasFontMethods {
        public const String DEST = "results/appendix/canvas_font_methods.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new CanvasFontMethods().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            // Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            PdfPage page = pdf.AddNewPage();
            PdfCanvas pdfCanvas = new PdfCanvas(page);
            Rectangle rectangle = new Rectangle(36, 36, 523, 770);
            iText.Layout.Canvas canvas = new iText.Layout.Canvas(pdfCanvas, pdf, rectangle);
            Paragraph p;
            p = new Paragraph("Testing font methods");
            canvas.Add(p);
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
            canvas.SetFont(font);
            p = new Paragraph("Testing font methods: changed font");
            canvas.Add(p);
            canvas.SetFontSize(18);
            p = new Paragraph("Testing font methods: changed font size");
            canvas.Add(p);
            canvas.SetFontColor(ColorConstants.BLUE);
            p = new Paragraph("Testing font methods: changed color");
            canvas.Add(p);
            canvas.SetBold();
            p = new Paragraph("Testing font methods: to bold");
            canvas.Add(p);
            canvas.SetItalic();
            p = new Paragraph("Testing font methods: to italic");
            canvas.Add(p);
            canvas.SetProperty(Property.BOLD_SIMULATION, false);
            canvas.SetProperty(Property.ITALIC_SIMULATION, false);
            canvas.SetProperty(Property.FONT_COLOR, null);
            p = new Paragraph("Testing font methods: resetting style and color");
            canvas.Add(p);
            canvas.SetLineThrough();
            p = new Paragraph("Testing font methods: line through (default)");
            canvas.Add(p);
            canvas.SetProperty(Property.UNDERLINE, null);
            canvas.SetUnderline();
            p = new Paragraph("Testing font methods: underline (default)");
            canvas.Add(p);
            canvas.SetProperty(Property.UNDERLINE, null);
            canvas.SetUnderline(2, 4);
            canvas.SetUnderline(ColorConstants.BLUE, 5, 0.1f, 2, -0.5f, PdfCanvasConstants.LineCapStyle.ROUND);
            p = new Paragraph("Testing font methods: underline (custom)");
            canvas.Add(p);
            canvas.SetProperty(Property.UNDERLINE, null);
            canvas.SetTextRenderingMode(PdfCanvasConstants.TextRenderingMode.STROKE);
            p = new Paragraph("Testing font methods: change text rendering mode");
            canvas.Add(p);
            canvas.SetStrokeWidth(0.1f);
            canvas.SetStrokeColor(ColorConstants.BLUE);
            p = new Paragraph("Testing font methods: change stroke width and color");
            canvas.Add(p);
            //Close document
            pdf.Close();
        }
    }
}
