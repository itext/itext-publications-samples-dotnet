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
using iText.Forms.Xfa;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Acroforms
{
    public class FillXFA2
    {
        public static readonly String DEST = "results/sandbox/acroforms/xfa_example_filled.pdf";

        public static readonly String SRC = "../../resources/pdfs/xfa_invoice_example.pdf";
        public static readonly String XML = "../../resources/xml/xfa_example.xml";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new FillXFA2().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(string dest)
        {
            PdfReader reader = new PdfReader(SRC);
            PdfDocument pdfDoc = new PdfDocument(reader, new PdfWriter(dest));
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);

            XfaForm xfa = form.GetXfaForm();

            // Method fills this object with XFA data under datasets/data.
            xfa.FillXfaForm(new FileStream(XML, FileMode.Open, FileAccess.Read));
            xfa.Write(pdfDoc);

            pdfDoc.Close();
        }
    }
}