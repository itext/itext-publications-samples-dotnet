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
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Fonts
{
    public class TickboxCharacter
    {
        public static readonly String DEST = "results/sandbox/fonts/tickbox_character.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new TickboxCharacter().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            Paragraph p = new Paragraph("This is a tick box character: ");

            Text text = new Text("o");
            PdfFont zapfdingbats = PdfFontFactory.CreateFont(StandardFonts.ZAPFDINGBATS);
            text.SetFont(zapfdingbats);
            text.SetFontSize(14);
            p.Add(text);

            doc.Add(p);

            doc.Close();
        }
    }
}