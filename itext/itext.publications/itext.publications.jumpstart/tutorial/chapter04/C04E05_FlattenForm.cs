using System;
using System.Collections.Generic;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;

namespace Tutorial.Chapter04 {
    /// <summary>Simple filling out form example.</summary>
    public class C04E05_FlattenForm {
        public const String SRC = "../../../resources/pdf/job_application.pdf";

        public const String DEST = "../../../results/chapter04/flatten_form.pdf";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C04E05_FlattenForm().ManipulatePdf(SRC, DEST);
        }

        public virtual void ManipulatePdf(String src, String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfReader(src), new PdfWriter(dest));
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdf, true);
            IDictionary<String, PdfFormField> fields = form.GetAllFormFields();
            PdfFormField toSet;
            fields.TryGetValue("name", out toSet);
            toSet.SetValue("James Bond");
            fields.TryGetValue("language", out toSet);
            toSet.SetValue("English");
            fields.TryGetValue("experience1", out toSet);
            toSet.SetValue("Off");
            fields.TryGetValue("experience2", out toSet);
            toSet.SetValue("Yes");
            fields.TryGetValue("experience3", out toSet);
            toSet.SetValue("Yes");
            fields.TryGetValue("shift", out toSet);
            toSet.SetValue("Any");
            fields.TryGetValue("info", out toSet);
            toSet.SetValue("I was 38 years old when I became an MI6 agent.");
            form.FlattenFields();
            pdf.Close();
        }
    }
}
