/*
* This example is part of the iText 7 tutorial.
*/
using System;
using System.Collections.Generic;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;
using iText.Layout;

namespace Tutorial.Chapter04 {
    /// <summary>Simple filling out form example.</summary>
    public class C04E03_CreateAndFill {
        public const String DEST = "../../results/chapter04/create_and_fill.pdf";

        /// <exception cref="System.IO.IOException"/>
        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C04E03_CreateAndFill().CreatePdf(DEST);
        }

        /// <exception cref="System.IO.IOException"/>
        public virtual void CreatePdf(String dest) {
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            // Initialize document
            Document doc = new Document(pdf);
            PdfAcroForm form = C04E02_JobApplication.AddAcroForm(doc);
            IDictionary<String, PdfFormField> fields = form.GetFormFields();
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
            doc.Close();
        }
    }
}
