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
using System.Diagnostics;

namespace Tutorial.Chapter06 {
    public class C06E09_FillOutFlattenAndMergeForms {
        public const String DEST1 = "../../../results/chapter06/fill_out_flatten_forms_merge.pdf";

        public const String DEST2 = "../../../results/chapter06/fill_out_flatten_forms_smart_merge.pdf";

        public const String SRC = "../../../resources/pdf/state.pdf";

        public const String DATA = "../../../resources/data/united_states.csv";

        public static void Main(String[] args) {
            FileInfo file = new FileInfo(DEST1);
            file.Directory.Create();
            file = new FileInfo(DEST2);
            file.Directory.Create();
            new C06E09_FillOutFlattenAndMergeForms().CreatePdf(DEST1, DEST2);
        }

        public virtual void CreatePdf(String dest1, String dest2) {
            PdfDocument destPdfDocument = new PdfDocument(new PdfWriter(dest1));
            //Smart mode
            PdfDocument destPdfDocumentSmartMode = new PdfDocument(new PdfWriter(dest2).SetSmartMode(true));
            using (StreamReader sr = File.OpenText(DATA))
            {
                String line;
                bool headerLine = true;
                int i = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    if (headerLine)
                    {
                        headerLine = false;
                        continue;
                    }

                    ByteArrayOutputStream baos = new ByteArrayOutputStream();
                    PdfDocument sourcePdfDocument = new PdfDocument(new PdfReader(SRC), new PdfWriter(baos));
                    //Read fields
                    PdfAcroForm form = PdfAcroForm.GetAcroForm(sourcePdfDocument, true);
                    StringTokenizer tokenizer = new StringTokenizer(line, ";");
                    IDictionary<String, PdfFormField> fields = form.GetFormFields();
                    //Fill out fields
                    PdfFormField toSet;
                    fields.TryGetValue("name", out toSet);
                    toSet.SetValue(tokenizer.NextToken());
                    fields.TryGetValue("abbr", out toSet);
                    toSet.SetValue(tokenizer.NextToken());
                    fields.TryGetValue("capital", out toSet);
                    toSet.SetValue(tokenizer.NextToken());
                    fields.TryGetValue("city", out toSet);
                    toSet.SetValue(tokenizer.NextToken());
                    fields.TryGetValue("population", out toSet);
                    toSet.SetValue(tokenizer.NextToken());
                    fields.TryGetValue("surface", out toSet);
                    toSet.SetValue(tokenizer.NextToken());
                    fields.TryGetValue("timezone1", out toSet);
                    toSet.SetValue(tokenizer.NextToken());
                    fields.TryGetValue("timezone2", out toSet);
                    toSet.SetValue(tokenizer.NextToken());
                    fields.TryGetValue("dst", out toSet);
                    toSet.SetValue(tokenizer.NextToken());
                    //Flatten fields
                    form.FlattenFields();
                    sourcePdfDocument.Close();
                    sourcePdfDocument = new PdfDocument(new PdfReader(new MemoryStream(baos.ToArray())));
                    //Copy pages
                    sourcePdfDocument.CopyPagesTo(1, sourcePdfDocument.GetNumberOfPages(), destPdfDocument, null);
                    sourcePdfDocument.CopyPagesTo(1, sourcePdfDocument.GetNumberOfPages(), destPdfDocumentSmartMode,
                        null);
                    sourcePdfDocument.Close();
                }
            }

            destPdfDocument.Close();
            destPdfDocumentSmartMode.Close();
        }
    }
}
