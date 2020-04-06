/*
* This example is part of the iText 7 tutorial.
*/
using System;
using System.Collections.Generic;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;

namespace Tutorial.Chapter05 {
    /// <summary>Simple filling out form example.</summary>
    public class C05E02_FillAndModifyForm {
        public const String SRC = "../../../resources/pdf/job_application.pdf";

        public const String DEST = "../../../results/chapter05/filled_out_job_application.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C05E02_FillAndModifyForm().ManipulatePdf(SRC, DEST);
        }

        public virtual void ManipulatePdf(String src, String dest) {
            //Initialize PDF document
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(src), new PdfWriter(dest));
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
            IDictionary<String, PdfFormField> fields = form.GetFormFields();
            fields["name"].SetValue("James Bond").SetBackgroundColor(ColorConstants.ORANGE);
            fields["language"].SetValue("English");
            fields["experience1"].SetValue("Yes");
            fields["experience2"].SetValue("Yes");
            fields["experience3"].SetValue("Yes");
            IList<PdfObject> options = new List<PdfObject>();
            options.Add(new PdfString("Any"));
            options.Add(new PdfString("8.30 am - 12.30 pm"));
            options.Add(new PdfString("12.30 pm - 4.30 pm"));
            options.Add(new PdfString("4.30 pm - 8.30 pm"));
            options.Add(new PdfString("8.30 pm - 12.30 am"));
            options.Add(new PdfString("12.30 am - 4.30 am"));
            options.Add(new PdfString("4.30 am - 8.30 am"));
            PdfArray arr = new PdfArray(options);
            fields["shift"].SetOptions(arr);
            fields["shift"].SetValue("Any");
            PdfFont courier = PdfFontFactory.CreateFont(StandardFonts.COURIER);
            fields["info"].SetValue("I was 38 years old when I became an MI6 agent.", courier, 7f);
            pdfDoc.Close();
        }
    }
}
