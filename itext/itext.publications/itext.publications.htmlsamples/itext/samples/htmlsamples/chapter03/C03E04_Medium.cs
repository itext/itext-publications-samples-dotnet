using iText.Samples.Util;
using System;
using System.IO;
using iText.Commons.Utils;
using iText.Html2pdf;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Licensing.Base;
using iText.StyledXmlParser.Css.Media;

namespace iText.Samples.Htmlsamples.Chapter03
{
    /// <summary>
    /// Converts an HTML file to a PDF document that is meant to view on a medium-sized screen (tablet).
    /// </summary>
    public class C03E04_Medium
    {
        /// <summary>
        /// The path to the resulting PDF file.
        /// </summary>
        public static readonly String DEST = "results/htmlsamples/ch03/sxsw_medium.pdf";

        /// <summary>
        /// The Base URI of the HTML page.
        /// </summary>
        public static readonly String BASEURI = "../../../resources/htmlsamples/html/";

        /// <summary>
        /// The path to the source HTML file.
        /// </summary>
        public static readonly String SRC = String.Format("{0}sxsw.html", BASEURI);

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

            new C03E04_Medium().CreatePdf(BASEURI, SRC, DEST);
        }

        /// <summary>
        /// Creates the PDF file.
        /// </summary>
        /// <param name="baseUri">the base URI</param>
        /// <param name="src">the path to the source HTML file</param>
        /// <param name="dest">the path to the resulting PDF</param>
        public void CreatePdf(String baseUri, String src, String dest)
        {
            PdfWriter writer = new PdfWriter(dest);
            PdfDocument pdf = new PdfDocument(writer);
            pdf.SetTagged();
            PageSize pageSize = new PageSize(575, 1500);
            pdf.SetDefaultPageSize(pageSize);
            ConverterProperties properties = new ConverterProperties();
            properties.SetBaseUri(baseUri);
            MediaDeviceDescription mediaDeviceDescription = new MediaDeviceDescription(MediaType.SCREEN);
            mediaDeviceDescription.SetWidth(pageSize.GetWidth());
            properties.SetMediaDeviceDescription(mediaDeviceDescription);
            HtmlConverter.ConvertToPdf(new FileStream(src, FileMode.Open, FileAccess.Read), pdf, properties);
        }
    }
}