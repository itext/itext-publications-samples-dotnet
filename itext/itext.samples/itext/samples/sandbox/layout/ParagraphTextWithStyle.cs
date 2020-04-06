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
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Layout
{
    public class ParagraphTextWithStyle
    {
        public static readonly String DEST = "results/sandbox/layout/paragraphTextWithStyle.pdf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ParagraphTextWithStyle().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            PdfFont code = PdfFontFactory.CreateFont(StandardFonts.COURIER);

            Style style = new Style()
                .SetFont(code)
                .SetFontSize(14)
                .SetFontColor(ColorConstants.RED)
                .SetBackgroundColor(ColorConstants.LIGHT_GRAY);

            Paragraph paragraph = new Paragraph()
                .Add("In this example, named ")
                .Add(new Text("HelloWorldStyles").AddStyle(style))
                .Add(", we experiment with some text in ")
                .Add(new Text("code style").AddStyle(style))
                .Add(".");

            using (Document document = new Document(pdf))
            {
                document.Add(paragraph);
            }
        }
    }
}