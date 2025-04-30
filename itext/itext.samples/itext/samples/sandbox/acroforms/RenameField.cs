using System;
using System.Collections.Generic;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Acroforms
{
   
    // RenameField.cs
    // 
    // This example demonstrates how to rename a form field in a PDF document.
    // It changes the field name from "personal.loginname" to "login" and then verifies 
    // the change by reopening the document and printing all field names to the console.
 
    public class RenameField
    {
        public static readonly String DEST = "results/sandbox/acroforms/rename_field.pdf";

        public static readonly String SRC = "../../../resources/pdfs/subscribe.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new RenameField().ManipulatePdf(DEST);
        }

        public void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            PdfAcroForm form = PdfFormCreator.GetAcroForm(pdfDoc, true);

            form.RenameField("personal.loginname", "login");

            pdfDoc.Close();

            pdfDoc = new PdfDocument(new PdfReader(dest));
            form = PdfFormCreator.GetAcroForm(pdfDoc, true);
            IDictionary<String, PdfFormField> fields = form.GetAllFormFields();

            // See the renamed field in the console
            foreach (String name in fields.Keys)
            {
                Console.WriteLine(name);
            }

            pdfDoc.Close();
        }
    }
}