using System.IO;
using iText.Svg.Converter;

namespace iText.Samples.Sandbox.Svg
{
    public class ConvertSvgToPdf
    {
        public static readonly string SRC = "../../../resources/svg/";
        public static readonly string DEST = "results/sandbox/svg/ConvertSvgToPdf.pdf";

        public static void Main(string[] args)
        {
            var svgImage = SRC + "cauldron.svg";
            var file = new FileInfo(DEST);
            file.Directory?.Create();

            ManipulatePdf(svgImage, DEST);
        }

        private static void ManipulatePdf(string svgSource, string pdfDest)
        {
            var svgFile = new FileInfo(svgSource);
            var destinationPdf = new FileInfo(pdfDest);

            //Directly convert the SVG file to a PDF.
            SvgConverter.CreatePdf(svgFile, destinationPdf);
        }
    }
}