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
using System.Xml.Xsl;
using iText.Html2pdf;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.License;

namespace iText.Samples.Htmlsamples.Chapter04
{
    /// <summary>
    /// Converts an HTML page to a PDF that consists of a single page.
    /// </summary>
    public class C04E03_MovieTable3
    {
        /// <summary>
        /// The path to the resulting PDF file.
        /// </summary>
        public static readonly String DEST = "results/htmlsamples/ch04/movie_table3.pdf";

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
        /// The main method of this example.
        /// </summary>
        /// <param name="args">no arguments are needed to run this example.</param>
        public static void Main(String[] args)
        {
            LicenseKey.LoadLicenseFile(Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") +
                                       "/itextkey-html2pdf_typography.xml");
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            C04E03_MovieTable3 app = new C04E03_MovieTable3();
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
            PdfWriter writer = new PdfWriter(dest);
            PdfDocument pdf = new PdfDocument(writer);
            pdf.SetDefaultPageSize(new PageSize(595, 14400));
            Document document = HtmlConverter.ConvertToDocument(new MemoryStream(html), pdf, properties);
            EndPosition endPosition = new EndPosition();
            LineSeparator separator = new LineSeparator(endPosition);
            document.Add(separator);
            document.GetRenderer().Close();
            PdfPage page = pdf.GetPage(1);
            float y = endPosition.GetY() - 36;
            page.SetMediaBox(new Rectangle(0, y, 595, 14400 - y));
            document.Close();
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
        /// Implementation of the ILineDrawer interface that won't draw a line,
        /// but that will allow us to get the Y-position at the end of the file.
        /// </summary>
        class EndPosition : ILineDrawer
        {
            /// <summary>
            /// A Y-position.
            /// </summary>
            protected float y;

            /// <summary>
            /// Gets the Y-position.
            /// </summary>
            /// <returns>the Y-position</returns>
            public float GetY()
            {
                return y;
            }

            /* (non-Javadoc)
             * @see com.itextpdf.kernel.pdf.canvas.draw.ILineDrawer#draw(com.itextpdf.kernel.pdf.canvas.PdfCanvas, com.itextpdf.kernel.geom.Rectangle)
             */
            public void Draw(PdfCanvas pdfCanvas, Rectangle rect)
            {
                this.y = rect.GetY();
            }

            /* (non-Javadoc)
             * @see com.itextpdf.kernel.pdf.canvas.draw.ILineDrawer#getColor()
             */
            public Color GetColor()
            {
                return null;
            }

            /* (non-Javadoc)
             * @see com.itextpdf.kernel.pdf.canvas.draw.ILineDrawer#getLineWidth()
             */
            public float GetLineWidth()
            {
                return 0;
            }

            /* (non-Javadoc)
             * @see com.itextpdf.kernel.pdf.canvas.draw.ILineDrawer#setColor(com.itextpdf.kernel.color.Color)
             */
            public void SetColor(Color color)
            {
            }

            /* (non-Javadoc)
             * @see com.itextpdf.kernel.pdf.canvas.draw.ILineDrawer#setLineWidth(float)
             */
            public void SetLineWidth(float lineWidth)
            {
            }
        }
    }
}