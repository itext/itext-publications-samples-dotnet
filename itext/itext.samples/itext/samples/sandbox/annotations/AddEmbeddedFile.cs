using System;
using System.IO;
using System.Text;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Filespec;

namespace iText.Samples.Sandbox.Annotations
{
   
    // AddEmbeddedFile.cs
    // 
    // This class demonstrates how to add an embedded file attachment to a PDF document.
    // The code opens an existing PDF document, creates a file specification for a simple 
    // text file, and attaches it at the document level. This process allows for embedding 
    // arbitrary file data directly within the PDF structure.
 
    public class AddEmbeddedFile
    {
        public static readonly String DEST = "results/sandbox/annotations/add_embedded_file.pdf";

        public static readonly String SRC = "../../../resources/pdfs/hello.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new AddEmbeddedFile().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));

            String embeddedFileName = "test.txt";
            String embeddedFileDescription = "some_test";
            byte[] embeddedFileContentBytes = Encoding.UTF8.GetBytes("Some test");

            // the 5th argument is the mime-type of the embedded file;
            // the 6th argument is the PdfDictionary containing embedded file's parameters;
            // the 7th argument is the AFRelationship key value.
            PdfFileSpec spec = PdfFileSpec.CreateEmbeddedFileSpec(pdfDoc, embeddedFileContentBytes,
                embeddedFileDescription, embeddedFileName, null, null, null);

            // This method adds file attachment at document level.
            pdfDoc.AddFileAttachment("embedded_file", spec);

            pdfDoc.Close();
        }
    }
}