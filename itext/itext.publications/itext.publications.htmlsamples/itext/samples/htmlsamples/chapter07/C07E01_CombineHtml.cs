using System;
using System.IO;
using iText.Commons.Utils;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using iText.Licensing.Base;

namespace iText.Samples.Htmlsamples.Chapter07
{
    /// <summary>
    /// Can we parse different HTML files and combine them into one PDF?
    /// Yes, this can be done in different ways. This example shows how
    /// to create a PDF in memory for each HTML, then use PdfMerger to
    /// merge the different PDFs into one, on a page per page basis.
    /// </summary>
    public class C07E01_CombineHtml
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
        public static readonly String DEST = "results/htmlsamples/ch07/bundle.pdf";

        /// <summary>
        /// The main method of this example.
        /// </summary>
        /// <param name="args">no arguments are needed to run this example.</param>
        public static void Main(String[] args)
        {
            using (Stream license = FileUtil.GetInputStreamForFile(
                Environment.GetEnvironmentVariable("ITEXT_LICENSE_FILE_LOCAL_STORAGE") + "/itextkey-html2pdf_typography.json"))
            {
                LicenseKey.LoadLicenseFile(license);
            }
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new C07E01_CombineHtml().CreatePdf(BASEURI, SRC, DEST);
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
            PdfMerger merger = new PdfMerger(pdf);
            foreach (String html in src)
            {
                MemoryStream baos = new MemoryStream();
                PdfDocument temp = new PdfDocument(new PdfWriter(baos));
                HtmlConverter.ConvertToPdf(new FileStream(html, FileMode.Open, FileAccess.Read), temp, properties);
                temp = new PdfDocument(new PdfReader(new MemoryStream(baos.ToArray())));
                merger.Merge(temp, 1, temp.GetNumberOfPages());
                temp.Close();
            }

            pdf.Close();
        }
    }
}