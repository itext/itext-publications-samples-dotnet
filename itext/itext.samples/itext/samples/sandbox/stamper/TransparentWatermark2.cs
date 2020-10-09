using System;
using System.IO;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Extgstate;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Stamper 
{
    public class TransparentWatermark2 
    {
        public static readonly String DEST = "results/sandbox/stamper/transparent_watermark2.pdf";
        public static readonly String IMG = "../../../resources/img/itext.png";
        public static readonly String SRC = "../../../resources/pdfs/pages.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new TransparentWatermark2().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            Paragraph paragraph = new Paragraph("My watermark (text)")
                    .SetFont(font)
                    .SetFontSize(30);
            ImageData img = ImageDataFactory.Create(IMG);
            
            float w = img.GetWidth();
            float h = img.GetHeight();
            
            PdfExtGState gs1 = new PdfExtGState().SetFillOpacity(0.5f);
            
            // Implement transformation matrix usage in order to scale image
            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++) 
            {
                PdfPage pdfPage = pdfDoc.GetPage(i);
                Rectangle pageSize = pdfPage.GetPageSizeWithRotation();
                
                // When "true": in case the page has a rotation, then new content will be automatically rotated in the
                // opposite direction. On the rotated page this would look as if new content ignores page rotation.
                pdfPage.SetIgnorePageRotationForContent(true);
                
                float x = (pageSize.GetLeft() + pageSize.GetRight()) / 2;
                float y = (pageSize.GetTop() + pageSize.GetBottom()) / 2;
                PdfCanvas over = new PdfCanvas(pdfDoc.GetPage(i));
                over.SaveState();
                over.SetExtGState(gs1);
                if (i % 2 == 1) 
                {
                    doc.ShowTextAligned(paragraph, x, y, i, TextAlignment.CENTER, VerticalAlignment.TOP, 0);
                }
                else 
                {
                    over.AddImageWithTransformationMatrix(img, w, 0, 0, h, x - (w / 2), y - (h / 2), false);
                }
                over.RestoreState();
            }
            
            doc.Close();
        }
    }
}
