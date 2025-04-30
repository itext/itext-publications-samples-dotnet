using System;
using System.IO;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Annotations
{
   
    // RemoveEmbeddedFiles.cs
    // 
    // This class demonstrates how to remove all embedded file attachments from a PDF document
    // in a single operation. Unlike the RemoveEmbeddedFile class which removes individual
    // attachments, this code completely removes the EmbeddedFiles entry from the Names
    // dictionary in the document catalog. This approach efficiently removes all document-level
    // attachments at once without needing to iterate through them individually.
 
    public class RemoveEmbeddedFiles
    {
        public static readonly String DEST = "results/sandbox/annotations/remove_embedded_files.pdf";

        public static readonly String SRC = "../../../resources/pdfs/hello_with_attachment.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new RemoveEmbeddedFiles().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));

            PdfDictionary root = pdfDoc.GetCatalog().GetPdfObject();
            PdfDictionary names = root.GetAsDictionary(PdfName.Names);

            // Remove the whole EmbeddedFiles dictionary from the Names dictionary.
            names.Remove(PdfName.EmbeddedFiles);

            pdfDoc.Close();
        }
    }
}