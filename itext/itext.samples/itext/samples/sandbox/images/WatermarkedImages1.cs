using System;
using System.IO;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Images
{
   
    // WatermarkedImages1.cs
    //
    // Example showing how to add rotated text watermarks to images.
    // Demonstrates creating form XObjects with overlaid text on images.
 
    public class WatermarkedImages1
    {
        public static readonly String DEST = "results/sandbox/images/watermarked_images1.pdf";

        public static readonly String IMAGE1 = "../../../resources/img/bruno.jpg";
        public static readonly String IMAGE2 = "../../../resources/img/dog.bmp";
        public static readonly String IMAGE3 = "../../../resources/img/fox.bmp";
        public static readonly String IMAGE4 = "../../../resources/img/bruno_ingeborg.jpg";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new WatermarkedImages1().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Image image = GetWatermarkedImage(pdfDoc, new Image(ImageDataFactory.Create(IMAGE1)), "Bruno");
            doc.Add(image);

            image = GetWatermarkedImage(pdfDoc, new Image(ImageDataFactory.Create(IMAGE2)), "Dog");
            doc.Add(image);

            image = GetWatermarkedImage(pdfDoc, new Image(ImageDataFactory.Create(IMAGE3)), "Fox");
            doc.Add(image);

            Image srcImage = new Image(ImageDataFactory.Create(IMAGE4));
            srcImage.ScaleToFit(400, 700);
            image = GetWatermarkedImage(pdfDoc, srcImage, "Bruno and Ingeborg");
            doc.Add(image);

            doc.Close();
        }

        private static Image GetWatermarkedImage(PdfDocument pdfDoc, Image img, String watermark)
        {
            float width = img.GetImageScaledWidth();
            float height = img.GetImageScaledHeight();
            PdfFormXObject template = new PdfFormXObject(new Rectangle(width, height));
            new Canvas(template, pdfDoc)
                .Add(img)
                .SetFontColor(DeviceGray.WHITE)
                .ShowTextAligned(watermark, width / 2, height / 2, TextAlignment.CENTER, (float) Math.PI / 6)
                .Close();
            return new Image(template);
        }
    }
}