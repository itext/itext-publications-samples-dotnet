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

namespace iText.Samples.Sandbox.Acroforms
{
    public class ChangeButton
    {
        public static readonly String DEST = "results/sandbox/acroforms/change_button.pdf";

        public static readonly String SRC = "../../resources/pdfs/hello_button.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ChangeButton().ManipulatePdf(DEST);
        }
        
        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
            PdfFormField button = form.CopyField("Test");
            PdfArray rect = button.GetWidgets()[0].GetRectangle();

            // Increase value of the right coordinate (index 2 corresponds with right coordinate)
            rect.Set(2, new PdfNumber(rect.GetAsNumber(2).FloatValue() + 172));

            button.SetValue("Print Amended");
            form.ReplaceField("Test", button);

            pdfDoc.Close();
        }
    }
}