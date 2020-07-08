using System;
using System.IO;
using iText.Barcodes;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Barcodes
{
    public class BarcodePlacement
    {
        public static readonly String DEST = "results/sandbox/barcodes/barcode_placement.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new BarcodePlacement().ManipulatePdf(DEST);
        }

        public Image CreateBarcode(float xScale, float yScale, PdfDocument pdfDoc)
        {
            BarcodePDF417 barcode = new BarcodePDF417();
            barcode.SetCode("BarcodePDF417 barcode");
            PdfFormXObject barcodeObject = barcode.CreateFormXObject(ColorConstants.BLACK, pdfDoc);
            Image barcodeImage = new Image(barcodeObject).Scale(xScale, yScale);
            return barcodeImage;
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Image img = CreateBarcode(1, 1, pdfDoc);
            doc.Add(new Paragraph(String.Format("This barcode measures {0:0.0} by {1:0.0} user units",
                img.GetImageScaledWidth(), img.GetImageScaledHeight())));
            doc.Add(img);

            img = CreateBarcode(3, 3, pdfDoc);
            doc.Add(new Paragraph(String.Format("This barcode measures {0:0.0} by {1:0.0} user units",
                img.GetImageScaledWidth(), img.GetImageScaledHeight())));
            doc.Add(img);

            img = CreateBarcode(3, 1, pdfDoc);
            doc.Add(new Paragraph(String.Format("This barcode measures {0:0.0} by {1:0.0} user units",
                img.GetImageScaledWidth(), img.GetImageScaledHeight())));
            doc.Add(img);

            doc.Close();
        }
    }
}