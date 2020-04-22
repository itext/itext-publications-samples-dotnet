using System;
using System.IO;
using iText.Barcodes;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Barcodes
{
    public class BarcodeInTable
    {
        public static readonly String DEST = "results/sandbox/barcodes/barcode_in_table.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new BarcodeInTable().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            String code = "675-FH-A12";

            Table table = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            table.AddCell("Change baseline:");

            Barcode128 code128 = new Barcode128(pdfDoc);

            // If value is positive, the text distance under the bars. If zero or negative,
            // the text distance above the bars.
            code128.SetBaseline(-1);
            code128.SetSize(12);
            code128.SetCode(code);
            code128.SetCodeType(Barcode128.CODE128);
            Image code128Image = new Image(code128.CreateFormXObject(pdfDoc));

            // Notice that in iText5 in default PdfPCell constructor (new PdfPCell(Image img))
            // this image does not fit the cell, but it does in addCell().
            // In iText7 there is no constructor (new Cell(Image img)),
            // so the image adding to the cell can be done only using method add().
            Cell cell = new Cell().Add(code128Image);
            table.AddCell(cell);
            table.AddCell("Add text and bar code separately:");

            code128 = new Barcode128(pdfDoc);
            
            // Suppress the barcode text
            code128.SetFont(null);
            code128.SetCode(code);
            code128.SetCodeType(Barcode128.CODE128);

            // Let the image resize automatically by setting it to be autoscalable.
            code128Image = new Image(code128.CreateFormXObject(pdfDoc)).SetAutoScale(true);
            cell = new Cell();
            cell.Add(new Paragraph("PO #: " + code));
            cell.Add(code128Image);
            table.AddCell(cell);

            doc.Add(table);

            doc.Close();
        }
    }
}