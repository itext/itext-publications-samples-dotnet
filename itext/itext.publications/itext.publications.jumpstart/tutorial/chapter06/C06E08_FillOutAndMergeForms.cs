/*
* This example is part of the iText 7 tutorial.
*/
using System;
using System.Collections.Generic;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.IO.Source;
using iText.IO.Util;
using iText.Kernel.Pdf;

namespace Tutorial.Chapter06 {
    public class C06E08_FillOutAndMergeForms {
        public const String DEST = "../../../results/chapter06/fill_out_and_merge_forms.pdf";

        public const String SRC = "../../../resources/pdf/state.pdf";

        public const String DATA = "../../../resources/data/united_states.csv";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new C06E08_FillOutAndMergeForms().CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest) {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            PdfPageFormCopier formCopier = new PdfPageFormCopier();
            using (StreamReader sr = File.OpenText(DATA))
            {
                String line;
                bool headerLine = true;
                int i = 1;
                while ((line = sr.ReadLine()) != null)
                {
                    if (headerLine)
                    {
                        headerLine = false;
                        continue;
                    }

                    ByteArrayOutputStream baos = new ByteArrayOutputStream();
                    PdfDocument sourcePdfDocument = new PdfDocument(new PdfReader(SRC), new PdfWriter(baos));
                    //Rename fields
                    i++;
                    PdfAcroForm form = PdfAcroForm.GetAcroForm(sourcePdfDocument, true);
                    form.RenameField("name", "name_" + i);
                    form.RenameField("abbr", "abbr_" + i);
                    form.RenameField("capital", "capital_" + i);
                    form.RenameField("city", "city_" + i);
                    form.RenameField("population", "population_" + i);
                    form.RenameField("surface", "surface_" + i);
                    form.RenameField("timezone1", "timezone1_" + i);
                    form.RenameField("timezone2", "timezone2_" + i);
                    form.RenameField("dst", "dst_" + i);
                    //Fill out fields
                    StringTokenizer tokenizer = new StringTokenizer(line, ";");
                    IDictionary<String, PdfFormField> fields = form.GetFormFields();
                    PdfFormField toSet;
                    fields.TryGetValue("name_" + i, out toSet);
                    toSet.SetValue(tokenizer.NextToken());
                    fields.TryGetValue("abbr_" + i, out toSet);
                    toSet.SetValue(tokenizer.NextToken());
                    fields.TryGetValue("capital_" + i, out toSet);
                    toSet.SetValue(tokenizer.NextToken());
                    fields.TryGetValue("city_" + i, out toSet);
                    toSet.SetValue(tokenizer.NextToken());
                    fields.TryGetValue("population_" + i, out toSet);
                    toSet.SetValue(tokenizer.NextToken());
                    fields.TryGetValue("surface_" + i, out toSet);
                    toSet.SetValue(tokenizer.NextToken());
                    fields.TryGetValue("timezone1_" + i, out toSet);
                    toSet.SetValue(tokenizer.NextToken());
                    fields.TryGetValue("timezone2_" + i, out toSet);
                    toSet.SetValue(tokenizer.NextToken());
                    fields.TryGetValue("dst_" + i, out toSet);
                    toSet.SetValue(tokenizer.NextToken());
                    sourcePdfDocument.Close();
                    sourcePdfDocument = new PdfDocument(new PdfReader(new MemoryStream(baos.ToArray())));
                    //Copy pages
                    sourcePdfDocument.CopyPagesTo(1, sourcePdfDocument.GetNumberOfPages(), pdfDocument, formCopier);
                    sourcePdfDocument.Close();
                }
            }

            pdfDocument.Close();
        }
    }
}
