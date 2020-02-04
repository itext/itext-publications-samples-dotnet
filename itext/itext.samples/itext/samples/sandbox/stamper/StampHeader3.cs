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
    public class StampHeader3 
    {
        public static readonly String DEST = "results/sandbox/stamper/stamp_header3.pdf";
        public static readonly String SRC = "../../../resources/pdfs/Wrong.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new StampHeader3().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest) 
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(SRC), new PdfWriter(dest));
            Document doc = new Document(pdfDoc);
            Paragraph header = new Paragraph("Copy")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA)).SetFontSize(6);
            
            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++) 
            {
                PdfPage pdfPage = pdfDoc.GetPage(i);
                
                // When "true": in case the page has a rotation, then new content will be automatically rotated in the
                // opposite direction. On the rotated page this would look as if new content ignores page rotation.
                pdfPage.SetIgnorePageRotationForContent(true);
                
                Rectangle pageSize = pdfPage.GetPageSize();
                float x;
                float y;
                if (pdfPage.GetRotation() % 180 == 0) 
                {
                    x = pageSize.GetWidth() / 2;
                    y = pageSize.GetTop() - 20;
                }
                else 
                {
                    x = pageSize.GetHeight() / 2;
                    y = pageSize.GetRight() - 20;
                }
                
                doc.ShowTextAligned(header, x, y, i, TextAlignment.CENTER, VerticalAlignment.BOTTOM, 0);
            }
            
            doc.Close();
        }
    }
}
