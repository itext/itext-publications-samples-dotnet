using System;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Acroforms.Reporting
{
   
    // FlattenForm.cs
    // 
    // This example demonstrates how to fill and flatten a form.
 
    public class FlattenForm
    {
        public static readonly String DEST = "results/sandbox/acroforms/reporting/flatten_form.pdf";

        public static readonly String SRC = "../../../resources/pdfs/state.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new FlattenForm().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            PdfAcroForm form = PdfFormCreator.GetAcroForm(pdfDoc, true);

            form.GetField("name").SetValue("CALIFORNIA");
            form.GetField("abbr").SetValue("CA");
            form.GetField("capital").SetValue("Sacramento");
            form.GetField("city").SetValue("Los Angeles");
            form.GetField("population").SetValue("36,961,664");
            form.GetField("surface").SetValue("163,707");
            form.GetField("timezone1").SetValue("PT (UTC-8)");
            form.GetField("timezone2").SetValue("-");
            form.GetField("dst").SetValue("YES");

            // If no fields have been explicitly included via PartialFormFlattening(),
            // then all fields are flattened. Otherwise only the included fields are flattened.
            form.FlattenFields();

            pdfDoc.Close();
        }
    }
}