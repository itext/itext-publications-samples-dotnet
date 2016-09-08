/*
* This example is part of the iText 7 tutorial.
*/
using System;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Annot;
using iText.Kernel.Pdf.Canvas;
using iText.Test.Attributes;

namespace Tutorial.Chapter05 {
    /// <summary>Simple adding annotations example.</summary>
    [WrapToTest]
    public class C05E01_AddAnnotationsAndContent {
        public const String SRC = "../../resources/pdf/job_application.pdf";

        public const String DEST = "../../results/chapter05/edited_job_application.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C05E01_AddAnnotationsAndContent().ManipulatePdf(SRC, DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void ManipulatePdf(String src, String dest) {
            //Initialize PDF document
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(src), new PdfWriter(dest));
            //Add text annotation
            PdfAnnotation ann = new PdfTextAnnotation(new Rectangle(400, 795, 0, 0)).SetTitle(new PdfString("iText")).
                SetContents("Please, fill out the form.").SetOpen(true);
            pdfDoc.GetFirstPage().AddAnnotation(ann);
            PdfCanvas canvas = new PdfCanvas(pdfDoc.GetFirstPage());
            canvas.BeginText().SetFontAndSize(PdfFontFactory.CreateFont(FontConstants.HELVETICA), 12).MoveText(265, 597
                ).ShowText("I agree to the terms and conditions.").EndText();
            //Add form field
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
            PdfButtonFormField checkField = PdfFormField.CreateCheckBox(pdfDoc, new Rectangle(245, 594, 15, 15), "agreement"
                , "Off", PdfFormField.TYPE_CHECK);
            checkField.SetRequired(true);
            form.AddField(checkField);
            //Update reset button
            form.GetField("reset").SetAction(PdfAction.CreateResetForm(new String[] { "name", "language", "experience1"
                , "experience2", "experience3", "shift", "info", "agreement" }, 0));
            pdfDoc.Close();
        }
    }
}
