/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;

namespace iText.Samples.Sandbox.Acroforms
{
    public class ChangeFieldSize
    {
        public static readonly String DEST = "../../results/sandbox/acroforms/change_field_size.pdf";

        public static readonly String SRC = "../../resources/pdfs/form.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ChangeFieldSize().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
            PdfFormField field = form.GetField("Name");
            PdfWidgetAnnotation widgetAnnotation = field.GetWidgets()[0];
            PdfArray annotationRect = widgetAnnotation.GetRectangle();

            // Change value of the right coordinate (index 2 corresponds with right coordinate)
            annotationRect.Set(2, new PdfNumber(annotationRect.GetAsNumber(2).FloatValue() + 20f));

            String value = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" +
                           "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA";
            field.SetValue(value);
            form.GetField("Company").SetValue(value);

            pdfDoc.Close();
        }
    }
}