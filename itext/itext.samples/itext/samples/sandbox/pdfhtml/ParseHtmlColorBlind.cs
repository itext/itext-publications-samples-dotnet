using System.IO;
using iText.Html2pdf;
using iText.Html2pdf.Css.Apply.Impl;
using iText.Samples.Sandbox.Pdfhtml.Colorblindness;

namespace iText.Samples.Sandbox.Pdfhtml
{
   
    // ParseHtmlColorBlind.cs
    //
    // Example showing HTML to PDF conversion simulating color blindness vision.
    // Demonstrates custom CSS applier to transform colors for accessibility.
 
    public class ParseHtmlColorBlind
    {
        public static readonly string SRC = "../../../resources/pdfhtml/rainbow/";
        public static readonly string DEST = "results/sandbox/pdfhtml/rainbow_colourBlind.pdf";

        public static void Main(string[] args)
        {
            string currentSrc = SRC + "rainbow.html";
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();

            new ParseHtmlColorBlind().ManipulatePdf(currentSrc, DEST, SRC);
        }

        public void ManipulatePdf(string htmlSource, string pdfDest, string resourceLoc)
        {
            // Base URI is required to resolve the path to source files
            ConverterProperties converterProperties = new ConverterProperties().SetBaseUri(resourceLoc);

            // Create custom css applier factory.
            // Current custom css applier factory handle <div> and <span> tags of html and returns corresponding css applier.
            // All of that css appliers change value of RGB colors
            // to simulate color blindness of people (like Tritanopia, Achromatopsia, etc.)
            DefaultCssApplierFactory cssApplierFactory =
                new ColorBlindnessCssApplierFactory(ColorBlindnessTransforms.DEUTERANOMALY);
            converterProperties.SetCssApplierFactory(cssApplierFactory);
            
            HtmlConverter.ConvertToPdf(
                new FileStream(htmlSource, FileMode.Open, FileAccess.Read, FileShare.Read), 
                new FileStream(pdfDest, FileMode.Create, FileAccess.Write), 
                converterProperties);
        }
    }
}