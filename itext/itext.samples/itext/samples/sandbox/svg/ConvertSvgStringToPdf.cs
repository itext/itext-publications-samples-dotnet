using System.IO;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Svg.Converter;
using iText.Svg.Processors.Impl;

namespace iText.Samples.Sandbox.Svg
{
    public class ConvertSvgStringToPdf
    {
        public static readonly string DEST = "results/sandbox/svg/SvgStringToPdf.pdf";

        public static void Main(string[] args)
        {
            var file = new FileInfo(DEST);
            file.Directory?.Create();

            ManipulatePdf(DEST);
        }

        private static void ManipulatePdf(string pdfDest)
        {
            using (var pdfDocument = new PdfDocument(new PdfWriter(pdfDest)))
            {
                //Create new page
                pdfDocument.AddNewPage(PageSize.A4);

                //SVG String
                var contents = "<svg viewBox=\"0 0 240 240\" xmlns=\"http://www.w3.org/2000/svg\">\n"
                               + "  <style media=\"(height: 900px)\">\n"
                               + "    circle {\n"
                               + "      fill: green;\n"
                               + "    }\n"
                               + "  </style>\n"
                               + "\n"
                               + "  <circle cx=\"100\" cy=\"100\" r=\"50\"/>\n"
                               + "</svg>";

                //Convert and draw the string directly to the PDF.
                SvgConverter.DrawOnDocument(contents, pdfDocument, 1, new SvgConverterProperties());
            }
        }
    }
}