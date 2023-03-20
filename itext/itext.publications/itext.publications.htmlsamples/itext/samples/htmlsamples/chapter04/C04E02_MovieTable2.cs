using System;
using System.IO;
using System.Xml.Xsl;
using iText.Commons.Utils;
using iText.Html2pdf;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Layout.Element;
using iText.Licensing.Base;

namespace iText.Samples.Htmlsamples.Chapter04
{
    /// <summary>
    /// Creates a PDF document from an XML file using XSLT to convert the XML to HTML,
    /// introducing a single-page PDF in the background as "company stationery" and
    /// as well as a custom page number.
    /// </summary>
    public class C04E02_MovieTable2
    {
        
        /// <summary>
        /// The path to the resulting PDF file.
        /// </summary>
        public static readonly String DEST = "results/htmlsamples/ch04/movie_table2.pdf";

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
        public static readonly String XSL = "../../../resources/htmlsamples/xml/movies_table.xsl";


        /// <summary>
        /// The path to a single-page PDF that will be used as company stationery.
        /// </summary>
        public static readonly String STATIONERY = "../../../resources/htmlsamples/pdf/stationery.pdf";

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

            C04E02_MovieTable2 app = new C04E02_MovieTable2();
            app.CreatePdf(app.CreateHtml(XML, XSL), BASEURI, STATIONERY, DEST);
        }

        /// <summary>
        /// Creates the PDF file.
        /// </summary>
        /// <param name="html">the HTML file as a byte array</param>
        /// <param name="baseUri">the base URI</param>
        /// <param name="stationery">the path to a single-page PDF file that will act as stationery</param>
        /// <param name="dest">the path to the resulting PDF</param>
        public void CreatePdf(byte[] html, String baseUri, String stationery, String dest)
        {
            ConverterProperties properties = new ConverterProperties();
            properties.SetBaseUri(baseUri);
            PdfWriter writer = new PdfWriter(dest);
            PdfDocument pdf = new PdfDocument(writer);
            IEventHandler handler = new Background(pdf, stationery);
            pdf.AddEventHandler(PdfDocumentEvent.START_PAGE, handler);
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

        /// <summary>
        /// Implementation of the IEventHandler to add a background and a page number to every page.
        /// </summary>
        class Background : IEventHandler
        {
            /// <summary>
            /// The Form XObject that will be added as the background for every page.
            /// </summary>
            PdfXObject stationery;

            /// <summary>
            /// Instantiates a new Background instance.
            /// </summary>
            /// <param name="pdf">the PdfDocument instance of the PDF to which the background will be added</param>
            /// <param name="src">the path to the single-page PDF file</param>
            public Background(PdfDocument pdf, String src)
            {
                PdfDocument template = new PdfDocument(new PdfReader(src));
                PdfPage page = template.GetPage(1);
                stationery = page.CopyAsFormXObject(pdf);
                template.Close();
            }

            /* (non-Javadoc)
             * @see com.itextpdf.kernel.events.IEventHandler#handleEvent(com.itextpdf.kernel.events.Event)
             */
            public void HandleEvent(Event @event)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent) @event;
                PdfDocument pdf = docEvent.GetDocument();
                PdfPage page = docEvent.GetPage();
                PdfCanvas pdfCanvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdf);
                pdfCanvas.AddXObjectAt(stationery, 0, 0);
                Rectangle rect = new Rectangle(36, 32, 36, 64);
                Canvas canvas = new Canvas(pdfCanvas, rect);
                canvas.Add(new Paragraph((pdf.GetNumberOfPages().ToString())).SetFontSize(48)
                    .SetFontColor(ColorConstants.WHITE));
                canvas.Close();
            }
        }
    }
}