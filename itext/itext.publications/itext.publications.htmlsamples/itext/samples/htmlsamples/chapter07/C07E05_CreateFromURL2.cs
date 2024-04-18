using System;
using System.IO;
using System.Net;
using iText.Commons.Utils;
using iText.Html2pdf;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Licensing.Base;
using iText.StyledXmlParser.Css.Media;
using NUnit.Framework;

namespace iText.Samples.Htmlsamples.Chapter07
{
    /// <summary>
    /// Converts a simple HTML file to PDF using an InputStream and an OutputStream
    /// as arguments for the convertToPdf() method.
    /// </summary>
    public class C07E05_CreateFromURL2
    {
        const string USER_AGENT = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0; WOW64; " +
                                  "Trident/4.0; SLCC1; .NET CLR 2.0.50727; Media Center PC 5.0; " +
                                  ".NET CLR 3.5.21022; .NET CLR 3.5.30729; .NET CLR 3.0.30618; " +
                                  "InfoPath.2; OfficeLiveConnector.1.3; OfficeLivePatch.0.0)";

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
            using (Stream license = FileUtil.GetInputStreamForFile(
                Environment.GetEnvironmentVariable("ITEXT7_LICENSEKEY") + "/itextkey-html2pdf_typography.json"))
            {
                LicenseKey.LoadLicenseFile(license);
            }

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
            var maxTries = 3;
            while (maxTries != 0)
            {
                var webClient = new TimedWebClient();
                webClient.Headers.Add("User-Agent", USER_AGENT);

                int responseCode;
                try
                {
                    byte[] website = webClient.DownloadData(url);
                    HtmlConverter.ConvertToPdf(new MemoryStream(website), pdf, properties);
                    break;
                }
                catch (WebException e)
                {
                    if (e.Status == WebExceptionStatus.Timeout)
                    {
                        responseCode = -1;
                    }
                    else
                    {
                        try
                        {
                            responseCode = (int)((HttpWebResponse)e.Response).StatusCode;
                        }
                        catch
                        {
                            responseCode = -1;
                        }
                    }
                }

                Assert.True(
                    (responseCode >= 200 && responseCode < 300) || responseCode == -1,
                    "Http request was not successful. Error code: " + responseCode);

                maxTries--;
            }
        }
    }
}