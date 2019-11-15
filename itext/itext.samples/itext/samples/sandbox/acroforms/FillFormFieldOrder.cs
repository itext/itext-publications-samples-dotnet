/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
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
using iText.Kernel.Pdf;
using iText.Layout;

namespace iText.Samples.Sandbox.Acroforms
{
    public class FillFormFieldOrder
    {
        public static readonly String DEST = "../../results/sandbox/acroforms/fill_form_field_order.pdf";

        public static readonly String SRC = "../../resources/pdfs/calendar_example.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new FillFormFieldOrder().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            
            // Get partially flattened form's byte array with content, which should be placed beneath the other content.
            byte[] tempForm = CreatePartiallyFlattenedForm();
            CreateResultantPdf(dest, tempForm);
        }

        public virtual byte[] CreatePartiallyFlattenedForm()
        {
            ByteArrayOutputStream baos = new ByteArrayOutputStream();
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(baos));
            Document doc = new Document(pdfDoc);
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, false);

            IDictionary<String, PdfFormField> fields = form.GetFormFields();
            fields["sunday_1"].SetValue("1");
            fields["sunday_2"].SetValue("2");
            fields["sunday_3"].SetValue("3");
            fields["sunday_4"].SetValue("4");
            fields["sunday_5"].SetValue("5");
            fields["sunday_6"].SetValue("6");

            // Add the fields, identified by name, to the list of fields to be flattened
            form.PartialFormFlattening("sunday_1");
            form.PartialFormFlattening("sunday_2");
            form.PartialFormFlattening("sunday_3");
            form.PartialFormFlattening("sunday_4");
            form.PartialFormFlattening("sunday_5");
            form.PartialFormFlattening("sunday_6");

            // Only the included above fields are flattened.
            // If no fields have been explicitly included, then all fields are flattened.
            form.FlattenFields();

            doc.Close();

            return baos.ToArray();
        }

        public virtual void CreateResultantPdf(String dest, byte[] src)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(new RandomAccessSourceFactory().CreateSource(src),
                new ReaderProperties()), new PdfWriter(dest));
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);

            IDictionary<String, PdfFormField> fields = form.GetFormFields();
            fields["sunday_1_notes"].SetValue("It's Sunday today, let's go to the sea").SetBorderWidth(0);
            fields["sunday_2_notes"].SetValue("It's Sunday today, let's go to the park").SetBorderWidth(0);
            fields["sunday_3_notes"].SetValue("It's Sunday today, let's go to the beach").SetBorderWidth(0);
            fields["sunday_4_notes"].SetValue("It's Sunday today, let's go to the woods").SetBorderWidth(0);
            fields["sunday_5_notes"].SetValue("It's Sunday today, let's go to the lake").SetBorderWidth(0);
            fields["sunday_6_notes"].SetValue("It's Sunday today, let's go to the river").SetBorderWidth(0);

            // All fields will be flattened, because no fields have been explicitly included
            form.FlattenFields();
            
            pdfDoc.Close();
        }
    }
}