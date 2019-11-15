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
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Acroforms
{
    public class FormatFields
    {
        public static readonly String DEST = "../../results/sandbox/acroforms/format_fields.pdf";

        public static readonly String SRC = "../../resources/pdfs/form.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new FormatFields().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);

            // The second parameter sets how the field's value will be displayed in the resultant pdf.
            // If the second parameter is null, then actual value will be shown.
            form.GetField("Name").SetValue("1.0", "100%");
            form.GetField("Company").SetValue("1217000.000000", "$1,217,000");

            pdfDoc.Close();
        }
    }
}