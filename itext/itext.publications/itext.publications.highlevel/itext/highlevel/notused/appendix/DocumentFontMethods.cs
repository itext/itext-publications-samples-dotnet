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
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Highlevel.Notused.Appendix {
    /// <author>Bruno Lowagie (iText Software)</author>
    public class DocumentFontMethods {
        public const String DEST = "results/appendix/document_font_methods.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new DocumentFontMethods().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            // Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document document = new Document(pdf);
            Paragraph p;
            p = new Paragraph("Testing font methods");
            document.Add(p);
            PdfFont font = PdfFontFactory.CreateFont(FontConstants.TIMES_ROMAN);
            document.SetFont(font);
            p = new Paragraph("Testing font methods: changed font");
            document.Add(p);
            document.SetFontSize(18);
            p = new Paragraph("Testing font methods: changed font size");
            document.Add(p);
            document.SetFontColor(Color.BLUE);
            p = new Paragraph("Testing font methods: changed color");
            document.Add(p);
            document.SetBold();
            p = new Paragraph("Testing font methods: to bold");
            document.Add(p);
            document.SetItalic();
            p = new Paragraph("Testing font methods: to italic");
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
            document.SetUnderline(Color.BLUE, 5, 0.1f, 2, -0.5f, PdfCanvasConstants.LineCapStyle.ROUND);
            p = new Paragraph("Testing font methods: underline (custom)");
            document.Add(p);
            document.SetProperty(Property.UNDERLINE, null);
            document.SetTextRenderingMode(PdfCanvasConstants.TextRenderingMode.STROKE);
            p = new Paragraph("Testing font methods: change text rendering mode");
            document.Add(p);
            document.SetStrokeWidth(0.1f);
            document.SetStrokeColor(Color.BLUE);
            p = new Paragraph("Testing font methods: change stroke width and color");
            document.Add(p);
            //Close document
            document.Close();
        }
    }
}
