using System;
using System.IO;
using iText.Commons.Utils;
using iText.Html2pdf;
using iText.Licensing.Base;

namespace iText.Samples.Htmlsamples.Chapter03
{
    /// <summary>
    /// Converts an HTML file to a PDF document using the default properties.
    /// </summary>
    public class C03E01_Normal
    {
        /// <summary>
        /// The path to the resulting PDF file.
        /// </summary>
        public static readonly String DEST = "results/htmlsamples/ch03/sxsw_normal.pdf";

        /// <summary>
        /// The path to the source HTML file.
        /// </summary>
        public static readonly String SRC = "../../../resources/htmlsamples/html/sxsw.html";

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

            new C03E01_Normal().CreatePdf(SRC, DEST);
        }

        /// <summary>
        /// Creates the PDF file.
        /// </summary>
        /// <param name="src">the path to the source HTML file</param>
        /// <param name="dest">the path to the resulting PDF</param>
        public void CreatePdf(String src, String dest)
        {
            HtmlConverter.ConvertToPdf(new FileInfo(src), new FileInfo(dest));
        }
    }
}