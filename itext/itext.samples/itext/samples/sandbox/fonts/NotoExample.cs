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
    public class NotoExample
    {
        public static readonly String DEST = "results/sandbox/fonts/chinese.pdf";

        public static readonly String FONT = "../../resources/font/NotoSansCJKsc-Regular.otf";

        /*
         * "These are the protagonists in 'Hero', a movie by Zhang Yimou:
         * 無名 (Nameless), 殘劍 (Broken Sword), 飛雪 (Flying Snow), 如月 (Moon), 秦王 (the King), and 長空 (Sky)."
         */
        public static readonly String TEXT = "These are the protagonists in 'Hero', a movie by Zhang Yimou:\n"
                                             + "\u7121\u540d (Nameless), \u6b98\u528d (Broken Sword), "
                                             + "\u98db\u96ea (Flying Snow), \u5982\u6708 (Moon), "
                                             + "\u79e6\u738b (the King), and \u9577\u7a7a (Sky).";

        // "十锊埋伏"
        public static readonly String CHINESE = "\u5341\u950a\u57cb\u4f0f";

        // "誰も知らない"
        public static readonly String JAPANESE = "\u8ab0\u3082\u77e5\u3089\u306a\u3044";

        // "빈집"
        public static readonly String KOREAN = "\ube48\uc9d1";

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new NotoExample().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            PdfFont font = PdfFontFactory.CreateFont(FONT, PdfEncodings.IDENTITY_H);

            // Overwrite some default document font-related properties. From now on they will be used for all the elements
            // added to the document unless they are overwritten inside these elements
            doc.SetFont(font);

            doc.Add(new Paragraph(TEXT));
            doc.Add(new Paragraph(CHINESE));
            doc.Add(new Paragraph(JAPANESE));
            doc.Add(new Paragraph(KOREAN));

            doc.Close();
        }
    }
}