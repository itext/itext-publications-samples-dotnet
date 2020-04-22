using System;
using System.IO;
using iText.Html2pdf;
using iText.License;

namespace iText.Samples.Htmlsamples.Chapter01
{
    /// <summary>
    /// Converts a Hello World HTML String with a reference to an external image to a PDF document.
    /// </summary>
    public class C01E02_HelloWorld
    {
        /// <summary>
        /// The path to the resulting PDF file.
        /// </summary>
        public static readonly String DEST = "results/htmlsamples/ch01/helloWorld02.pdf";

        /// <summary>
        /// The Base URI of the HTML page.
        /// </summary>
        public static readonly String BASEURI = "../../../resources/htmlsamples/html";

        /// <summary>
        /// The HTML-string that we are going to convert to PDF.
        /// </summary>
        public static readonly String HTML = "<h1>Test</h1><p>Hello World</p><img src=\"img/logo.png\">";

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

            new C01E02_HelloWorld().CreatePdf(BASEURI, HTML, DEST);
        }

        /// <summary>
        /// Creates the PDF file.
        /// </summary>
        /// <param name="baseUri">the base URI</param>
        /// <param name="html">the HTML as a String</param>
        /// <param name="dest">the path to the resulting PDF</param>
        public void CreatePdf(String baseUri, String html, String dest)
        {
            ConverterProperties properties = new ConverterProperties();
            properties.SetBaseUri(baseUri);
            HtmlConverter.ConvertToPdf(html, new FileStream(dest, FileMode.Create), properties);
        }
    }
}