using System;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Acroforms
{
   
    // FlattenSignature.cs
    // 
    // This example demonstrates how to flatten a signed PDF form, converting the signature field
    // into a static part of the PDF content.
 
    public class FlattenSignature
    {
        public static readonly String DEST = "results/sandbox/acroforms/flatten_signature.pdf";

        public static readonly String SRC = "../../../resources/pdfs/input_signed.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new FlattenSignature().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            PdfAcroForm form = PdfFormCreator.GetAcroForm(pdfDoc, true);

            // If no fields have been explicitly included, then all fields are flattened.
            // Otherwise only the included fields are flattened.
            form.FlattenFields();

            pdfDoc.Close();
        }
    }
}