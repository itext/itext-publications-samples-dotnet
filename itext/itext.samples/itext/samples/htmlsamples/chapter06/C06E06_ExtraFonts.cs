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
using iText.Layout.Font;
using iText.License;

namespace iText.Samples.Htmlsamples.Chapter06
{
    public class C06E06_ExtraFonts
    {
        /// <summary>
        /// The path to the resulting PDF file.
        /// </summary>
        public static readonly String DEST = "results/htmlsamples/ch06/fonts_extrafonts.pdf";

        /// <summary>
        /// The path to the source HTML file.
        /// </summary>
        public static readonly String SRC = "../../resources/htmlsamples/html/fonts_extra.html";

        /// <summary>
        /// The path to a folder containing extra fonts.
        /// </summary>
        public static readonly String FONTS = "../../resources/htmlsamples/fonts/cardo/";

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

            C06E06_ExtraFonts app = new C06E06_ExtraFonts();
            app.CreatePdf(SRC, FONTS, DEST);
        }

        /// <summary>
        /// Creates the PDF file.
        /// </summary>
        /// <param name="src">the path to the source HTML file</param>
        /// <param name="fonts">the path to a font folder with extra fonts</param>
        /// <param name="dest">the path to the resulting PDF</param>
        public void CreatePdf(String src, String fonts, String dest)
        {
            ConverterProperties properties = new ConverterProperties();
            FontProvider fontProvider = new DefaultFontProvider();
            fontProvider.AddDirectory(fonts);
            properties.SetFontProvider(fontProvider);
            HtmlConverter.ConvertToPdf(new FileInfo(src), new FileInfo(dest), properties);
        }
    }
}