using System;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Acroforms
{
   
    // CheckBoxFlatten.cs
    // 
    // This example demonstrates how to flatten checkboxes in an existing PDF file.
    // Flattening a form field means converting it into a static part of the PDF content.
 
    public class CheckBoxFlatten
    {
        public static readonly String DEST = "results/sandbox/acroforms/checkbox_flatten.pdf";

        public static readonly String SRC = "../../../resources/pdfs/checkboxes.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new CheckBoxFlatten().ManipulatePdf(DEST);
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