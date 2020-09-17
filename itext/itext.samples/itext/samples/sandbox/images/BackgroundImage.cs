using System;
using System.IO;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Images
{
    public class BackgroundImage
    {
        public static readonly String DEST = "results/sandbox/images/background_image.pdf";

        public static readonly String IMAGE = "../../../resources/img/berlin2013.jpg";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new BackgroundImage().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            PageSize pageSize = PageSize.A4.Rotate();
            Document doc = new Document(pdfDoc, pageSize);

            PdfCanvas canvas = new PdfCanvas(pdfDoc.AddNewPage());
            canvas.AddImageFittedIntoRectangle(ImageDataFactory.Create(IMAGE), pageSize, false);

            doc.Add(new Paragraph("Berlin!"));

            doc.Close();
        }
    }
}