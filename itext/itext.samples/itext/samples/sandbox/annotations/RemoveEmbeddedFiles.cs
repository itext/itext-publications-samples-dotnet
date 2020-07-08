using System;
using System.IO;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Annotations
{
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