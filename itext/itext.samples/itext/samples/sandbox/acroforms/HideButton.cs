using System;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Acroforms
{
    public class HideButton
    {
        public static readonly String DEST = "results/sandbox/acroforms/hide_button.pdf";

        public static readonly String SRC = "../../../resources/pdfs/hello_button.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new HideButton().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);

            // Set the visibility flag of the form field annotation.
            // Options are: HIDDEN, HIDDEN_BUT_PRINTABLE, VISIBLE, VISIBLE_BUT_DOES_NOT_PRINT
            form.GetField("Test").GetFirstFormAnnotation().SetVisibility(PdfFormAnnotation.HIDDEN);

            pdfDoc.Close();
        }
    }
}