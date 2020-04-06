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
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;

namespace iText.Samples.Sandbox.Stamper 
{
    public class SuperImpose 
    {
        public static readonly String DEST = "results/sandbox/stamper/super_impose.pdf";
        public static readonly String SRC = "../../../resources/pdfs/primes.pdf";
        public static readonly String[] EXTRA =
        {       "../../../resources/pdfs/hello.pdf", 
                "../../../resources/pdfs/base_url.pdf", 
                "../../../resources/pdfs/state.pdf"
                
        };
        
        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new SuperImpose().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            PdfCanvas canvas = new PdfCanvas(pdfDoc.GetFirstPage().NewContentStreamBefore(), 
                    pdfDoc.GetFirstPage().GetResources(), pdfDoc);
            
            foreach (String path in EXTRA) 
            {
                PdfDocument srcDoc = new PdfDocument(new PdfReader(path));
                PdfFormXObject page = srcDoc.GetFirstPage().CopyAsFormXObject(pdfDoc);
                canvas.AddXObject(page, 0, 0);
                srcDoc.Close();
            }
            
            pdfDoc.Close();
        }
    }
}
