using System;
using System.IO;
using iText.IO.Util;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Stamper 
{
    public class WatermarkToTheSide 
    {
        public static readonly String DEST = "results/sandbox/stamper/watermark_to_the_side.pdf";
        public static readonly String SRC = "../../../resources/pdfs/pages.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new WatermarkToTheSide().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            
            for (int p = 1; p <= pdfDoc.GetNumberOfPages(); p++) 
            {
                Rectangle pageSize = pdfDoc.GetPage(p).GetPageSize();
                
                PdfCanvas canvas = new PdfCanvas(pdfDoc.GetPage(p));
                
                // In case the page has a rotation, then new content will be automatically rotated.
                // Such an automatic rotation means, that we should consider page as if it's not rotated.
                // This is the particular case for the page 3 below
                if (p == 3) 
                {
                    // The width of the page rotated by 90 degrees corresponds to the height of the unrotated one.
                    // The left side of the page rotated by 90 degrees corresponds to the bottom of the unrotated page.
                    DrawText(canvas, pdfDoc, pageSize, pageSize.GetWidth() / 2, 18, 180);
                    DrawText(canvas, pdfDoc, pageSize, pageSize.GetWidth() / 2, 34, 180);
                }
                else 
                {
                    DrawText(canvas, pdfDoc, pageSize, pageSize.GetLeft() + 18, 
                            (pageSize.GetTop() + pageSize.GetBottom()) / 2, 90);
                    DrawText(canvas, pdfDoc, pageSize, pageSize.GetLeft() + 34, 
                            (pageSize.GetTop() + pageSize.GetBottom()) / 2, 90);
                }
            }
            
            pdfDoc.Close();
        }

        private static void DrawText(PdfCanvas canvas, PdfDocument pdfDoc, Rectangle pageSize, float x, float y, double rotation) 
        {
            Canvas canvasDrawText = new Canvas(canvas, pageSize)
                    .ShowTextAligned("This is some extra text added to the left of the page",
                            x, y, TextAlignment.CENTER, (float) MathUtil.ToRadians(rotation));
            canvasDrawText.Close();
        }
    }
}
