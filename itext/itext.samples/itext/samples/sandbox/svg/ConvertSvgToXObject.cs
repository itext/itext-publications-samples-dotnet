using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Svg.Converter;

namespace iText.Samples.Sandbox.Svg
{
    public class ConvertSvgToXObject
    {
        public static readonly string SRC = "../../../resources/svg/";
        public static readonly string DEST = "results/sandbox/svg/SvgToXObject.pdf";

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

                //Convert directly to an XObject
                var xObj = SvgConverter.ConvertToXObject(svgPath, pdfDocument);

                //Get the PdfCanvas of the page
                var canv = new PdfCanvas(pdfPage);

                //Add the XObject to the page
                canv.AddXObjectFittedIntoRectangle(xObj, new Rectangle(100, 180, PageSize.A4.GetWidth()/2, PageSize.A4.GetHeight()/2));
            }
        }
    }
}