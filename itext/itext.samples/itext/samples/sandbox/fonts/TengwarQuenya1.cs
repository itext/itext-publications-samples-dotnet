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
    public class TengwarQuenya1
    {
        public static readonly String DEST = "results/sandbox/fonts/tengwar_quenya1.pdf";

        public static readonly String FONT = "../../../resources/font/Greifswalder Tengwar.ttf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new TengwarQuenya1().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            PdfFont font = PdfFontFactory.CreateFont(FONT, PdfEncodings.WINANSI, true);
            Paragraph p = new Paragraph("A Hello World PDF document.").SetFont(font);
            doc.Add(p);

            doc.Close();
        }
    }
}