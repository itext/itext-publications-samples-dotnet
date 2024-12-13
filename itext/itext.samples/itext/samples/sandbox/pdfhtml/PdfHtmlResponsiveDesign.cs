using System.Globalization;
using System.IO;
using iText.Html2pdf;
using iText.Html2pdf.Resolver.Font;
using iText.StyledXmlParser.Resolver.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout.Font;
using iText.StyledXmlParser.Css.Media;
using iText.StyledXmlParser.Css.Util;

namespace iText.Samples.Sandbox.Pdfhtml
{
    public class PdfHtmlResponsiveDesign
    {
        public static readonly string SRC = "../../../resources/pdfhtml/ResponsiveDesign/responsive/";
        public static readonly string DEST = "results/sandbox/pdfhtml/<filename>";

        public static readonly PageSize[] pageSizes =
        {
            PageSize.A4.Rotate(),
            new PageSize(720, PageSize.A4.GetHeight()),
            new PageSize(PageSize.A5.GetWidth(), PageSize.A4.GetHeight())
        };

        public static void Main(string[] args)
        {
            FileInfo file = new FileInfo(DEST.Replace("<filename>", ""));
            file.Directory.Create();
            string htmlSource = SRC + "responsive.html";
            PdfHtmlResponsiveDesign runner = new PdfHtmlResponsiveDesign();

            // Create a pdf for each page size
            for (int i = 0; i < pageSizes.Length; i++)
            {
                float width = CssDimensionParsingUtils.ParseAbsoluteLength(pageSizes[i].GetWidth().ToString(CultureInfo.InvariantCulture));
                string dest = DEST.Replace("<filename>", 
                    "responsive_" + width.ToString("0.0", CultureInfo.InvariantCulture) + ".pdf");

                runner.ManipulatePdf(htmlSource, dest, SRC, pageSizes[i], width);
            }
        }

        public void ManipulatePdf(string htmlSource, string pdfDest, string resourceLoc, PageSize pageSize, float screenWidth)
        {
            PdfWriter writer = new PdfWriter(pdfDest);
            PdfDocument pdfDoc = new PdfDocument(writer);

            // Set the result to be tagged
            pdfDoc.SetTagged();
            pdfDoc.SetDefaultPageSize(pageSize);

            ConverterProperties converterProperties = new ConverterProperties();

            // Set media device description details
            MediaDeviceDescription mediaDescription = new MediaDeviceDescription(MediaType.SCREEN);
            mediaDescription.SetWidth(screenWidth);
            converterProperties.SetMediaDeviceDescription(mediaDescription);

            FontProvider fp = new BasicFontProvider();

            // Register external font directory
            fp.AddDirectory(resourceLoc);

            converterProperties.SetFontProvider(fp);
            // Base URI is required to resolve the path to source files
            converterProperties.SetBaseUri(resourceLoc);

            // Create acroforms from text and button input fields
            converterProperties.SetCreateAcroForm(true);
            
            HtmlConverter.ConvertToPdf(new FileStream(htmlSource, FileMode.Open, FileAccess.Read, FileShare.Read), 
                pdfDoc, converterProperties);

            pdfDoc.Close();
        }
    }
}