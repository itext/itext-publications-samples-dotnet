using System;
using System.IO;
using iText.Commons.Utils;
using iText.Html2pdf;
using iText.Html2pdf.Resolver.Font;
using iText.IO.Font;
using iText.Layout.Font;
using iText.Licensing.Base;

namespace iText.Samples.Htmlsamples.Chapter06
{
    public class C06E11_Internationalization
    {
        /// <summary>
        /// An array with the paths to extra fonts.
        /// </summary>
        public static readonly String[] FONTS =
        {
            "../../../resources/htmlsamples/fonts/noto/NotoSans-Regular.ttf",
            "../../../resources/htmlsamples/fonts/noto/NotoSans-Bold.ttf",
            "../../../resources/htmlsamples/fonts/noto/NotoSansCJKsc-Regular.otf",
            "../../../resources/htmlsamples/fonts/noto/NotoSansCJKjp-Regular.otf",
            "../../../resources/htmlsamples/fonts/noto/NotoSansCJKkr-Regular.otf",
            "../../../resources/htmlsamples/fonts/noto/NotoNaskhArabic-Regular.ttf",
            "../../../resources/htmlsamples/fonts/noto/NotoSansHebrew-Regular.ttf"
        };

        /// <summary>
        /// The path to the resulting PDF file.
        /// </summary>
        public static readonly String DEST = "results/htmlsamples/ch06/fonts_i18n.pdf";

        /// <summary>
        /// The path to the source HTML file.
        /// </summary>
        public static readonly String SRC = "../../../resources/htmlsamples/html/fonts_i18n.html";

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

            C06E11_Internationalization app = new C06E11_Internationalization();
            app.CreatePdf(SRC, FONTS, DEST);
        }

        /// <summary>
        /// Creates the PDF file.
        /// </summary>
        /// <param name="src">the path to the source HTML file</param>
        /// <param name="fonts">an array containing paths to extra fonts</param>
        /// <param name="dest">the path to the resulting PDF</param>
        public void CreatePdf(String src, String[] fonts, String dest)
        {
            ConverterProperties properties = new ConverterProperties();
            FontProvider fontProvider = new DefaultFontProvider(false, false, false);
            foreach (String font in fonts)
            {
                FontProgram fontProgram = FontProgramFactory.CreateFont(font);
                fontProvider.AddFont(fontProgram);
            }

            properties.SetFontProvider(fontProvider);
            HtmlConverter.ConvertToPdf(new FileInfo(src), new FileInfo(dest), properties);
        }
    }
}