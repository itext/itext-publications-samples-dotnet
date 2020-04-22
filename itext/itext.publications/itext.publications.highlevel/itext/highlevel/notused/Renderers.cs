using System;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;

namespace iText.Highlevel.Notused {
    /// <author>Bruno Lowagie (iText Software)</author>
    public class Renderers {
        public const String DEST = "results/notused/renderers.pdf";

        public const String TEXT = "This is a long sentence. We will render this paragraph in different " + "ways. We'll use the default renderer as well as a column renderer. "
             + "This is some more text to make the paragraph longer. We want it " + "to span multiple lines. An example with a short paragraph isn't "
             + "useful for us.";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new Renderers().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document document = new Document(pdf);
            Paragraph p = new Paragraph().Add(TEXT);
            document.Add(p);
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            //Set column parameters
            float offSet = 36;
            float gutter = 23;
            float columnWidth = (PageSize.A4.GetWidth() - offSet * 2) / 2 - gutter;
            float columnHeight = PageSize.A4.GetHeight() - offSet * 2;
            //Define column areas
            Rectangle[] columns = new Rectangle[] { new Rectangle(offSet, offSet, columnWidth, columnHeight), new Rectangle
                (offSet + columnWidth + gutter, offSet, columnWidth, columnHeight) };
            document.SetRenderer(new ColumnDocumentRenderer(document, columns));
            document.Add(new AreaBreak(AreaBreakType.LAST_PAGE));
            for (int i = 0; i < 10; i++) {
                document.Add(p);
            }
            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            document.SetRenderer(new DocumentRenderer(document));
            document.Add(new AreaBreak(AreaBreakType.LAST_PAGE));
            document.Add(p);
            //Close document
            document.Close();
        }
    }
}
