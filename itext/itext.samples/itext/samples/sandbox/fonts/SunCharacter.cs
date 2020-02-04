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
    public class SunCharacter
    {
        public static readonly String DEST = "results/sandbox/fonts/sun_character.pdf";

        public static readonly String FONT = "../../../resources/font/Cardo-Regular.ttf";

        // "The Cardo family of fonts supports this character: ☉"
        public static readonly String TEXT = "The Cardo family of fonts supports this character: \u2609";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new SunCharacter().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            PdfFont font = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H, true);
            Paragraph p = new Paragraph(TEXT).SetFont(font);
            doc.Add(p);

            doc.Close();
        }
    }
}