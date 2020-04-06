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
    public class FreeSansBold
    {
        public static readonly String DEST = "results/sandbox/fonts/free_sans_bold.pdf";

        public static readonly String FONT = "../../../resources/font/FreeSans.ttf";
        public static readonly String FONTBOLD = "../../../resources/font/FreeSansBold.ttf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new FreeSansBold().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            PdfFont font = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H);

            // "FreeSans regular: Đ"
            Paragraph p = new Paragraph("FreeSans regular: \u0110").SetFont(font);
            doc.Add(p);

            PdfFont bold = PdfFontFactory.CreateFont(FONTBOLD, PdfEncodings.IDENTITY_H);

            // "FreeSans bold: Đ"
            p = new Paragraph("FreeSans bold: \u0110").SetFont(bold);
            doc.Add(p);

            doc.Close();
        }
    }
}