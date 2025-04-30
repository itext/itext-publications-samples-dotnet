using System;
using System.IO;
using iText.Barcodes;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;

namespace iText.Samples.Sandbox.Barcodes
{
   
    // BarcodeBackground.cs
    // 
    // This class demonstrates how to create a barcode with a custom background color in a PDF document.
    // The code generates a Code 128 barcode representing the string "12345XX789XXX", converts it to 
    // a PDF form XObject, then places it on a page with a light gray rectangular background.
    // This example shows how to enhance barcode presentation by adding background elements
    // to make the barcode stand out on the page.
 
    public class BarcodeBackground
    {
        public static readonly String DEST = "results/sandbox/barcodes/barcode_background.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new BarcodeBackground().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));

            Barcode128 code128 = new Barcode128(pdfDoc);
            code128.SetCode("12345XX789XXX");
            code128.SetCodeType(Barcode128.CODE128);
            PdfFormXObject xObject = code128.CreateFormXObject(ColorConstants.BLACK,
                ColorConstants.BLACK, pdfDoc);

            float x = 36;
            float y = 750;
            float width = xObject.GetWidth();
            float height = xObject.GetHeight();

            // Draw the rectangle with set background color and add the created barcode object.
            PdfCanvas canvas = new PdfCanvas(pdfDoc.AddNewPage());
            canvas.SaveState();
            canvas.SetFillColor(ColorConstants.LIGHT_GRAY);
            canvas.Rectangle(x, y, width, height);
            canvas.Fill();
            canvas.RestoreState();
            canvas.AddXObjectAt(xObject, 36, 750);

            pdfDoc.Close();
        }
    }
}