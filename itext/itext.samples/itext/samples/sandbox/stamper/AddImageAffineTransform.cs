using System;
using System.IO;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;

namespace iText.Samples.Sandbox.Stamper 
{
    public class AddImageAffineTransform 
    {
        public static readonly String DEST = "results/sandbox/stamper/add_image_affine_transform.pdf";
        public static readonly String IMG = "../../../resources/img/bruno.jpg";
        public static readonly String SRC = "../../../resources/pdfs/hello.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new AddImageAffineTransform().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            ImageData image = ImageDataFactory.Create(IMG);
            
            // Translation defines the position of the image on the page and scale transformation sets image dimensions
            // Please also note that image without scaling is drawn in 1x1 rectangle. And here we draw image on page using
            // its original size in pixels.
            AffineTransform affineTransform = AffineTransform.GetTranslateInstance(36, 300);
            
            // Make sure that the image is visible by concatenating a scale transformation
            affineTransform.Concatenate(AffineTransform.GetScaleInstance(image.GetWidth(), image.GetHeight()));
            
            PdfCanvas canvas = new PdfCanvas(pdfDoc.GetFirstPage());
            float[] matrix = new float[6];
            affineTransform.GetMatrix(matrix);
            canvas.AddImageWithTransformationMatrix(image, matrix[0], matrix[1], matrix[2], matrix[3], matrix[4], matrix[5]);
            pdfDoc.Close();
        }
    }
}
