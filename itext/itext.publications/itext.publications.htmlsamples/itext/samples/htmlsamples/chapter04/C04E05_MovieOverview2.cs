using iText.Samples.Util;
using System;
using System.IO;
using System.Xml.Xsl;
using iText.Commons.Utils;
using iText.Html2pdf;
using iText.Html2pdf.Attach.Impl;
using iText.Licensing.Base;

namespace iText.Samples.Htmlsamples.Chapter04
{
    /// <summary>
    /// Creates a PDF document from an XML file using XSLT to convert the XML to HTML,
    /// introducing customized bookmarks.
    /// </summary>
    public class C04E05_MovieOverview2
    {
        /// <summary>
        /// The path to the resulting PDF file.
        /// </summary>
        public static readonly String DEST = "results/htmlsamples/ch04/movie_overview2.pdf";

        /// <summary>
        /// The Base URI of the HTML page.
        /// </summary>
        public static readonly String BASEURI = "../../../resources/htmlsamples/html/";

        /// <summary>
        /// The XML containing all the data.
        /// </summary>
        public static readonly String XML = "../../../resources/htmlsamples/xml/movies.xml";

        /// <summary>
        /// The XSLT needed to transform the XML to HTML.
        /// </summary>
        public static readonly String XSL = "../../../resources/htmlsamples/xml/movies_overview.xsl";

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

            C04E05_MovieOverview2 app = new C04E05_MovieOverview2();
            app.CreatePdf(app.CreateHtml(XML, XSL), BASEURI, DEST);
        }

        /// <summary>
        /// Creates the PDF file.
        /// </summary>
        /// <param name="html">the HTML file as a byte array</param>
        /// <param name="baseUri">the base URI</param>
        /// <param name="dest">the path to the resulting PDF</param>
        public void CreatePdf(byte[] html, String baseUri, String dest)
        {
            ConverterProperties properties = new ConverterProperties();
            properties.SetBaseUri(baseUri);
            OutlineHandler outlineHandler = new OutlineHandler();
            outlineHandler.PutMarkPriorityMapping("h1", 1);
            outlineHandler.PutMarkPriorityMapping("p", 2);
            properties.SetOutlineHandler(outlineHandler);
            HtmlConverter.ConvertToPdf(new MemoryStream(html), new FileStream(dest, FileMode.Create),
                properties);
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