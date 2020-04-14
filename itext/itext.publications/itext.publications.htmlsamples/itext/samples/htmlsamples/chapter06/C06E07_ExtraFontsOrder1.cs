/*
    This file is part of the iText (R) project.
    Copyright (c) 1998-2020 iText Group NV
    Authors: iText Software.

    For more information, please contact iText Software at this address:
    sales@itextpdf.com
 */
/*
 * This example was written in the context of the following book:
 * https://leanpub.com/itext7_pdfHTML
 * Go to http://developers.itextpdf.com for more info.
 */

using System;
using System.IO;
using iText.Html2pdf;
using iText.Html2pdf.Resolver.Font;
using iText.IO.Font;
using iText.Layout.Font;
using iText.License;

namespace iText.Samples.Htmlsamples.Chapter06
{
    public class C06E07_ExtraFontsOrder1
    {
        /// <summary>
        /// An array with the paths to extra fonts.
        /// </summary>
        public static readonly String[] FONTS =
        {
            "../../../resources/htmlsamples/fonts/noto/NotoSans-Regular.ttf",
            "../../../resources/htmlsamples/fonts/cardo/Cardo-Regular.ttf"
        };

        /// <summary>
        /// The path to the resulting PDF file.
        /// </summary>
        public static readonly String DEST = "results/htmlsamples/ch06/fonts_noto.pdf";

        /// <summary>
        /// The path to the source HTML file.
        /// </summary>
        public static readonly String SRC = "../../../resources/htmlsamples/html/hello.html";

        /// <summary>
        /// The main method of this example.
        /// </summary>
        /// <param name="args">no arguments are needed to run this example.</param>
        public static void Main(String[] args)
        {
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") +
                                       "/itextkey-html2pdf_typography.xml");
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            C06E07_ExtraFontsOrder1 app = new C06E07_ExtraFontsOrder1();
            app.CreatePdf(SRC, FONTS, DEST);
        }

        /// <summary>
        /// Creates the PDF file.
        /// </summary>
        /// <param name="src">the path to the source HTML file</param>
        /// <param name="fonts">an array containing paths to extra fonts</param>
        /// <param name="dest">the path to the resulting PDF</param>
        public void CreatePdf(String src, String[] fonts, String dest)
        {
            ConverterProperties properties = new ConverterProperties();
            FontProvider fontProvider = new DefaultFontProvider(false, false, false);
            foreach (String font in fonts)
            {
                FontProgram fontProgram = FontProgramFactory.CreateFont(font);
                fontProvider.AddFont(fontProgram);
            }

            properties.SetFontProvider(fontProvider);
            HtmlConverter.ConvertToPdf(new FileInfo(src), new FileInfo(dest), properties);
        }
    }
}