using System;
using System.IO;
using iText.Commons.Utils;
using iText.Html2pdf;
using iText.Html2pdf.Resolver.Font;
using iText.StyledXmlParser.Resolver.Font;
using iText.IO.Font;
using iText.Layout.Font;
using iText.Licensing.Base;

namespace iText.Samples.Htmlsamples.Chapter06
{
    public class C06E09_Encoding
    {
        /// <summary>
        /// The path to the resulting PDF file.
        /// </summary>
        public static readonly String DEST = "results/htmlsamples/ch06/fonts_encoding.pdf";

        /// <summary>
        /// The path to the source HTML file.
        /// </summary>
        public static readonly String SRC = "../../../resources/htmlsamples/html/hello.html";

        /// <summary>
        /// The path to an extra font.
        /// </summary>
        public static readonly String FONT = "../../../resources/htmlsamples/fonts/cardo/Cardo-Regular.ttf";

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

            C06E09_Encoding app = new C06E09_Encoding();
            app.CreatePdf(SRC, FONT, DEST);
        }

        /// <summary>
        /// Creates the PDF file.
        /// </summary>
        /// <param name="src">the path to the source HTML file</param>
        /// <param name="font">the path to an extra fonts</param>
        /// <param name="dest">the path to the resulting PDF</param>
        public void CreatePdf(String src, String font, String dest)
        {
            ConverterProperties properties = new ConverterProperties();
            FontProvider fontProvider = new BasicFontProvider(false, false, false);
            FontProgram fontProgram = FontProgramFactory.CreateFont(font);
            fontProvider.AddFont(fontProgram, "Winansi");
            properties.SetFontProvider(fontProvider);
            HtmlConverter.ConvertToPdf(new FileInfo(src), new FileInfo(dest), properties);
        }
    }
}