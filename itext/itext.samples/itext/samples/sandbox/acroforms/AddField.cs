using System;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;

namespace iText.Samples.Sandbox.Acroforms
{
    public class AddField
    {
        public static readonly String DEST = "results/sandbox/acroforms/add_field.pdf";

        public static readonly String SRC = "../../../resources/pdfs/form.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new AddField().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);

            PdfButtonFormField button = new PushButtonFormFieldBuilder(pdfDoc, "post")
                .SetWidgetRectangle(new Rectangle(36, 700, 36, 30)).SetCaption("POST").CreatePushButton();
            button.GetFirstFormAnnotation().SetBackgroundColor(ColorConstants.GRAY);
            button.SetValue("POST");

            // The second parameter is optional, it declares which fields to include in the submission or which to exclude,
            // depending on the setting of the Include/Exclude flag.
            button.GetFirstFormAnnotation().SetAction(PdfAction.CreateSubmitForm("http://itextpdf.com:8180/book/request", null,
                PdfAction.SUBMIT_HTML_FORMAT | PdfAction.SUBMIT_COORDINATES));
            button.GetFirstFormAnnotation().SetVisibility(PdfFormAnnotation.VISIBLE_BUT_DOES_NOT_PRINT);
            form.AddField(button);

            pdfDoc.Close();
        }
    }
}