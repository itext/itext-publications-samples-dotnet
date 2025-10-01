using System;
using System.IO;
using iText.Commons.Utils;
using iText.Html2pdf;
using iText.Licensing.Base;

namespace iText.Samples.Htmlsamples.Chapter07
{
    /// <summary>
    /// Converts an HTML file containing a form to a PDF file with AcroForm fields.
    /// </summary>
    public class C07E10_HelloAcroForm
    {
        /// <summary>
        /// The path to the resulting PDF file.
        /// </summary>
        public static readonly String DEST = "results/htmlsamples/ch07/form_acroform.pdf";

        /// <summary>
        /// The path to the source HTML file.
        /// </summary>
        public static readonly String SRC = "../../../resources/htmlsamples/html/form.html";

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

            new C07E10_HelloAcroForm().CreatePdf(SRC, DEST);
        }

        /// <summary>
        /// Creates the PDF file.
        /// </summary>
        /// <param name="src">the path to the source HTML file</param>
        /// <param name="dest">the path to the resulting PDF</param>
        public void CreatePdf(String src, String dest)
        {
            ConverterProperties properties = new ConverterProperties();
            properties.SetCreateAcroForm(true);
            HtmlConverter.ConvertToPdf(new FileInfo(src), new FileInfo(dest), properties);
        }
    }
}