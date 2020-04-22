using System;
using System.IO;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Images
{
    public class FlateCompressJPEG1Pass
    {
        public static readonly String DEST = "results/sandbox/images/flate_compress_jpeg_1pass.pdf";

        public static readonly String IMAGE = "../../../resources/img/berlin2013.jpg";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new FlateCompressJPEG1Pass().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            PageSize pageSize = PageSize.A4.Rotate();
            Document doc = new Document(pdfDoc, pageSize);

            Image image = new Image(ImageDataFactory.Create(IMAGE));
            image.GetXObject().GetPdfObject().SetCompressionLevel(CompressionConstants.BEST_COMPRESSION);
            image.ScaleAbsolute(pageSize.GetWidth(), pageSize.GetHeight());
            image.SetFixedPosition(0, 0);
            doc.Add(image);

            doc.Close();
        }
    }
}