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
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout.Font;
using iText.License;

namespace iText.Samples.Htmlsamples.Chapter07
{
    public class C07E13_Peace
    {
        /// <summary>
        /// The path to a folder containing extra fonts.
        /// </summary>
        public static readonly String FONTS = "../../resources/htmlsamples/fonts/noto/";

        /// <summary>
        /// The path to the resulting PDF file.
        /// </summary>
        public static readonly String DEST = "results/htmlsamples/ch07/peace.pdf";

        /// <summary>
        /// The path to the source HTML file.
        /// </summary>
        public static readonly String SRC = "../../resources/htmlsamples/html/peace.html";

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

            C07E13_Peace app = new C07E13_Peace();
            app.CreatePdf(SRC, FONTS, DEST);
        }

        /// <summary>
        /// Creates the PDF file.
        /// </summary>
        /// <param name="src">the path to the source HTML file</param>
        /// <param name="fonts">the path to a folder containing a series of fonts</param>
        /// <param name="dest">the path to the resulting PDF</param>
        public void CreatePdf(String src, String fonts, String dest)
        {
            PdfWriter writer = new PdfWriter(dest);
            PdfDocument pdf = new PdfDocument(writer);
            pdf.SetDefaultPageSize(PageSize.A4.Rotate());
            ConverterProperties properties = new ConverterProperties();
            FontProvider fontProvider = new DefaultFontProvider(false, false, false);
            fontProvider.AddDirectory(fonts);
            properties.SetFontProvider(fontProvider);
            HtmlConverter.ConvertToPdf(new FileStream(src, FileMode.Open, FileAccess.Read), pdf, properties);
        }
    }
}