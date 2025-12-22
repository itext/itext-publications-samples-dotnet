using System.IO;
using iText.Html2pdf;
using iText.Html2pdf.Attach.Impl;
using iText.Kernel.Pdf;
using iText.Samples.Sandbox.Pdfhtml.Formtagging;

namespace iText.Samples.Sandbox.Pdfhtml
{
   
    // PdfHtmlFormTagging.cs
    //
    // Example showing how to convert HTML forms to tagged PDF with custom roles.
    // Demonstrates using custom tag worker factory for form element tagging.
 
    public class PdfHtmlFormTagging
    {
        public static readonly string SRC = "../../../resources/pdfhtml/PdfHtmlFormTagging/changeFormRole.html";
        public static readonly string DEST = "results/sandbox/pdfhtml/changeFormRole.pdf";
        
        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new PdfHtmlFormTagging().ConvertToPdf(DEST, SRC);
        }

        private void ConvertToPdf(string dest, string src)
        {
            ConverterProperties converterProperties = new ConverterProperties();
            DefaultTagWorkerFactory tagWorkerFactory = new FormTagWorkerFactory();
            converterProperties.SetTagWorkerFactory(tagWorkerFactory);
            
            PdfWriter taggedWriter = new PdfWriter(dest);
            PdfDocument pdfTagged = new PdfDocument(taggedWriter);
            pdfTagged.SetTagged();

            HtmlConverter.ConvertToPdf(new FileStream(src, FileMode.Open), pdfTagged, converterProperties);
        }
    }
}
