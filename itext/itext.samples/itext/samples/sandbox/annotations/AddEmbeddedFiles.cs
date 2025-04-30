using System;
using System.IO;
using System.Text;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Filespec;

namespace iText.Samples.Sandbox.Annotations
{
   
    // AddEmbeddedFiles.cs
    // 
    // This class demonstrates how to add multiple embedded file attachments to a PDF document.
    // The code opens an existing PDF document and iterates through a predefined list of text strings,
    // creating a separate file attachment for each one. Each attachment is a simple text file
    // with content derived from the string. This example shows batch processing of file attachments
    // at the document level.
 
    public class AddEmbeddedFiles
    {
        public static readonly String[] ATTACHMENTS =
        {
            "hello", "world", "what", "is", "up"
        };

        public static readonly String DEST = "results/sandbox/annotations/add_embedded_files.pdf";

        public static readonly String SRC = "../../../resources/pdfs/hello.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new AddEmbeddedFiles().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));

            foreach (String text in ATTACHMENTS)
            {
                String embeddedFileName = String.Format("{0}.txt", text);
                String embeddedFileDescription = String.Format("Some test: {0}", text);
                byte[] embeddedFileContentBytes = Encoding.UTF8.GetBytes(embeddedFileDescription);

                // the 5th argument is the mime-type of the embedded file;
                // the 6th argument is the PdfDictionary containing embedded file's parameters;
                // the 7th argument is the AFRelationship key value.
                PdfFileSpec spec = PdfFileSpec.CreateEmbeddedFileSpec(pdfDoc, embeddedFileContentBytes,
                    embeddedFileDescription, embeddedFileName, null, null, null);

                // This method adds file attachment at document level.
                pdfDoc.AddFileAttachment(String.Format("embedded_file{0}", text), spec);
            }

            pdfDoc.Close();
        }
    }
}