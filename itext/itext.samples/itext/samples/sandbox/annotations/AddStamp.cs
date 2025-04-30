using System;
using System.IO;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;

namespace iText.Samples.Sandbox.Annotations
{
   
    // AddStamp.cs
    // 
    // This class demonstrates how to add a custom image stamp annotation to a PDF document.
    // The code opens an existing PDF file, loads an image, creates a PdfFormXObject containing
    // the image, and then adds it to the document as a stamp annotation with a custom name.
    // The annotation is configured to be printed when the document is printed. This example
    // shows how to create visual stamps with custom images rather than using the built-in
    // stamp types.
 
    public class AddStamp
    {
        public static readonly String DEST = "results/sandbox/annotations/add_stamp.pdf";

        public static readonly String IMG = "../../../resources/img/itext.png";
        public static readonly String SRC = "../../../resources/pdfs/hello.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new AddStamp().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            ImageData img = ImageDataFactory.Create(IMG);
            float width = img.GetWidth();
            float height = img.GetHeight();
            PdfFormXObject xObj = new PdfFormXObject(new Rectangle(width, height));

            PdfCanvas canvas = new PdfCanvas(xObj, pdfDoc);
            canvas.AddImageAt(img, 0, 0, false);

            Rectangle location = new Rectangle(36, 770 - height, width, height);
            PdfStampAnnotation stamp = new PdfStampAnnotation(location);
            stamp.SetStampName(new PdfName("ITEXT"));
            stamp.SetNormalAppearance(xObj.GetPdfObject());

            // Set to print the annotation when the page is printed
            stamp.SetFlags(PdfAnnotation.PRINT);
            pdfDoc.GetFirstPage().AddAnnotation(stamp);

            pdfDoc.Close();
        }
    }
}