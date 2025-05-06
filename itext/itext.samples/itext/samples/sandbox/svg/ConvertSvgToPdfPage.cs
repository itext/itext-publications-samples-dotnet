using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Svg.Converter;
using iText.Svg.Processors.Impl;

namespace iText.Samples.Sandbox.Svg
{
    public class ConvertSvgToPdfPage
    {
        private static readonly string SRC = "../../../resources/svg/";
        public static readonly string DEST = "results/sandbox/svg/SvgToPdfPage.pdf";

        public static void Main(string[] args)
        {
            var svgImage = SRC + "cauldron.svg";
            var file = new FileInfo(DEST);
            file.Directory?.Create();

            ManipulatePdf(svgImage, DEST);
        }

        private static void ManipulatePdf(string svgSource, string pdfDest)
        {
            using(var pdfDocument = new PdfDocument(new PdfWriter(pdfDest)))
            {
                //Create new page
                var pdfPage = pdfDocument.AddNewPage(PageSize.A4);

                //SVG image
                var svgPath = new FileStream(svgSource, FileMode.Open, FileAccess.Read);

                //Create SVG converter properties object
                var props = new SvgConverterProperties();

                //Draw image on the page
                SvgConverter.DrawOnPage(svgPath, pdfPage, props);
            }
        }
    }
}