/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Stamper 
{
    public class ScaleRotate 
    {
        public static readonly String DEST = "results/sandbox/stamper/scale_rotate.pdf";
        public static readonly String SRC = "../../../resources/pdfs/pages.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new ScaleRotate().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            
            for (int p = 1; p <= pdfDoc.GetNumberOfPages(); p++) 
            {
                PdfDictionary page = pdfDoc.GetPage(p).GetPdfObject();
                if (page.GetAsNumber(PdfName.UserUnit) == null) 
                {
                    page.Put(PdfName.UserUnit, new PdfNumber(2.5f));
                }
                page.Remove(PdfName.Rotate);
            }
            
            pdfDoc.Close();
        }
    }
}
