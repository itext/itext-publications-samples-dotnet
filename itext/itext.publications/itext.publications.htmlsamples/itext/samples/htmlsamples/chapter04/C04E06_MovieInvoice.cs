using System;
using System.IO;
using System.Xml.Xsl;
using iText.Commons.Utils;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Licensing.Base;
using iText.Pdfa;

namespace iText.Samples.Htmlsamples.Chapter04
{
    /// <summary>
    /// Creates a PDF document from an XML file using XSLT to convert the XML to HTML,
    /// introducing customized bookmarks.
    /// </summary>
    public class C04E06_MovieInvoice
    {
        /// <summary>
        /// The path to the resulting PDF file.
        /// </summary>
        /// <returns></returns>
        public static readonly String DEST = "results/htmlsamples/ch04/movie_invoice.pdf";

        /// <summary>
        /// The Base URI of the HTML page.
        /// </summary>
        /// <returns></returns>
        public static readonly String BASEURI = "../../../resources/htmlsamples/html/";

        /// <summary>
        /// The XML containing all the data.
        /// </summary>
        public static readonly String XML = "../../../resources/htmlsamples/xml/movies.xml";

        /// <summary>
        /// The XSLT needed to transform the XML to HTML.
        /// </summary>
        public static readonly String XSL = "../../../resources/htmlsamples/xml/movies_invoice.xsl";

        /// <summary>
        /// The path to the output intent file.
        /// </summary>
        public static readonly String INTENT = "../../../resources/htmlsamples/color/sRGB_CS_profile.icm";

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

            C04E06_MovieInvoice app = new C04E06_MovieInvoice();
            app.CreatePdf(app.CreateHtml(XML, XSL), BASEURI, DEST, INTENT);
        }

        /// <summary>
        /// Creates the PDF file.
        /// </summary>
        /// <param name="html">the HTML file as a byte array</param>
        /// <param name="baseUri">the base URI</param>
        /// <param name="dest">the path to the resulting PDF</param>
        /// <param name="intent">the path to the output intent</param>
        public void CreatePdf(byte[] html, String baseUri, String dest, String intent)
        {
            PdfWriter writer = new PdfWriter(dest);
            PdfADocument pdf = new PdfADocument(writer, PdfAConformance.PDF_A_2B,
                new PdfOutputIntent("Custom", "", "http://www.color.org",
                    "sRGB IEC61966-2.1", new FileStream(intent, FileMode.Open, FileAccess.Read)));
            ConverterProperties properties = new ConverterProperties();
            properties.SetBaseUri(baseUri);
            HtmlConverter.ConvertToPdf(new MemoryStream(html), pdf, properties);
        }

        /// <summary>
        /// Creates an HTML file by performing an XSLT transformation on an XML file.
        /// </summary>
        /// <param name="xmlPath">the path to the XML file.</param>
        /// <param name="xslPath">the path to the XSL file</param>
        /// <returns>the resulting HTML as a byte[]</returns>
        public byte[] CreateHtml(String xmlPath, String xslPath)
        {
            MemoryStream baos = new MemoryStream();
            XslCompiledTransform transform = new XslCompiledTransform();
            transform.Load(xslPath);
            transform.Transform(xmlPath, null, baos);
            return baos.ToArray();
        }
    }
}