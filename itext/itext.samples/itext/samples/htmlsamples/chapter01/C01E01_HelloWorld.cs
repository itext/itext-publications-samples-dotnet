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
using iText.License;

namespace iText.Samples.Htmlsamples.Chapter01
{
    /// <summary>
    /// Converts a simple Hello World HTML String to a PDF document.
    /// </summary>
    public class C01E01_HelloWorld
    {
        /// <summary>
        /// The path to the resulting PDF file.
        /// </summary>
        public static readonly String DEST = "results/htmlsamples/ch01/helloWorld01.pdf";

        /// <summary>
        /// The HTML-string that we are going to convert to PDF.
        /// </summary>
        public static readonly String HTML = "<h1>Test</h1><p>Hello World</p>";

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

            new C01E01_HelloWorld().CreatePdf(HTML, DEST);
        }

        /// <summary>
        /// Creates the PDF file.
        /// </summary>
        /// <param name="html">the HTML as a String value</param>
        /// <param name="dest">the path of the resulting PDF</param>
        public void CreatePdf(String html, String dest)
        {
            HtmlConverter.ConvertToPdf(html, new FileStream(dest, FileMode.Create));
        }
    }
}