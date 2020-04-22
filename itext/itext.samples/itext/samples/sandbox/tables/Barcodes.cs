using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Barcodes;
using iText.Kernel.Pdf.Xobject;

namespace iText.Samples.Sandbox.Tables
{
    public class Barcodes
    {
        public static readonly string DEST = "results/sandbox/tables/barcodes.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new Barcodes().ManipulatePdf(DEST);
        }

        private void ManipulatePdf(string dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            Table table = new Table(UnitValue.CreatePercentArray(4)).UseAllAvailableWidth();

            for (int i = 0; i < 12; i++)
            {
                table.AddCell(CreateBarcode(string.Format("{0:d8}", i), pdfDoc));
            }

            doc.Add(table);
            
            doc.Close();
        }

        private static Cell CreateBarcode(string code, PdfDocument pdfDoc)
        {
            BarcodeEAN barcode = new BarcodeEAN(pdfDoc);
            barcode.SetCodeType(BarcodeEAN.EAN8);
            barcode.SetCode(code);

            // Create barcode object to put it to the cell as image
            PdfFormXObject barcodeObject = barcode.CreateFormXObject(null, null, pdfDoc);
            Cell cell = new Cell().Add(new Image(barcodeObject));
            cell.SetPaddingTop(10);
            cell.SetPaddingRight(10);
            cell.SetPaddingBottom(10);
            cell.SetPaddingLeft(10);

            return cell;
        }
    }
}