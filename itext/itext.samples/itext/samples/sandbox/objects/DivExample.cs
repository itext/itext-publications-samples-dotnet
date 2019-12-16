/*
This file is part of the iText (R) project.
Copyright (c) 1998-2019 iText Group NV
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
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Objects
{
    public class DivExample
    {
        public readonly static String DEST = "results/sandbox/objects/divExample.pdf";

        public readonly static String TEXT1 = "Test document which can be altered and ignored. "
                                              + "This text should come above the rectangle.";

        public readonly static String TEXT2 = "Test document which can be altered and ignored. "
                                              + "Some text in the Div. For more information, please visit https://itextpdf.com/";

        public readonly static String TEXT3 = "This text should come below the rectangle "
                                              + "and thereafter a normal flow should happen ";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new DivExample().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDocument);

            Paragraph para = new Paragraph(TEXT1);
            doc.Add(para);
            doc.Add(para);

            Paragraph divHeader = new Paragraph("Notice:")
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD));
            Paragraph divText = new Paragraph(TEXT2)
                .SetFontSize(11);
            Div div = new Div()
                .Add(divHeader)
                .Add(divText)
                .SetWidth(400)
                .SetPadding(3f)
                .SetHorizontalAlignment(HorizontalAlignment.CENTER)
                .SetBorder(new SolidBorder(0.5f));
            doc.Add(div);

            para = new Paragraph(TEXT3);
            doc.Add(para);

            pdfDocument.Close();
        }
    }
}