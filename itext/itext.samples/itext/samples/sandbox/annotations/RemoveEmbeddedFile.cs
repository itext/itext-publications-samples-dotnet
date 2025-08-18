using System;
using System.IO;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Annotations
{
   
    // RemoveEmbeddedFile.cs
    // 
    // This class demonstrates how to remove embedded files from a PDF document.
    // The code opens an existing PDF that contains file attachments, navigates through
    // the PDF dictionary structure to locate the embedded files array, and removes both
    // the description and reference entries for the attachment. This example shows how
    // to access and modify low-level PDF structures to remove document-level attachments.
 
    public class RemoveEmbeddedFile
    {
        public static readonly String DEST = "results/sandbox/annotations/remove_embedded_file.pdf";

        public static readonly String SRC = "../../../resources/pdfs/hello_with_attachment.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new RemoveEmbeddedFile().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));

            PdfDictionary root = pdfDoc.GetCatalog().GetPdfObject();
            PdfDictionary names = root.GetAsDictionary(PdfName.Names);
            PdfDictionary embeddedFiles = names.GetAsDictionary(PdfName.EmbeddedFiles);
            PdfArray namesArray = embeddedFiles.GetAsArray(PdfName.Names);

            // Remove the description of the embedded file
            namesArray.Remove(0);

            // Remove the reference to the embedded file.
            namesArray.Remove(0);

            pdfDoc.Close();
        }
    }
}