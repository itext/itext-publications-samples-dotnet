using System;
using System.IO;
using iText.Kernel.Pdf;
using iText.PdfCleanup;

namespace iText.Samples.Sandbox.Parse
{
   
    // RemoveRedactedContent.cs
    //
    // Example showing how to apply and remove redaction annotations.
    // Demonstrates using PdfCleaner to process redact annotation markers.
 
    public class RemoveRedactedContent
    {
        public static readonly String DEST = "results/sandbox/parse/remove_redacted_content.pdf";

        public static readonly String SRC = "../../../resources/pdfs/page229_redacted.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new RemoveRedactedContent().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));

            // If you use CleanUpRedactAnnotations method, then regions to be erased are extracted from the redact annotations
            // contained inside the given document.
            PdfCleaner.CleanUpRedactAnnotations(pdfDoc);

            pdfDoc.Close();
        }
    }
}