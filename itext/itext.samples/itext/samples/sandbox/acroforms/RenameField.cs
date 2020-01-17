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
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Acroforms
{
    public class RenameField
    {
        public static readonly String DEST = "results/sandbox/acroforms/rename_field.pdf";

        public static readonly String SRC = "../../resources/pdfs/subscribe.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new RenameField().ManipulatePdf(DEST);
        }

        public void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);

            form.RenameField("personal.loginname", "login");

            pdfDoc.Close();

            pdfDoc = new PdfDocument(new PdfReader(dest));
            form = PdfAcroForm.GetAcroForm(pdfDoc, true);
            IDictionary<String, PdfFormField> fields = form.GetFormFields();

            // See the renamed field in the console
            foreach (String name in fields.Keys)
            {
                Console.WriteLine(name);
            }

            pdfDoc.Close();
        }
    }
}