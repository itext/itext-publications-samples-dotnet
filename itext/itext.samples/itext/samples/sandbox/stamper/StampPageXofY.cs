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
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Stamper 
{
    public class StampPageXofY 
    {
        public static readonly String DEST = "results/sandbox/stamper/stamp_page_x_of_y.pdf";
        public static readonly String SRC = "../../../resources/pdfs/nameddestinations.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new StampPageXofY().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            
            int numberOfPages = pdfDoc.GetNumberOfPages();
            for (int i = 1; i <= numberOfPages; i++) 
            {
                // Write aligned text to the specified by parameters point
                doc.ShowTextAligned(new Paragraph("page " + i + " of " + numberOfPages),
                        559, 806, i, TextAlignment.RIGHT, VerticalAlignment.TOP, 0);
            }
            
            doc.Close();
        }
    }
}
