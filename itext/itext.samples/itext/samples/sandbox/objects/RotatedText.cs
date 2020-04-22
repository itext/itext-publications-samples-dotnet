using System;
using System.IO;
using iText.Barcodes;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Objects
{
    public class RotatedText
    {
        public static readonly string DEST = "results/sandbox/objects/rotated_text.pdf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new RotatedText().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc, new PageSize(60, 140));
            doc.SetMargins(5, 5, 5, 5);

            PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            PdfFont regularFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

            Paragraph p1 = new Paragraph();
            p1.Add(new Text("23").SetFont(boldFont).SetFontSize(12));
            p1.Add(new Text("000").SetFont(boldFont).SetFontSize(6));
            doc.Add(p1);

            Paragraph p2 = new Paragraph("T.T.C.").SetFont(regularFont).SetFontSize(6);
            p2.SetTextAlignment(TextAlignment.RIGHT);
            doc.Add(p2);

            BarcodeEAN barcode = new BarcodeEAN(pdfDoc);
            barcode.SetCodeType(BarcodeEAN.EAN8);
            barcode.SetCode("12345678");

            Rectangle rect = barcode.GetBarcodeSize();
            PdfFormXObject formXObject = new PdfFormXObject(new Rectangle(rect.GetWidth(), rect.GetHeight() + 10));
            PdfCanvas pdfCanvas = new PdfCanvas(formXObject, pdfDoc);
            new Canvas(pdfCanvas, pdfDoc, new Rectangle(rect.GetWidth(), rect.GetHeight() + 10))
                .ShowTextAligned(new Paragraph("DARK GRAY").SetFont(regularFont).SetFontSize(6), 0,
                    rect.GetHeight() + 2, TextAlignment.LEFT);
            barcode.PlaceBarcode(pdfCanvas, ColorConstants.BLACK, ColorConstants.BLACK);

            Image image = new Image(formXObject);
            image.SetRotationAngle(Math.PI / 2f);
            image.SetAutoScale(true);
            doc.Add(image);

            Paragraph p3 = new Paragraph("SMALL").SetFont(regularFont).SetFontSize(6);
            p3.SetTextAlignment(TextAlignment.CENTER);
            doc.Add(p3);

            doc.Close();
        }
    }
}