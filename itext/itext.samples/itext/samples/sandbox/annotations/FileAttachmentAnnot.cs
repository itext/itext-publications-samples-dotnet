using System;
using System.IO;
using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Filespec;
using iText.Kernel.Pdf.Xobject;

namespace iText.Samples.Sandbox.Annotations
{
   
    // FileAttachmentAnnot.cs
    // 
    // This class demonstrates how to create a file attachment annotation in a PDF document.
    // The code creates a new PDF file with an annotation that contains an embedded Word document.
    // The annotation is displayed as an image (info icon) and includes a prompt message.
    // When clicked in a PDF viewer, the annotation allows the user to open or save the 
    // attached document. This example shows how to embed files at specific locations within
    // a PDF document rather than as general document-level attachments.
 
    public class FileAttachmentAnnot
    {
        public static readonly String DEST = "results/sandbox/annotations/file_attachment_annot.pdf";

        public static readonly String IMG = "../../../resources/img/info.png";
        public static readonly String PATH = "../../../resources/text/test.docx";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new FileAttachmentAnnot().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));

            Rectangle rect = new Rectangle(36, 700, 100, 100);
            String embeddedFileName = "test.docx";

            // the 3rd argument is the file description.
            // the 5th argument is the mime-type of the embedded file;
            // the 6th argument is the AFRelationship key value.
            PdfFileSpec fileSpec = PdfFileSpec.CreateEmbeddedFileSpec(pdfDoc, PATH, null, embeddedFileName, null, null);
            PdfAnnotation attachment = new PdfFileAttachmentAnnotation(rect, fileSpec);

            // This method sets the text that will be displayed for the annotation or the alternate description,
            // if this type of annotation does not display text.
            attachment.SetContents("Click me");

            // Create XObject and draw it with the imported image on the canvas
            // to add XObject as normal appearance.
            PdfFormXObject xObject = new PdfFormXObject(rect);
            ImageData imageData = ImageDataFactory.Create(IMG);
            PdfCanvas canvas = new PdfCanvas(xObject, pdfDoc);
            canvas.AddImageFittedIntoRectangle(imageData, rect, true);
            attachment.SetNormalAppearance(xObject.GetPdfObject());

            pdfDoc.AddNewPage().AddAnnotation(attachment);

            pdfDoc.Close();
        }
    }
}