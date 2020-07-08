using System.IO;
using iText.Html2pdf;
using iText.Html2pdf.Resolver.Font;
using iText.Kernel.Pdf;
using iText.Layout.Font;

namespace iText.Samples.Sandbox.Pdfhtml
{
    public class UsingCustomFonts
    {
        public static readonly string SRC = "../../../resources/pdfhtml/FontExample/";
        public static readonly string DEST = "results/sandbox/pdfhtml/FontExample.pdf";
        public static readonly string FONT_FOLDER = "../../../resources/pdfhtml/FontExample/font/";
        public static readonly string FONT1 = "../../../resources/font/New Walt Disney.ttf";
        public static readonly string FONT2 = "../../../resources/font/Greifswalder Tengwar.ttf";

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            string htmlSource = SRC + "FontExample.html";

            new UsingCustomFonts().ManipulatePdf(htmlSource, DEST);
        }

        public void ManipulatePdf(string htmlSource, string pdfDest)
        {
            PdfWriter writer = new PdfWriter(pdfDest);
            PdfDocument pdfDoc = new PdfDocument(writer);

            // Default provider will register standard fonts and free fonts shipped with iText, but not system fonts
            FontProvider provider = new DefaultFontProvider();

            // 1. Register all fonts in a directory
            provider.AddDirectory(FONT_FOLDER);

            // 2. Register a single font by specifying path
            provider.AddFont(FONT1);

            // 3. Use the raw bytes of the font file
            byte[] fontBytes = File.ReadAllBytes(FONT2);
            provider.AddFont(fontBytes);

            // Make sure the provider is used
            ConverterProperties converterProperties = new ConverterProperties()
                // Base URI is required to resolve the path to source files
                .SetBaseUri(SRC)
                .SetFontProvider(provider);
            
            HtmlConverter.ConvertToPdf(new FileStream(htmlSource, FileMode.Open), pdfDoc, converterProperties);

            pdfDoc.Close();
        }
    }
}