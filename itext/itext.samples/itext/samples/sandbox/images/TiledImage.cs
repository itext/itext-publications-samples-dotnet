using System;
using System.IO;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;

namespace iText.Samples.Sandbox.Images
{
    public class TiledImage
    {
        public static readonly String DEST = "results/sandbox/images/tiled_image.pdf";

        public static readonly String IMAGE = "../../../resources/img/bruno_ingeborg.jpg";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new TiledImage().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            ImageData image = ImageDataFactory.Create(IMAGE);
            float width = image.GetWidth();
            float height = image.GetHeight();
            PageSize pageSize = new PageSize(width / 2, height / 2);
            pdfDoc.SetDefaultPageSize(pageSize);
            
            PdfCanvas canvas = new PdfCanvas(pdfDoc.AddNewPage());
            canvas.AddImageWithTransformationMatrix(image, width, 0, 0, height, 0, -height / 2, false);

            canvas = new PdfCanvas(pdfDoc.AddNewPage());
            canvas.AddImageWithTransformationMatrix(image, width, 0, 0, height, 0, 0, false);

            canvas = new PdfCanvas(pdfDoc.AddNewPage());
            canvas.AddImageWithTransformationMatrix(image, width, 0, 0, height, -width / 2, -height / 2, false);

            canvas = new PdfCanvas(pdfDoc.AddNewPage());
            canvas.AddImageWithTransformationMatrix(image, width, 0, 0, height, -width / 2, 0, false);

            pdfDoc.Close();
        }
    }
}