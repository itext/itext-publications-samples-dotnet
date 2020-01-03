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
using iText.Kernel.XMP;

namespace iText.Samples.Sandbox.Stamper 
{
    public class AddXmpToPage 
    {
        public static readonly String DEST = "results/sandbox/stamper/add_xmp_to_page.pdf";
        public static readonly String SRC = "../../resources/pdfs/hello.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new AddXmpToPage().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            
            PdfPage page = pdfDoc.GetFirstPage();
            page.SetXmpMetadata(XMPMetaFactory.Create());
            
            pdfDoc.Close();
        }
    }
}
