using iText.Samples.Util;
using System;
using System.IO;
using iText.Commons.Utils;
using iText.Html2pdf;
using iText.Html2pdf.Resolver.Font;
using iText.StyledXmlParser.Resolver.Font;
using iText.Licensing.Base;

namespace iText.Samples.Htmlsamples.Chapter06
{
    public class C06E03_SystemFonts
    {
        /// <summary>
        /// The path to the resulting PDF file.
        /// </summary>
        public static readonly String DEST = "results/htmlsamples/ch06/fonts_system.pdf";

        /// <summary>
        /// The path to the source HTML file.
        /// </summary>
        public static readonly String SRC = "../../../resources/htmlsamples/html/fonts_system.html";

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

            C06E03_SystemFonts app = new C06E03_SystemFonts();
            app.CreatePdf(SRC, DEST);
        }

        /// <summary>
        /// Creates the PDF file.
        /// </summary>
        /// <param name="src">the path to the source HTML file</param>
        /// <param name="dest">the path to the resulting PDF</param>
        public void CreatePdf(String src, String dest)
        {
            ConverterProperties properties = new ConverterProperties();
            properties.SetFontProvider(new BasicFontProvider(true, true, true));
            HtmlConverter.ConvertToPdf(new FileInfo(src), new FileInfo(dest), properties);
        }
    }
}