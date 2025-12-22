using System;
using System.IO;
using iText.Html2pdf;
using iText.Kernel.Pdf;

namespace iText.Samples.Sandbox.Pdfhtml.pdfua2
{
   
    // Html2UA2CompliantPdf.cs
    //
    // Example showing how to convert HTML to PDF/UA-2 compliant document.
    // Demonstrates setting PDF 2.0 version with universal accessibility.
 
    public class Html2UA2CompliantPdf
    {
        public const String DEST = "results/sandbox/pdfua2/html2UA2CompliantPdf.pdf";

        public const String SRC = "../../../resources/pdfhtml/pdfua2/";
        

        public static void Main(String[] args) {
            String currentSrc = SRC + "html2UA2CompliantPdf.html";
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new Html2UA2CompliantPdf().ManipulatePdf(currentSrc, DEST);
        }
        
        public virtual void ManipulatePdf(String htmlSource, String pdfDest) {
            ConverterProperties converterProperties = new ConverterProperties();
            converterProperties.SetPdfUAConformance(PdfUAConformance.PDF_UA_2);
            WriterProperties writerProperties = new WriterProperties();
            writerProperties.SetPdfVersion(PdfVersion.PDF_2_0);
            FileStream fileInputStream = new FileStream(htmlSource, FileMode.Open, FileAccess.Read);
            using (PdfWriter pdfWriter = new PdfWriter(pdfDest, writerProperties)) {
                HtmlConverter.ConvertToPdf(fileInputStream, pdfWriter, converterProperties);
            }
        }
    }
}