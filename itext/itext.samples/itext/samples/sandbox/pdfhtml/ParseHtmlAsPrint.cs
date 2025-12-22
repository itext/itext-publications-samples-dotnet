using System.IO;
using iText.Html2pdf;
using iText.StyledXmlParser.Css.Media;

namespace iText.Samples.Sandbox.Pdfhtml
{
   
    // ParseHtmlAsPrint.cs
    //
    // Example showing how to parse HTML with print media type for PDF output.
    // Demonstrates setting media device description for CSS media queries.
 
    public class ParseHtmlAsPrint
    {
        public static readonly string SRC = "../../../resources/pdfhtml/media/";
        public static readonly string DEST = "results/sandbox/pdfhtml/rainbow_asPrint.pdf";

        public static void Main(string[] args)
        {
            string currentSrc = SRC + "rainbow.html";
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ParseHtmlAsPrint().ManipulatePdf(currentSrc, DEST, SRC);
        }

        public void ManipulatePdf(string htmlSource, string pdfDest, string resourceLoc)
        {
            // Base URI is required to resolve the path to source files
            ConverterProperties converterProperties = new ConverterProperties().SetBaseUri(resourceLoc);

            // Set media device type to correctly parsing html with media handling
            converterProperties.SetMediaDeviceDescription(new MediaDeviceDescription(MediaType.PRINT));
            
            HtmlConverter.ConvertToPdf(
                new FileStream(htmlSource, FileMode.Open), 
                new FileStream(pdfDest, FileMode.Create, FileAccess.Write), 
                converterProperties);
        }
    }
}