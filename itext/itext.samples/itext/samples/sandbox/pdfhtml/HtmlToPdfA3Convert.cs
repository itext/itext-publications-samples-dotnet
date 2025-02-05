using System;
using System.IO;
using iText.Html2pdf;
using iText.Html2pdf.Resolver.Font;
using iText.StyledXmlParser.Resolver.Font;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Pdfhtml 
{
    public class HtmlToPdfA3Convert 
    {
        public static readonly string SRC = "../../../resources/pdfhtml/HtmlToPdfA3Convert/";
        public static readonly string DEST = "results/sandbox/pdfhtml/HtmlToPdfA3Convert.pdf";

        public static void Main(String[] args) 
        {
            var file = new FileInfo(DEST);
            file.Directory.Create();
            
            new HtmlToPdfA3Convert().ManipulatePdf(DEST);
        }

        public void ManipulatePdf(String pdfDest) 
        {
            var htmlSource = SRC + "MixedContent.html";
            
            var inputStream = new FileStream(SRC + "sRGB Color Space Profile.icm", FileMode.Open, FileAccess.Read);
            
            var converterProperties = new ConverterProperties();
            converterProperties.SetBaseUri(SRC);
            converterProperties.SetPdfAConformance(PdfAConformance.PDF_A_3B);
            converterProperties.SetDocumentOutputIntent(new PdfOutputIntent("Custom", "", "http://www.color.org",
            "sRGB IEC61966-2.1", inputStream));
            converterProperties.SetFontProvider(new BasicFontProvider(false, true, false));
            
            HtmlConverter.ConvertToPdf(new FileStream(htmlSource, FileMode.Open, FileAccess.Read), new FileStream(pdfDest, FileMode.Create), converterProperties);
        }
    }
}
