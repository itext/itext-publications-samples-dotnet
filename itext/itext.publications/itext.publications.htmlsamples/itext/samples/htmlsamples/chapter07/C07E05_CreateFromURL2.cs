using System;
using System.IO;
using System.Net;
using iText.Html2pdf;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.License;
using iText.StyledXmlParser.Css.Media;

namespace iText.Samples.Htmlsamples.Chapter07
{
    /// <summary>
    /// Converts a simple HTML file to PDF using an InputStream and an OutputStream
    /// as arguments for the convertToPdf() method.
    /// </summary>
    public class C07E05_CreateFromURL2
    {
        const string USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36";
        
        /// <summary>
        /// The path to the resulting PDF file.
        /// </summary>
        public static readonly String DEST = "results/htmlsamples/ch07/url2pdf_2.pdf";

        /// <summary>
        /// The target folder for the result.
        /// </summary>
        public static readonly String ADDRESS = "https://stackoverflow.com/help/on-topic";

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

            new C07E05_CreateFromURL2().CreatePdf(new Uri(ADDRESS), DEST);
        }

        /// <summary>
        /// Creates the PDF file.
        /// </summary>
        /// <param name="url">the URL object for the web page</param>
        /// <param name="dest">the path to the resulting PDF</param>
        public void CreatePdf(Uri url, String dest)
        {
            PdfWriter writer = new PdfWriter(dest);
            PdfDocument pdf = new PdfDocument(writer);
            PageSize pageSize = new PageSize(850, 1700);
            pdf.SetDefaultPageSize(pageSize);
            ConverterProperties properties = new ConverterProperties();
            MediaDeviceDescription mediaDeviceDescription = new MediaDeviceDescription(MediaType.SCREEN);
            mediaDeviceDescription.SetWidth(pageSize.GetWidth());
            properties.SetMediaDeviceDescription(mediaDeviceDescription);
            //Some websites forbid web-page access if user-agent is not defined.
            using (var webClient = new WebClient())
            {
                webClient.Headers.Add("user-agent", USER_AGENT);
                byte[] website = webClient.DownloadData(url);
                HtmlConverter.ConvertToPdf(new MemoryStream(website), pdf, properties);
            }
        }
    }
}