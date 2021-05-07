using System;
using System.Collections.Generic;
using System.IO;
using iText.Commons.Utils;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Licensing.Base;

namespace iText.Samples.Htmlsamples.Chapter01
{
    /// <summary>
    /// Converts a simple HTML file to PDF using the convertToElements() method.
    /// </summary>
    public class C01E08_HelloWorld
    {
        /// <summary>
        /// The path to the resulting PDF file.
        /// </summary>
        public static readonly String DEST = "results/htmlsamples/ch01/helloWorld08.pdf";

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
            using (Stream license = FileUtil.GetInputStreamForFile(
                Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") + "/itextkey-html2pdf_typography.json"))
            {
                LicenseKey.LoadLicenseFile(license);
            }
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new C01E08_HelloWorld().CreatePdf(BASEURI, SRC, DEST);
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
            IList<IElement> elements = HtmlConverter.ConvertToElements(
                new FileStream(src, FileMode.Open, FileAccess.Read), properties);
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            Document document = new Document(pdf);
            foreach (IElement element in elements)
            {
                document.Add(new Paragraph(element.GetType().FullName));
                document.Add((IBlockElement) element);
            }

            document.Close();
        }
    }
}