using iText.Samples.Util;
using System;
using System.IO;
using iText.Commons.Utils;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Licensing.Base;

namespace iText.Samples.Htmlsamples.Chapter01
{
    /// <summary>
    /// Converts a simple HTML file to PDF using an InputStream and a PdfDocument
    /// as arguments for the convertToPdf() method.
    /// </summary>
    public class C01E06_HelloWorld
    {
        /// <summary>
        /// The path to the resulting PDF file.
        /// </summary>
        /// <returns></returns>
        public static readonly String DEST = "results/htmlsamples/ch01/helloWorld06.pdf";

        /// <summary>
        /// The Base URI of the HTML page.
        /// </summary>
        public static readonly String BASEURI = "../../../resources/htmlsamples/html/";

        /// <summary>
        /// The path to the source HTML file.
        /// </summary>
        public static readonly String SRC = String.Format("{0}hello.html", BASEURI);

        /// <summary>
        /// The main method of this example.
        /// </summary>
        /// <param name="args">no arguments are needed to run this example.</param>
        public static void Main(String[] args)
        {
            String licensePath = LicenseUtil.GetPathToLicenseFileWithITextCoreAndPdfHtmlAndPdfCalligraphProducts();
            using (Stream license = FileUtil.GetInputStreamForFile(licensePath))
            {
                LicenseKey.LoadLicenseFile(license);
            }
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new C01E06_HelloWorld().CreatePdf(BASEURI, SRC, DEST);
        }

        /// <summary>
        /// Creates the PDF file.
        /// </summary>
        /// <param name="baseUri">the base URI</param>
        /// <param name="src">the path to the source HTML file</param>
        /// <param name="dest">the path to the resulting PDF</param>
        public void CreatePdf(String baseUri, String src, String dest)
        {
            ConverterProperties properties = new ConverterProperties();
            properties.SetBaseUri(baseUri);
            PdfWriter writer = new PdfWriter(dest);
            PdfDocument pdf = new PdfDocument(writer);
            pdf.SetTagged();
            HtmlConverter.ConvertToPdf(new FileStream(src, FileMode.Open, FileAccess.Read), pdf, properties);
        }
    }
}