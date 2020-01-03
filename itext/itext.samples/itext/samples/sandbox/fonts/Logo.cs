/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2020 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;

namespace iText.Samples.Sandbox.Fonts
{
    public class Logo
    {
        public static readonly String DEST = "results/sandbox/fonts/logo.pdf";

        public static readonly String FONT = "./src/test/resources/font/FreeSans.ttf";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new Logo().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            /* Create a custom type 3 font.
             * The second argument defines whether the glyph color is specified
             * in the glyph description in the font.
             */
            PdfType3Font t3 = PdfFontFactory.CreateType3Font(pdfDoc, true);
            float linewidth = 125;

            /* Define I letter of the custom font.
             * If the character was already defined, addGlyph() method will return the same content.
             * If the colorized parameter of PdfType3Font instance is set to true,
             * then parameters of the glyph bounding box will be ignored.
             */
            PdfCanvas i = t3.AddGlyph('I', 700, 0, 0, 1200, 600);
            i.SetLineWidth(10);
            i.SetStrokeColor(new DeviceRgb(0xf9, 0x9d, 0x25));
            i.SetLineWidth(linewidth);
            i.SetLineCapStyle(PdfCanvasConstants.LineCapStyle.ROUND);
            i.MoveTo(600, 36);
            i.LineTo(600, 564);
            i.Stroke();

            // Define T letter of the custom font
            PdfCanvas t = t3.AddGlyph('T', 1170, 0, 0, 1200, 600);
            t.SetLineWidth(10);
            t.SetStrokeColor(new DeviceRgb(0x08, 0x49, 0x75));
            t.SetLineWidth(linewidth);
            t.SetLineCapStyle(PdfCanvasConstants.LineCapStyle.ROUND);
            t.MoveTo(144, 564);
            t.LineTo(1056, 564);
            t.MoveTo(600, 36);
            t.LineTo(600, 564);
            t.Stroke();

            // Define E letter of the custom font
            PdfCanvas e = t3.AddGlyph('E', 1150, 0, 0, 1200, 600);
            e.SetLineWidth(10);
            e.SetStrokeColor(new DeviceRgb(0xf8, 0x9b, 0x22));
            e.SetLineWidth(linewidth);
            e.SetLineCapStyle(PdfCanvasConstants.LineCapStyle.ROUND);
            e.MoveTo(144, 36);
            e.LineTo(1056, 36);
            e.MoveTo(144, 300);
            e.LineTo(1056, 300);
            e.MoveTo(144, 564);
            e.LineTo(1056, 564);
            e.Stroke();

            // Define X letter of the custom font
            PdfCanvas x = t3.AddGlyph('X', 1160, 0, 0, 1200, 600);
            x.SetStrokeColor(new DeviceRgb(0x10, 0x46, 0x75));
            x.SetLineWidth(10);
            x.SetLineWidth(linewidth);
            x.SetLineCapStyle(PdfCanvasConstants.LineCapStyle.ROUND);
            x.MoveTo(144, 36);
            x.LineTo(1056, 564);
            x.MoveTo(144, 564);
            x.LineTo(1056, 36);
            x.Stroke();

            Paragraph p = new Paragraph("ITEXT")
                .SetFont(t3)
                .SetFontSize(20);
            doc.Add(p);

            p = new Paragraph("I\nT\nE\nX\nT")
                .SetFixedLeading(20)
                .SetFont(t3)
                .SetFontSize(20);
            doc.Add(p);

            doc.Close();
        }
    }
}