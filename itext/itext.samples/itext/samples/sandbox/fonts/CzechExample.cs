/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2020 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.IO;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Fonts
{
    public class CzechExample
    {
        public static readonly String DEST = "results/sandbox/fonts/czech_example.pdf";
        public static readonly String FONT = "../../resources/font/FreeSans.ttf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new CzechExample().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            PdfFont f1 = PdfFontFactory.CreateFont(FONT, PdfEncodings.CP1250, true);

            // "Č,Ć,Š,Ž,Đ"
            Paragraph p1 = new Paragraph("Testing of letters \u010c,\u0106,\u0160,\u017d,\u0110").SetFont(f1);
            doc.Add(p1);

            PdfFont f2 = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H, true);

            // "Č,Ć,Š,Ž,Đ"
            Paragraph p2 = new Paragraph("Testing of letters \u010c,\u0106,\u0160,\u017d,\u0110").SetFont(f2);
            doc.Add(p2);

            doc.Close();
        }
    }
}