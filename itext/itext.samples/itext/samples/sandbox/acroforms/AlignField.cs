using System;
using System.Collections.Generic;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Acroforms
{
    public class AlignField
    {
        public static readonly String DEST = "results/sandbox/acroforms/align_field.pdf";

        public static readonly String SRC = "../../../resources/pdfs/subscribe.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new AlignField().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
            IDictionary<String, PdfFormField> fields = form.GetAllFormFields();

            PdfFormField field = fields["personal.name"];
            field.SetJustification(HorizontalAlignment.LEFT);
            field.SetValue("Test");

            field = fields["personal.loginname"];
            field.SetJustification(HorizontalAlignment.CENTER);
            field.SetValue("Test");

            field = fields["personal.password"];
            field.SetJustification(HorizontalAlignment.RIGHT);
            field.SetValue("Test");

            field = fields["personal.reason"];
            field.SetValue("Test");

            pdfDoc.Close();
        }
    }
}