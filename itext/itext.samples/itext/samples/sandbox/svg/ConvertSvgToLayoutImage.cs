using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Svg.Converter;

namespace iText.Samples.Sandbox.Svg
{
    public class ConvertSvgToLayoutImage
    {
        private static readonly string SRC = "../../../resources/svg/";
        public static readonly string DEST = "results/sandbox/svg/ConvertSvgToLayoutImage.pdf";

        public static void Main(string[] args)
        {
            var svgImage = SRC + "cauldron.svg";
            var file = new FileInfo(DEST);
            file.Directory?.Create();

            ManipulatePdf(svgImage, DEST);
        }

        private static void ManipulatePdf(string svgSource, string pdfDest)
        {
            using (var pdfDocument = new PdfDocument(new PdfWriter(pdfDest)))
            {
                var doc = new Document(pdfDocument);

                //Create new page
                pdfDocument.AddNewPage(PageSize.A4);
                
                doc.Add(new Paragraph("This is some text added before the SVG image."));

                //SVG image
                var svgPath = new FileStream(svgSource, FileMode.Open, FileAccess.Read);

                //Convert to image
                var image = SvgConverter.ConvertToImage(svgPath, pdfDocument);

                //Set scale
                image.ScaleToFit(PageSize.A4.GetWidth()/2, PageSize.A4.GetHeight()/2);

                //Add to the document
                doc.Add(image);
                
                doc.Add(new Paragraph("This is some text added after the SVG image."));
            }
        }
    }
}