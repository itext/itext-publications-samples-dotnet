using System;
using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;

namespace iText.Samples.Sandbox.Stamper 
{
    public class TileClipped 
    {
        public static readonly String DEST = "results/sandbox/stamper/tile_clipped.pdf";
        public static readonly String SRC = "../../../resources/pdfs/hero.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new TileClipped().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest) 
        {
            float margin = 30;
            PdfDocument srcDoc = new PdfDocument(new PdfReader(SRC));
            Rectangle rect = srcDoc.GetFirstPage().GetPageSizeWithRotation();
            Rectangle pageSize = new Rectangle(rect.GetWidth() + margin * 2, rect.GetHeight() + margin * 2);
            
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            
            // The functionality below will work only for the pages, added after the method is called
            pdfDoc.SetDefaultPageSize(new PageSize(pageSize));
            
            PdfCanvas content = new PdfCanvas(pdfDoc.AddNewPage());
            PdfFormXObject page = srcDoc.GetFirstPage().CopyAsFormXObject(pdfDoc);
            
            // Adding the same page 16 times with a different offset
            for (int i = 0; i < 16; i++) 
            {
                float x = -rect.GetWidth() * (i % 4) + margin;
                float y = rect.GetHeight() * (i / 4 - 3) + margin;
                content.Rectangle(margin, margin, rect.GetWidth(), rect.GetHeight());
                content.Clip();
                content.EndPath();
                content.AddXObject(page, 4, 0, 0, 4, x, y);
                if (15 != i) 
                {
                    content = new PdfCanvas(pdfDoc.AddNewPage());
                }
            }
            
            pdfDoc.Close();
        }
    }
}
