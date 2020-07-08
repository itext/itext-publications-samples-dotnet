using System;
using System.IO;
using iText.Html2pdf;
using iText.Html2pdf.Resolver.Font;
using iText.Kernel.Pdf;
using iText.Pdfa;

namespace iText.Samples.Sandbox.Pdfhtml 
{
    public class HtmlToPdfAConvert 
    {
        public static readonly string SRC = "../../../resources/pdfhtml/HtmlToPdfAConvert/";
        public static readonly string DEST = "results/sandbox/pdfhtml/HtmlToPdfAConvert.pdf";

        public static void Main(String[] args) 
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            
            new HtmlToPdfAConvert().ManipulatePdf(DEST);
        }

        public void ManipulatePdf(String pdfDest) 
        {
            String htmlSource = SRC + "hello.html";
            
            Stream inputStream = new FileStream(SRC + "sRGB Color Space Profile.icm", FileMode.Open, FileAccess.Read);
            
            ConverterProperties converterProperties = new ConverterProperties();
            
            // Pdf/A files should have only embedded fonts inside. That's why the standard pdf fonts should be removed from
            // the FontProvider, which contains fonts to be used during conversion
            converterProperties.SetFontProvider(new DefaultFontProvider(false, true, false));
            
            PdfADocument pdfADocument = new PdfADocument(new PdfWriter(pdfDest), PdfAConformanceLevel.PDF_A_1B, 
                    new PdfOutputIntent("Custom", "", "http://www.color.org",
                            "sRGB IEC61966-2.1", inputStream));
            
            HtmlConverter.ConvertToPdf(new FileStream(htmlSource, FileMode.Open, FileAccess.Read), pdfADocument, converterProperties);
        }
    }
}
