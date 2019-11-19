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
using iText.Kernel.Pdf.Navigation;

namespace iText.Samples.Sandbox.Stamper 
{
    public class AddOpenAction {
        public static readonly String SRC = "../../resources/pdfs/hello.pdf";
        public static readonly String DEST = "results/sandbox/stamper/add_open_action.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new AddOpenAction().ManipulatePdf(DEST);
        }

        protected internal virtual void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            PdfPage page1 = pdfDoc.GetPage(1);
            float page1Height = page1.GetPageSize().GetHeight();
            PdfDestination pdfDestination = PdfExplicitDestination.CreateXYZ(page1, 0, page1Height, 0.75f);
            pdfDoc.GetCatalog().SetOpenAction(pdfDestination);
            pdfDoc.Close();
        }
    }
}
