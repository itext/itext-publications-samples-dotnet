using iText.Samples.Util;
using System;
using System.IO;
using iText.Commons.Utils;
using iText.Html2pdf;
using iText.Licensing.Base;

namespace iText.Samples.Htmlsamples.Chapter07
{
    public class C07E07_IncompleteHTML
    {
        /// <summary>
        /// The path to the resulting PDF file.
        /// </summary>
        public static readonly String DEST = "results/htmlsamples/ch07/complete.pdf";

        /// <summary>
        /// The path to the source HTML file.
        /// </summary>
        public static readonly String SRC = "../../../resources/htmlsamples/html/incomplete.html";

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

            C07E07_IncompleteHTML app = new C07E07_IncompleteHTML();
            app.CreatePdf(SRC, DEST);
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