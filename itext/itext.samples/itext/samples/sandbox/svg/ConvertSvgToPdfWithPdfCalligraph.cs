using System;
using System.IO;
using iText.Commons.Utils;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Licensing.Base;
using iText.Svg.Converter;

namespace iText.Samples.Sandbox.Svg
{
    public class ConvertSvgToPdfWithPdfCalligraph
    {
        private static readonly string SRC = "../../../resources/svg/";
        public static readonly string DEST = "results/sandbox/svg/ConvertSvgToPdfWithPdfCalligraph.pdf";

        public static void Main(string[] args)
        {
            // Load the license file to use typography features
            using (var license = FileUtil.GetInputStreamForFile(
                       Environment.GetEnvironmentVariable("ITEXT_LICENSE_FILE_LOCAL_STORAGE") + "/itextkey-typography.json"))
            {
                LicenseKey.LoadLicenseFile(license);
            }
            
            var svgImage = SRC + "cauldronWithText.svg";
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
                
                //Convert SVG image and add it to the page with the given properties containing the font provider
                SvgConverter.DrawOnPage(svgPath, pdfPage);
            }
        }
    }
}