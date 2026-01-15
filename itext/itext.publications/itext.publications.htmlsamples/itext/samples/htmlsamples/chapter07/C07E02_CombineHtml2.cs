using iText.Samples.Util;
using System;
using System.Collections.Generic;
using System.IO;
using iText.Commons.Utils;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Licensing.Base;

namespace iText.Samples.Htmlsamples.Chapter07
{
    /// <summary>
    /// Can we parse different HTML files and combine them into one PDF?
    /// Yes, this can be done in different ways. This example shows how
    /// to convert HTML to iText elements, and how to add the elements
    /// of the different HTML files to a single PDF document.
    /// </summary>
    public class C07E02_CombineHtml2
    {
        /// <summary>
        /// The Base URI of the HTML page.
        /// </summary>
        public static readonly String BASEURI = "../../../resources/htmlsamples/html/";

        /// <summary>
        /// An array containing the paths to different HTML files.
        /// </summary>
        public static readonly String[] SRC =
        {
            String.Format("{0}invitation.html", BASEURI),
            String.Format("{0}sxsw.html", BASEURI),
            String.Format("{0}movies.html", BASEURI)
        };

        /// <summary>
        /// The path to the resulting PDF file.
        /// </summary>
        public static readonly String DEST = "results/htmlsamples/ch07/bundle2.pdf";

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

            new C07E02_CombineHtml2().CreatePdf(BASEURI, SRC, DEST);
        }

        /// <summary>
        /// Creates the PDF file.
        /// </summary>
        /// <param name="baseUri">the base URI</param>
        /// <param name="src">an array with the paths to different source HTML files</param>
        /// <param name="dest">the path to the resulting PDF</param>
        public void CreatePdf(String baseUri, String[] src, String dest)
        {
            ConverterProperties properties = new ConverterProperties();
            properties.SetBaseUri(baseUri);
            PdfWriter writer = new PdfWriter(dest);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);
            foreach (String html in src)
            {
                IList<IElement> elements = HtmlConverter.ConvertToElements(
                    new FileStream(html, FileMode.Open, FileAccess.Read), properties);
                foreach (IElement element in elements)
                {
                    document.Add((IBlockElement) element);
                }
            }

            document.Close();
        }
    }
}