/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2020 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.Collections.Generic;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.IO.Source;
using iText.IO.Util;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Acroforms.Reporting
{
    public class FillFlattenMerge1
    {
        public static readonly String DEST = "results/sandbox/acroforms/reporting/fill_flatten_merge1.pdf";

        public static readonly String DATA = "../../../resources/data/united_states.csv";
        public static readonly String SRC = "../../../resources/pdfs/state.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new FillFlattenMerge1().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            PdfPageFormCopier formCopier = new PdfPageFormCopier();

            // Initialize an outline tree of the document and sets outline mode to true
            pdfDoc.InitializeOutlines();

            using (StreamReader streamReader = new StreamReader(DATA))
            {
                // Read first line with headers,
                // do nothing with this line, because headers are already filled in form
                String line = streamReader.ReadLine();

                while ((line = streamReader.ReadLine()) != null)
                {
                    // Create a PDF in memory
                    ByteArrayOutputStream baos = new ByteArrayOutputStream();
                    PdfDocument pdfInnerDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(baos));
                    PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfInnerDoc, true);

                    // Parse text line and fill all fields of form
                    FillAndFlattenForm(line, form);
                    pdfInnerDoc.Close();

                    // Copy page with current filled form to the result pdf document
                    pdfInnerDoc = new PdfDocument(new PdfReader(new MemoryStream(baos.ToArray())));
                    pdfInnerDoc.CopyPagesTo(1, pdfInnerDoc.GetNumberOfPages(), pdfDoc, formCopier);
                    pdfInnerDoc.Close();
                }
            }

            pdfDoc.Close();
        }

        public void FillAndFlattenForm(String line, PdfAcroForm form)
        {
            StringTokenizer tokenizer = new StringTokenizer(line, ";");
            IDictionary<String, PdfFormField> fields = form.GetFormFields();

            fields["name"].SetValue(tokenizer.NextToken());
            fields["abbr"].SetValue(tokenizer.NextToken());
            fields["capital"].SetValue(tokenizer.NextToken());
            fields["city"].SetValue(tokenizer.NextToken());
            fields["population"].SetValue(tokenizer.NextToken());
            fields["surface"].SetValue(tokenizer.NextToken());
            fields["timezone1"].SetValue(tokenizer.NextToken());
            fields["timezone2"].SetValue(tokenizer.NextToken());
            fields["dst"].SetValue(tokenizer.NextToken());

            // If no fields have been explicitly included via partialFormFlattening(),
            // then all fields are flattened. Otherwise only the included fields are flattened.
            form.FlattenFields();
        }
    }
}