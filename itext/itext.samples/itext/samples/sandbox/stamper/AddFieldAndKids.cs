/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Stamper 
{
    public class AddFieldAndKids {
        public static readonly String DEST = "results/sandbox/stamper/add_field_and_kids.pdf";
        public static readonly String SRC = "../../resources/pdfs/hello.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new AddFieldAndKids().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            
            PdfFormField personal = PdfFormField.CreateEmptyField(pdfDoc);
            personal.SetFieldName("personal");
            PdfTextFormField name = PdfFormField.CreateText(pdfDoc, 
                    new Rectangle(36, 760, 108, 30), "name", "");
            personal.AddKid(name);
            PdfTextFormField password = PdfFormField.CreateText(pdfDoc,
                    new Rectangle(150, 760, 300, 30), "password", "");
            personal.AddKid(password);
            PdfAcroForm.GetAcroForm(pdfDoc, true).AddField(personal, pdfDoc.GetFirstPage());
            
            pdfDoc.Close();
        }
    }
}
