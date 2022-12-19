using System;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;

namespace iText.Samples.Sandbox.Acroforms
{
    public class FileSelectionExample
    {
        public static readonly String DEST = "results/sandbox/acroforms/file_selection_example.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new FileSelectionExample().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);

            PdfTextFormField field = new TextFormFieldBuilder(pdfDoc, "myfile")
                .SetWidgetRectangle(new Rectangle(36, 788, 523, 18)).CreateText();
            field.SetValue("");

            // If true is passed, then the text entered in the field will represent the pathname of a file
            // whose contents are to be submitted as the value of the field.
            field.SetFileSelect(true);

            // When the mouse is released inside the annotation's area (that's what PdfName.U stands for),
            // then the focus will be set on the "mytitle" field.
            field.SetAdditionalAction(PdfName.U, PdfAction.CreateJavaScript(
                "this.getField('myfile').browseForFileToSubmit();"
                + "this.getField('mytitle').setFocus();"));
            form.AddField(field);

            PdfTextFormField title = new TextFormFieldBuilder(pdfDoc, "mytitle")
                .SetWidgetRectangle(new Rectangle(36, 752, 523, 18)).CreateText();
            title.SetValue("");
            form.AddField(title);

            pdfDoc.Close();
        }
    }
}