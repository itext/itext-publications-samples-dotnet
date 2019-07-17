/*
* This example was written by Bruno Lowagie
* in the context of the book: iText 7 building blocks
*/

using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Highlevel.Chapter01 {
    /// <author>Bruno Lowagie (iText Software)</author>
    public class C01E09_ColorRendering {
        public const String DEST = "../../results/chapter01/color_rendermode.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C01E09_ColorRendering().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            // Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document document = new Document(pdf);
            // Add content
            Text title1 = new Text("The Strange Case of ").SetFontColor(ColorConstants.BLUE);
            Text title2 = new Text("Dr. Jekyll").SetStrokeColor(ColorConstants.GREEN).SetTextRenderingMode(PdfCanvasConstants.TextRenderingMode
                .FILL_STROKE);
            Text title3 = new Text(" and ");
            Text title4 = new Text("Mr. Hyde").SetStrokeColor(ColorConstants.RED).SetStrokeWidth(0.5f).SetTextRenderingMode(PdfCanvasConstants.TextRenderingMode
                .STROKE);
            Paragraph p = new Paragraph().SetFontSize(24).Add(title1).Add(title2).Add(title3).Add(title4);
            document.Add(p);
            //Close document
            document.Close();
        }
    }
}
