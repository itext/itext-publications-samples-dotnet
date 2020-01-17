/*
This file is part of the iText (R) project.
Copyright (c) 1998-2020 iText Group NV
Authors: iText Software.

For more information, please contact iText Software at this address:
sales@itextpdf.com
*/

using System;
using System.IO;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Stamper 
{
    public class StampHeader2 
    {
        public static readonly String DEST = "results/sandbox/stamper/stamp_header2.pdf";
        public static readonly String SRC = "../../resources/pdfs/Wrong.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new StampHeader2().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            
            Paragraph header = new Paragraph("Copy")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                    .SetFontSize(14);
            
            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++) 
            {
                Rectangle pageSize = pdfDoc.GetPage(i).GetPageSize();
                float x = pageSize.GetWidth() / 2;
                float y = pageSize.GetTop() - 20;
                doc.ShowTextAligned(header, x, y, i, TextAlignment.CENTER, VerticalAlignment.BOTTOM, 0);
            }
            
            doc.Close();
        }
    }
}
