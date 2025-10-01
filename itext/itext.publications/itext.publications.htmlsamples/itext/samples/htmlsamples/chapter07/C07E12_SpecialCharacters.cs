using System;
using System.IO;
using iText.Commons.Utils;
using iText.Html2pdf;
using iText.Licensing.Base;

namespace iText.Samples.Htmlsamples.Chapter07
{
    /// <summary>
    /// Converts an HTML file with special entities to a PDF.
    /// </summary>
    public class C07E12_SpecialCharacters
    {
        /// <summary>
        /// An HTML file as a String value.
        /// </summary>
        public static readonly String HTML = "<html><head></head>" +
                                             "<body style=\"font-size:12.0pt; font-family:Arial\">" +
                                             "<p>Special symbols: " +
                                             "&larr;  &darr; &harr; &uarr; &rarr; &euro; &copy; &#9786;</p>" +
                                             "</body></html>";

        /// <summary>
        /// The path to the resulting PDF file.
        /// </summary>
        public static readonly String DEST = "results/htmlsamples/ch07/special_characters.pdf";

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

            new C07E12_SpecialCharacters().CreatePdf(HTML, DEST);
        }

        /// <summary>
        /// Creates the PDF file.
        /// </summary>
        /// <param name="html">the source HTML file as a String value.</param>
        /// <param name="dest">the path to the resulting PDF</param>
        public void CreatePdf(String html, String dest)
        {
            HtmlConverter.ConvertToPdf(html, new FileStream(dest, FileMode.Create));
        }
    }
}