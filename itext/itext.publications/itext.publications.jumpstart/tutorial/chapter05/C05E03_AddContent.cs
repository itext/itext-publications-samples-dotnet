/*
* This example is part of the iText 7 tutorial.
*/
using System;
using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Extgstate;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace Tutorial.Chapter05 {
    /// <summary>Simple adding content example.</summary>
    public class C05E03_AddContent {
        public const String SRC = "../../../resources/pdf/ufo.pdf";

        public const String DEST = "../../../results/chapter05/add_content.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C05E03_AddContent().ManipulatePdf(SRC, DEST);
        }

        public virtual void ManipulatePdf(String src, String dest) {
            //Initialize PDF document
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(src), new PdfWriter(dest));
            Document document = new Document(pdfDoc);
            Rectangle pageSize;
            PdfCanvas canvas;
            int n = pdfDoc.GetNumberOfPages();
            for (int i = 1; i <= n; i++) {
                PdfPage page = pdfDoc.GetPage(i);
                pageSize = page.GetPageSize();
                canvas = new PdfCanvas(page);
                //Draw header text
                canvas.BeginText().SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA), 7).MoveText(pageSize
                    .GetWidth() / 2 - 24, pageSize.GetHeight() - 10).ShowText("I want to believe").EndText();
                //Draw footer line
                canvas.SetStrokeColor(ColorConstants.BLACK).SetLineWidth(.2f).MoveTo(pageSize.GetWidth() / 2 - 30, 20).LineTo(pageSize
                    .GetWidth() / 2 + 30, 20).Stroke();
                //Draw page number
                canvas.BeginText().SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA), 7).MoveText(pageSize
                    .GetWidth() / 2 - 7, 10).ShowText(i.ToString()).ShowText(" of ").ShowText(n.ToString()).EndText();
                //Draw watermark
                Paragraph p = new Paragraph("CONFIDENTIAL").SetFontSize(60);
                canvas.SaveState();
                PdfExtGState gs1 = new PdfExtGState().SetFillOpacity(0.2f);
                canvas.SetExtGState(gs1);
                document.ShowTextAligned(p, pageSize.GetWidth() / 2, pageSize.GetHeight() / 2, pdfDoc.GetPageNumber(page), 
                    TextAlignment.CENTER, VerticalAlignment.MIDDLE, 45);
                canvas.RestoreState();
            }
            pdfDoc.Close();
        }
    }
}
