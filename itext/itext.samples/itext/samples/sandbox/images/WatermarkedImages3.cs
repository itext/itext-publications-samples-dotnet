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
    public class WatermarkedImages3
    {
        public static readonly String DEST = "results/sandbox/images/watermarked_images3.pdf";

        public static readonly String IMAGE1 = "../../../resources/img/bruno.jpg";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new WatermarkedImages3().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Table table = new Table(1).SetWidth(UnitValue.CreatePercentValue(80));
            for (int i = 0; i < 35; i++)
            {
                table.AddCell(new Cell().Add(new Paragraph("rahlrokks doesn't listen to what people tell him")));
            }

            Image img = GetWatermarkedImage(pdfDoc, new Image(ImageDataFactory.Create(IMAGE1)), "Bruno")
                .SetAutoScale(true);
            table.AddCell(new Cell().Add(img));
            doc.Add(table);

            doc.ShowTextAligned("Bruno knows best", 260, 400,
                TextAlignment.CENTER, 45f * (float) Math.PI / 180f);

            doc.Close();
        }

        private static Image GetWatermarkedImage(PdfDocument pdfDoc, Image img, String watermark)
        {
            float width = img.GetImageScaledWidth();
            float height = img.GetImageScaledHeight();
            float coordX = width / 2;
            float coordY = height / 2;
            float angle = (float) Math.PI * 30f / 180f;
            PdfFormXObject template = new PdfFormXObject(new Rectangle(width, height));
            new Canvas(template, pdfDoc)
                .Add(img)
                .SetFontColor(DeviceGray.WHITE)
                .ShowTextAligned(watermark, coordX, coordY, TextAlignment.CENTER, angle)
                .Close();
            return new Image(template);
        }
    }
}