/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Stamper 
{
    public class ChangeVersion 
    {
        public static readonly String DEST = "results/sandbox/stamper/change_version.pdf";
        public static readonly String SRC = "../../resources/pdfs/OCR.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new ChangeVersion().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), 
                    
                        // Please note that the default PdfVersion value is PDF 1.7
                        new PdfWriter(dest, new WriterProperties().SetPdfVersion(PdfVersion.PDF_1_5)));
            
            pdfDoc.Close();
        }
    }
}
