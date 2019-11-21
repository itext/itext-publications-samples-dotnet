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
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Acroforms
{
    public class FillFormSpecialChars2
    {
        public static readonly String DEST = "results/sandbox/acroforms/fill_form_special_chars2.pdf";

        public static readonly String FONT = "../../resources/font/FreeSans.ttf";
        public static readonly String SRC = "../../resources/pdfs/form.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new FillFormSpecialChars2().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);

            // This method tells to generate an appearance Stream while flattening for all form fields that don't have one.
            // Generating appearances will slow down form flattening,
            // but otherwise the results can be unexpected in Acrobat.
            form.SetGenerateAppearance(true);

            PdfFont font = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H);

            // ӧ character is used here
            form.GetField("Name").SetValue("\u04e711111", font, 12f);

            // If no fields have been explicitly included, then all fields are flattened.
            // Otherwise only the included fields are flattened.
            form.FlattenFields();

            pdfDoc.Close();
        }
    }
}