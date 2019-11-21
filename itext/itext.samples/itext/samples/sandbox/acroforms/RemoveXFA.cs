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
using iText.Kernel.Colors;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Acroforms
{
    public class RemoveXFA
    {
        public static readonly String DEST = "results/sandbox/acroforms/remove_xfa.pdf";

        public static readonly String SRC = "../../resources/pdfs/reportcardinitial.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new RemoveXFA().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);

            // Method removes the XFA stream from the document.
            form.RemoveXfaForm();

            IDictionary<String, PdfFormField> fields = form.GetFormFields();
            foreach (KeyValuePair<String, PdfFormField> name in fields)
            {
                if (name.Key.IndexOf("Total") > 0)
                {
                    name.Value.SetColor(ColorConstants.RED);
                }

                name.Value.SetValue("X");
            }

            pdfDoc.Close();
        }
    }
}