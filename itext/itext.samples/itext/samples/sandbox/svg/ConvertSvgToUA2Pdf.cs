using System.IO;
using iText.Commons.Utils;
using iText.Kernel.Pdf;
using iText.Pdfua;
using iText.Svg.Converter;
using iText.Svg.Processors.Impl;

namespace iText.Samples.Sandbox.Svg
{
    public class ConvertSvgToUA2Pdf
    {
        public static readonly string SRC = "../../../resources/svg/";
        public static readonly string DEST = "results/sandbox/svg/Svg2UA2CompliantPdf.pdf";

        public static void Main(string[] args)
        {
            var svgImage = SRC + "cauldron.svg";
            var file = new FileInfo(DEST);
            file.Directory?.Create();

            ManipulatePdf(svgImage, DEST);
        }

        private static void ManipulatePdf(string svgSource, string pdfDest)
        {
            WriterProperties writerProperties = new WriterProperties();
            writerProperties.SetPdfVersion(PdfVersion.PDF_2_0);
            PdfUADocument pdfDocument = new PdfUADocument(
                new PdfWriter(pdfDest, writerProperties),
                new PdfUAConfig(PdfUAConformance.PDF_UA_2, "ua title", "en-US"));

            pdfDocument.AddNewPage();
            SvgConverterProperties properties = new SvgConverterProperties();
            properties.GetAccessibilityProperties().SetAlternateDescription("Hello there");

            SvgConverter.DrawOnDocument(FileUtil.GetInputStreamForFile(svgSource), pdfDocument, 1, properties);
            
            PdfPage page = pdfDocument.AddNewPage();
            SvgConverter.DrawOnPage(FileUtil.GetInputStreamForFile(svgSource), page, properties);
            pdfDocument.Close();
        }
    }
}
