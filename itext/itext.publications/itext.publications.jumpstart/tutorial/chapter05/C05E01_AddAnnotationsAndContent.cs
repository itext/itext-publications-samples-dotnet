using System;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.Forms.Fields.Properties;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Annot;
using iText.Kernel.Pdf.Canvas;

namespace Tutorial.Chapter05 {
    /// <summary>Simple adding annotations example.</summary>
    public class C05E01_AddAnnotationsAndContent {
        public const String SRC = "../../../resources/pdf/job_application.pdf";

        public const String DEST = "../../../results/chapter05/edited_job_application.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C05E01_AddAnnotationsAndContent().ManipulatePdf(SRC, DEST);
        }

        public virtual void ManipulatePdf(String src, String dest) {
            //Initialize PDF document
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(src), new PdfWriter(dest));
            //Add text annotation
            PdfAnnotation ann = new PdfTextAnnotation(new Rectangle(400, 795, 0, 0))
                .SetOpen(true)
                .SetTitle(new PdfString("iText"))
                .SetContents("Please, fill out the form.");
            pdfDoc.GetFirstPage().AddAnnotation(ann);
            PdfCanvas canvas = new PdfCanvas(pdfDoc.GetFirstPage());
            canvas.BeginText().SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA), 12).MoveText(265, 597
                ).ShowText("I agree to the terms and conditions.").EndText();
            //Add form field
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
            PdfButtonFormField checkField = new CheckBoxFormFieldBuilder(pdfDoc, "agreement")
                    .SetWidgetRectangle(new Rectangle(245, 594, 15, 15))
                    .SetCheckType(CheckBoxType.CHECK).CreateCheckBox();
            checkField.SetValue("Off");
            checkField.SetRequired(true);
            form.AddField(checkField);
            //Update reset button
            form.GetField("reset").GetFirstFormAnnotation().SetAction(PdfAction.CreateResetForm(new String[] { "name", "language", "experience1"
                , "experience2", "experience3", "shift", "info", "agreement" }, 0));
            pdfDoc.Close();
        }
    }
}
