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
using System.Net;
using iText.Html2pdf;
using iText.License;
using iText.StyledXmlParser.Css.Media;

namespace iText.Samples.Htmlsamples.Chapter07
{
    /// <summary>
    /// Converts a simple HTML file to PDF using an InputStream and an OutputStream
    /// as arguments for the convertToPdf() method.
    /// </summary>
    public class C07E06_CreateFromURL3
    {
        /// <summary>
        /// The path to the resulting PDF file.
        /// </summary>
        public static readonly String DEST = "results/htmlsamples/ch07/url2pdf_3.pdf";

        /// <summary>
        /// The target folder for the result.
        /// </summary>
        public static readonly String ADDRESS = "https://stackoverflow.com/help/on-topic";

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

            new C07E06_CreateFromURL3().CreatePdf(new Uri(ADDRESS), DEST);
        }

        /// <summary>
        /// Creates the PDF file.
        /// </summary>
        /// <param name="url">the URL object for the web page</param>
        /// <param name="dest">the path to the resulting PDF</param>
        public void CreatePdf(Uri url, String dest)
        {
            ConverterProperties properties = new ConverterProperties();
            MediaDeviceDescription mediaDeviceDescription = new MediaDeviceDescription(MediaType.PRINT);
            properties.SetMediaDeviceDescription(mediaDeviceDescription);
            var httpWebRequest = (HttpWebRequest) WebRequest.Create(url);
            var httpResponse = (HttpWebResponse) httpWebRequest.GetResponse();
            HtmlConverter.ConvertToPdf(httpResponse.GetResponseStream(),
                new FileStream(dest, FileMode.Create), properties);
        }
    }
}