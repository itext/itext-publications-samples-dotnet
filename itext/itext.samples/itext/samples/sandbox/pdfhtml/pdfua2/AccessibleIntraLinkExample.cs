using System;
using System.IO;
using iText.Html2pdf.Resolver.Font;
using iText.StyledXmlParser.Resolver.Font;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Kernel.XMP;
using iText.Layout.Font;

namespace iText.Samples.Sandbox.Pdfhtml.pdfua2
{
   
    // AccessibleIntraLinkExample.cs
    //
    // Example showing how to create accessible PDF/UA-2 with internal links.
    // Demonstrates converting HTML with links to tagged PDF 2.0 document.
 
    public class AccessibleIntraLinkExample {
        public const String DEST = "results/sandbox/pdfua2/pdf_ua_links.pdf";

        public const String SRC = "../../../resources/pdfhtml/pdfua2/";

        public const String FONT = "../../../resources/font/FreeSans.ttf";

        public const String UA_XMP = "../../../resources/xml/pdf_ua_xmp.xmp";

        public static void Main(String[] args) {
            String currentSrc = SRC + "simpleLinks.html";
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            new AccessibleIntraLinkExample().ManipulatePdf(currentSrc, DEST, SRC);
        }

        public virtual void ManipulatePdf(String htmlSource, String pdfDest, String resourceLoc) {
            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(pdfDest, new WriterProperties().SetPdfVersion(PdfVersion
                .PDF_2_0)));
            // Create pdf/ua-2 document in which content will be placed
            CreateSimplePdfUA2Document(pdfDocument);
            ConverterProperties converterProperties = new ConverterProperties();
            FontProvider fontProvider = new BasicFontProvider(false, true, false);
            // Base URI is required to resolve the path to source files, setting font provider which provides only embeddable fonts
            converterProperties.SetFontProvider(fontProvider).SetBaseUri(resourceLoc);
            HtmlConverter.ConvertToPdf(new FileStream(htmlSource, FileMode.Open, FileAccess.Read), pdfDocument, converterProperties
                );
        }

        private static void CreateSimplePdfUA2Document(PdfDocument pdfDocument) {
            byte[] bytes = File.ReadAllBytes(System.IO.Path.Combine(UA_XMP));
            XMPMeta xmpMeta = XMPMetaFactory.Parse(new MemoryStream(bytes));
            pdfDocument.SetXmpMetadata(xmpMeta);
            pdfDocument.SetTagged();
            pdfDocument.GetCatalog().SetViewerPreferences(new PdfViewerPreferences().SetDisplayDocTitle(true));
            pdfDocument.GetCatalog().SetLang(new PdfString("en-US"));
            PdfDocumentInfo info = pdfDocument.GetDocumentInfo();
            info.SetTitle("PdfUA2 Title");
        }
    }
}