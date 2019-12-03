/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2019 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */

using System;
using System.Collections.Generic;
using System.IO;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace iText.Samples.Sandbox.Fonts
{
    public class FontTest
    {
        public static readonly String DEST = "results/sandbox/fonts/font_test.pdf";

        public static readonly String TEXT = "Quick brown fox jumps over the lazy dog; 0123456789";

        // "Nikogaršnja zemlja"
        public static readonly String CP1250 = "Nikogar\u0161nja zemlja";

        // "Я люблю тебя"
        public static readonly String CP1251 = "\u042f \u043b\u044e\u0431\u043b\u044e \u0442\u0435\u0431\u044f";

        // "Un long dimanche de fiançailles"
        public static readonly String CP1252 = "Un long dimanche de fian\u00e7ailles";

        // "Νύφες"
        public static readonly String CP1253 = "\u039d\u03cd\u03c6\u03b5\u03c2";

        // "十锊埋伏"
        public static readonly String CHINESE = "\u5341\u950a\u57cb\u4f0f";

        // "誰も知らない"
        public static readonly String JAPANESE = "\u8ab0\u3082\u77e5\u3089\u306a\u3044";

        // "빈집"
        public static readonly String KOREAN = "\ube48\uc9d1";

        public static readonly String[] FONTS =
        {
            "../../resources/font/cmr10.afm",
            "../../resources/font/cmr10.pfb",
            "../../resources/font/cmr10.pfm",
            "../../resources/font/EBGaramond12-Italic.ttf",
            "../../resources/font/EBGaramond12-Regular.ttf",
            "../../resources/font/FreeSans.ttf",
            "../../resources/font/FreeSansBold.ttf",
            "../../resources/font/NotoSans-Bold.ttf",
            "../../resources/font/NotoSans-BoldItalic.ttf",
            "../../resources/font/NotoSansCJKjp-Regular.otf",
            "../../resources/font/NotoSansCJKkr-Regular.otf",
            "../../resources/font/NotoSansCJKsc-Regular.otf"
        };

        public static void Main(String[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new FontTest().ManipulatePdf(DEST);
        }

        protected void ManipulatePdf(String dest)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(dest));
            Document doc = new Document(pdfDoc);

            foreach (String font in FONTS)
            {
                PdfFontFactory.Register(font);
            }

            PdfFont defaultFont = doc.GetDefaultProperty<PdfFont>(Property.FONT);
            
            HashSet<String> fonts = new HashSet<String>(FontProgramFactory.GetRegisteredFonts());
            foreach (String fontname in fonts)
            {
                Console.WriteLine(fontname);
                PdfFont font;
                try
                {
                    font = PdfFontFactory.CreateRegisteredFont(fontname, PdfEncodings.IDENTITY_H);
                }
                catch (ArgumentException e)
                {
                    doc.Add(new Paragraph(String.Format("The font {0} doesn't have unicode support: {1}",
                        fontname, e.Message)));
                    continue;
                }

                doc.Add(new Paragraph(String.Format("Postscript name for {0}: {1}", fontname,
                    font.GetFontProgram().GetFontNames().GetFontName())));
                doc.SetFont(font);
                ShowFontInfo(doc);
                
                // Restore the default document font
                doc.SetFont(defaultFont);
            }

            doc.Close();
        }

        private static void ShowFontInfo(Document doc)
        {
            doc.Add(new Paragraph(TEXT));
            doc.Add(new Paragraph(String.Format("CP1250: {0}", CP1250)));
            doc.Add(new Paragraph(String.Format("CP1251: {0}", CP1251)));
            doc.Add(new Paragraph(String.Format("CP1252: {0}", CP1252)));
            doc.Add(new Paragraph(String.Format("CP1253: {0}", CP1253)));
            doc.Add(new Paragraph(String.Format("CHINESE: {0}", CHINESE)));
            doc.Add(new Paragraph(String.Format("JAPANESE: {0}", JAPANESE)));
            doc.Add(new Paragraph(String.Format("KOREAN: {0}", KOREAN)));
        }
    }
}